using System;
using EditorDraw;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;
using UnityLocalize.Exceptions;

namespace UnityLocalize.Editor
{
    internal class LocalizationConfigWindow : EditorWindow
    {
        private LocalizationConfigPresenter _localizationConfigPresenter;

        private Culture _defaultCulture;
        private string _sceneExcludePrefix;

        private int _exportProviderIndex;
        private int _importProviderIndex;

        private string[] _exportProviders;
        private string[] _importProviders;

        private bool _isInit;

        public Culture DefaultCulture
        {
            get => this._defaultCulture;
            set => this._defaultCulture = value;
        }

        public string SceneExcludePrefix
        {
            get => this._sceneExcludePrefix;
            set => this._sceneExcludePrefix = value;
        }

        public string SelectedExportProvider => this._exportProviders[this._exportProviderIndex];
        public string SelectedImportProvider => this._importProviders[this._importProviderIndex];

        public string[] ExportProviders
        {
            set => this._exportProviders = value;
        }

        public string[] ImportProviders
        {
            set => this._importProviders = value;
        }

        public int ExportProviderIndex
        {
            set => this._exportProviderIndex = value;
        }

        public int ImportProviderIndex
        {
            set => this._importProviderIndex = value;
        }

        public void Init(LocalizationConfigPresenter presenter)
        {
            if(presenter == null)
                throw new ArgumentNullException(nameof(LocalizationConfigPresenter));

            this._localizationConfigPresenter = presenter;

            this._isInit = true;
        }

        private void OnGUI()
        {
            if (!this._isInit)
                throw new NotInitializedException(typeof(LocalizationConfigWindow));

            EDraw.BeginDraw()
                .DrawSpace()
                .S_DrawEnumPopup(ref this._defaultCulture, "Default culture: ")
                .DrawSpace()
                .S_TextField(ref this._sceneExcludePrefix, "Scene exclude prefix: ")
                .S_DrawStringsPopup(ref this._exportProviderIndex, this._exportProviders, "Export provider: ")
                .S_DrawStringsPopup(ref this._importProviderIndex, this._importProviders, "Import provider: ");

            EDraw.BeginDraw()
                .S_Button("Get scenes localization", this._localizationConfigPresenter.GetScenesLocalization)
                .S_Button("Get assets localization", this._localizationConfigPresenter.GetAssetsLocalization)
                .S_Button("Get inline strings localizationn", this._localizationConfigPresenter.GetInlineStringsLocalization);

            EDraw.BeginDraw()
                .DrawSpace(14)
                .S_BeginHorizontal(GUI.skin.box)
                .S_BeginVertical()
                .S_Button("Save config", this._localizationConfigPresenter.SaveConfig, color: Color.green)
                .EndVertical()
                .S_BeginVertical()
                .S_Button("Reset config", this._localizationConfigPresenter.ResetConfig, lossFocus: true, color: Color.red)
                .EndVertical()
                .EndHorizontal();

            EDraw.BeginDraw()
                .S_BeginHorizontal(GUI.skin.box)
                .S_Button("Open localization temp folder", this._localizationConfigPresenter.OpenTempFolder)
                .EndHorizontal()
                .S_BeginHorizontal(GUI.skin.box)
                .S_Button("Reset types to Localization", this._localizationConfigPresenter.SetTypesToLocalization, color: Color.gray)
                .EndHorizontal();
        }
    }
}
