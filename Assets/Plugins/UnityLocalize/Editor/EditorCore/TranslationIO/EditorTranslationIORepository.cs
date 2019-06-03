using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class EditorTranslationIORepository
    {
        private readonly TranslationStorageProvider _translationStorageProvider;

        public EditorTranslationIORepository(TranslationStorageProvider translationStorageProvider)
        {
            if (translationStorageProvider == null)
                throw new NullReferenceException(nameof(TranslationStorageProvider));

            this._translationStorageProvider = translationStorageProvider;
        }

        public void Save(TranslationInfo translationInfo, Translation translation)
        {
            string json = JsonUtility.ToJson(translation);
            string path = Path.Combine(LocalizationEditorEnvironmentManager.DataStoragePath, translationInfo.TranslationName + ".json");
            File.WriteAllText(path, json);

            AssetDatabase.Refresh();

            Debug.Log($"Translation: '{translationInfo.TranslationName}' import - Complete.");
        }

        public Translation Get(TranslationInfo translationInfo)
        {
            string defaultLocFile = Path.Combine(this._translationStorageProvider.GetStoragePath(), translationInfo.TranslationName + ".json");
            string json = File.ReadAllText(defaultLocFile);

            return JsonUtility.FromJson<Translation>(json);
        }
    }
}
