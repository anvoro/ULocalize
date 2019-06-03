using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityLocalize.DataStorage;
using Object = UnityEngine.Object;

namespace UnityLocalize.Editor
{
    internal class ScenesLocalizationProvider : LocalizationProviderBase
    {
        private const string SCENES_PATH = @"Assets\Scenes\";

        private readonly EditorInternalConfig _internalConfig;

        public ScenesLocalizationProvider(EditorInternalConfig internalConfig)
        {
            if (internalConfig == null)
                throw new NullReferenceException(nameof(EditorInternalConfig));

            this._internalConfig = internalConfig;
        }

        public override List<StoredString> GetLocalization()
        {
            List<StoredString> strings = new List<StoredString>();

            EditorSceneManager.SaveOpenScenes();

            this.GetScenesLoc(SCENES_PATH, ref strings);

            string[] allScenesDirs = Directory.GetDirectories(SCENES_PATH, "*", SearchOption.AllDirectories);
            foreach (string dir in allScenesDirs.Where(p => !p.Contains(this._internalConfig.SceneExcludePrefix)))
            {
                this.GetScenesLoc(dir, ref strings);
            }

            return strings;
        }

        private void GetScenesLoc(string path, ref List<StoredString> strings)
        {
            var scenesNames = this.GetScenesNames(path);

            foreach (string scenesName in scenesNames.Where(s => !s.StartsWith(this._internalConfig.SceneExcludePrefix)))
            {
                this.GetSceneLoc(scenesName, ref strings);
            }
        }

        private IEnumerable<string> GetScenesNames(string path)
        {
            return Directory.GetFiles(path, "*.unity");
        }

        private void GetSceneLoc(string scenePath, ref List<StoredString> strings)
        {
            EditorSceneManager.OpenScene(scenePath);

            List<MonoBehaviour> localizables = new List<MonoBehaviour>();

            foreach (Type type in this.TypesToLocalization.Where(t => t.IsSubclassOf(typeof(MonoBehaviour))))
            {
                IEnumerable<MonoBehaviour> findResult = Object.FindObjectsOfType(type).Cast<MonoBehaviour>();
                localizables.AddRange(findResult);
            }

            this.GetLocalizationInternal(localizables, ref strings);

            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
