using System;
using System.IO;
using UnityEngine;

namespace UnityLocalize.DataStorage
{
    internal class TranslationRepository
    {
        private readonly TranslationStorageProvider _translationStorageProvider;

        public TranslationRepository(TranslationStorageProvider translationStorageProvider)
        {
            if (translationStorageProvider == null)
                throw new NullReferenceException(nameof(TranslationStorageProvider));

            this._translationStorageProvider = translationStorageProvider;
        }

        public Translation Get(TranslationInfo translationInfo)
        {
            string defaultLocFile = Path.Combine(this._translationStorageProvider.GetStoragePath(), translationInfo.TranslationName + ".json");
            string json = File.ReadAllText(defaultLocFile);

            return JsonUtility.FromJson<Translation>(json);
        }
    }
}
