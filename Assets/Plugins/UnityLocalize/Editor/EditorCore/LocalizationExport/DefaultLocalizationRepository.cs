using System;
using System.IO;
using UnityEngine;

namespace UnityLocalize.Editor
{
    internal class DefaultLocalizationRepository
    {
        public DefaultLocalizationSet Get()
        {
            string defaultLocFile = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, EditorInternalConfig.DEFAULT_LOCALIZATION_FILE_NAME);

            DefaultLocalizationSet result;

            if (!File.Exists(defaultLocFile))
            {
                result = new DefaultLocalizationSet();
            }
            else
            {
                string json = File.ReadAllText(defaultLocFile);
                result = JsonUtility.FromJson<DefaultLocalizationSet>(json);
            }

            result.Init();

            return result;
        }

        public void Save(DefaultLocalizationSet localizationSet)
        {
            if (localizationSet == null)
                throw new NullReferenceException(nameof(DefaultLocalizationSet));

            string json = JsonUtility.ToJson(localizationSet);
            string path = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, EditorInternalConfig.DEFAULT_LOCALIZATION_FILE_NAME);
            File.WriteAllText(path, json);

            Debug.Log("Default localization set saved!");
        }
    }
}
