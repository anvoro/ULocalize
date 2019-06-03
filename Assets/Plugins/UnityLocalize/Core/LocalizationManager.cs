using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityLocalize.DataStorage;
using UnityLocalize.Exceptions;
using UnityLocalize.Plural;

namespace UnityLocalize
{
    public static class LocalizationManager
    {
        private static TranslationDatabase translationDatabase;
        private static LocalizationConfig config;
        private static CultureInfo defaultCulture;
        private static TranslationInfo[] translations;

        private static bool isInit;

        public static TranslationInfo CurrentTranslation { get; private set; }
        public static CultureInfo CurrentCulture => CurrentTranslation?.CultureInfo ?? defaultCulture;
        public static bool IsDefaultCulture { get; private set; } = true;
        public static IPluralForm PluralForm { get; private set; }

        /// <summary>
        /// Get available translations
        /// </summary>
        public static TranslationInfo[] Translations
        {
            get
            {
                if(!isInit)
                    throw new NotInitializedException(typeof(LocalizationManager));

                if (translations == null)
                    throw new NotInitializedException(typeof(LocalizationManager));

                return translations;
            }
        }

        /// <summary>
        /// Call it before use any other method
        /// </summary>
        public static void Init()
        {
            config = new RuntimeConfigRepository().Get();

            if (config == null)
                throw new NullReferenceException(nameof(LocalizationConfig));

            translationDatabase = new TranslationDatabase(
                new TranslationRepository(
                    new TranslationStorageProvider()));

            defaultCulture = CultureMap.Get(config.DefaultCulture);
            translations = new RuntimeTranslationInfoRepository().GetAll();

            if (translations.Length == 0)
            {
                Debug.LogWarning("No cultures found.");
            }

            isInit = true;

            SetDefaultCulture();
        }

        public static void SetDefaultCulture()
        {
            if (!isInit)
                throw new NotInitializedException(typeof(LocalizationManager));

            CurrentTranslation = null;
            IsDefaultCulture = true;

            PluralForm = PluralFormGenerator.CreateForm(defaultCulture);
        }

        /// <summary>
        /// Set any translation from 'Translations' property
        /// </summary>
        /// <param name="translation"></param>
        public static void SetCulture(TranslationInfo translation)
        {
            if (!isInit)
                throw new NotInitializedException(typeof(LocalizationManager));

            if (!Translations.Contains(translation))
                throw new CultureNotFoundException($"Invalid translation: '{translation.TranslationName}', with culture: '{translation.CultureInfo.Name}'");

            if (CurrentTranslation == translation)
            {
                Debug.LogWarning("You are trying to switch to the current culture.");
                return;
            }

            CurrentTranslation = translation;
            IsDefaultCulture = false;

            PluralForm = PluralFormGenerator.CreateForm(CurrentTranslation.CultureInfo);

            var translationDictionary = translationDatabase.GetTranslationDictionary(CurrentTranslation);
            LocalizationCache.Init(translationDictionary);
        }
    }
}