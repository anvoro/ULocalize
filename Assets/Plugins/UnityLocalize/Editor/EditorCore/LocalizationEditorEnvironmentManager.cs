using System;
using System.IO;
using UnityEditor;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    [InitializeOnLoad]
    internal static class LocalizationEditorEnvironmentManager
    {
        private const string TEMP_FOLDER_NAME = "~localizationTemp";

        private const string EDITOR_RESOURCES_PATH = "Assets/Plugins/UnityLocalize/Editor/Resources";

        static LocalizationEditorEnvironmentManager()
        {
            string projectDir = Environment.CurrentDirectory;
            TempFolder = Path.Combine(projectDir, TEMP_FOLDER_NAME);

            CheckEnvironmentFolders();
            CheckTranslationStorageExist();
        }

        [MenuItem("Window/UnityLocalize/Localization Config")]
        public static void ShowConfigWindow()
        {
            var configRepository = new EditorConfigRepository();

            var defaultExportUnit = new DefaultLocalizationExportUnit(new DefaultLocalizationRepository(), configRepository, new InlineStringsRepository());

            new LocalizationConfigPresenter(
                configRepository, defaultExportUnit)
                .Run();
        }

        [MenuItem("Window/UnityLocalize/Translations")]
        public static void ShowTranslationsWindow()
        {
            var config = new EditorConfigRepository().GetInternalConfig();
            var defaultLocalizationRepository = new DefaultLocalizationRepository();
            var editorTranslationIORepository = new EditorTranslationIORepository(new TranslationStorageProvider());

            var ioUnit = new TranslationIOUnit(config, defaultLocalizationRepository, editorTranslationIORepository);

            new TranslationInfoPresenter(
                new EditorTranslationInfoRepository(), ioUnit)
                .Run();
        }

        [MenuItem("Window/UnityLocalize/Inline strings")]
        public static void ShowInlineStringsWindow()
        {
            new InlineStringsPresenter(
                new InlineStringsRepository())
                .Run();
        }

        public static string TempFolder { get; }

        public static string DataStoragePath { get; private set; }

        private static void CheckEnvironmentFolders()
        {
            DataStoragePath = new TranslationStorageProvider().GetStoragePath();

            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }

            if (!Directory.Exists(DataStoragePath))
            {
                Directory.CreateDirectory(DataStoragePath);
            }

            if (!Directory.Exists(EDITOR_RESOURCES_PATH))
            {
                Directory.CreateDirectory(EDITOR_RESOURCES_PATH);
            }
        }

        private static void CheckTranslationStorageExist()
        {
            var translationRepo = new EditorTranslationInfoRepository();
            TranslationInfoSet set = translationRepo.Get();

            if (set == null)
            {
                translationRepo.CreateModel();
            }
        }
    }
}
