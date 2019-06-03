using System;
using System.Diagnostics;
using PluginsEditorEnvironment;
using UnityEditor;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class LocalizationConfigPresenter : IPresenter
    {
        private readonly string[] _exportProviders;
        private readonly string[] _importProviders;

        private readonly DefaultLocalizationExportUnit _localizationExportUnit;
        private readonly EditorConfigRepository _configRepository;

        private LocalizationConfigWindow _localizationConfigWindow;

        public LocalizationConfigPresenter(EditorConfigRepository repository, DefaultLocalizationExportUnit defaultLocalizationExportUnit)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(EditorConfigRepository));

            if (defaultLocalizationExportUnit == null)
                throw new ArgumentNullException(nameof(DefaultLocalizationExportUnit));

            this._configRepository = repository;
            this._localizationExportUnit = defaultLocalizationExportUnit;

            this._exportProviders = ReflectionHelper.GetAllSubtypesNames(typeof(ITranslationExport));
            this._importProviders = ReflectionHelper.GetAllSubtypesNames(typeof(ITranslationImport));
        }

        public void Run()
        {
            this._localizationConfigWindow = EditorWindow.GetWindow<LocalizationConfigWindow>("Localization Config");
            this._localizationConfigWindow.Init(this);

            this._localizationConfigWindow.ExportProviders = this._exportProviders;
            this._localizationConfigWindow.ImportProviders = this._importProviders;

            this.RepaintView();
        }

        public void OnClose()
        {
            throw new NotImplementedException();
        }

        public void OpenTempFolder()
        {
            Process.Start(LocalizationEditorEnvironmentManager.TempFolder);
        }

        public void SetTypesToLocalization()
        {
            this._localizationExportUnit.SetTypesToLocalization();
        }

        public void GetScenesLocalization()
        {
            this._localizationExportUnit.GetScenesLocalization();
        }

        public void GetAssetsLocalization()
        {
            this._localizationExportUnit.GetAssetsLocalization();
        }

        public void GetInlineStringsLocalization()
        {
            this._localizationExportUnit.GetInlineStringsLocalization();
        }

        public void ResetConfig()
        {
            this._configRepository.Save(new LocalizationConfig());
            this._configRepository.SaveInternalConfig(new EditorInternalConfig());

            this._localizationConfigWindow.ExportProviderIndex = 0;
            this._localizationConfigWindow.ImportProviderIndex = 0;

            this.RepaintView();
        }

        public void SaveConfig()
        {
            var newConfig = new LocalizationConfig
            {
                DefaultCulture = this._localizationConfigWindow.DefaultCulture
            };

            var newInternalConfig = new EditorInternalConfig
            {
                SceneExcludePrefix = this._localizationConfigWindow.SceneExcludePrefix,
                TranslationExportProvider = this._localizationConfigWindow.SelectedExportProvider,
                TranslationImportProvider = this._localizationConfigWindow.SelectedImportProvider
            };

            this._configRepository.Save(newConfig);
            this._configRepository.SaveInternalConfig(newInternalConfig);

            this.RepaintView();
        }

        private void RepaintView()
        {
            LocalizationConfig config = this._configRepository.Get();
            EditorInternalConfig internalConfig = this._configRepository.GetInternalConfig();

            this._localizationConfigWindow.DefaultCulture = config.DefaultCulture;
            this._localizationConfigWindow.SceneExcludePrefix = internalConfig.SceneExcludePrefix;
        }
    }
}
