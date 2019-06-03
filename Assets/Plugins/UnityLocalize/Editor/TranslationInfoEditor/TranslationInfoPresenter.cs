using System;
using PluginsEditorEnvironment;
using UnityEditor;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class TranslationInfoPresenter : IPresenter
    {
        private readonly TranslationInfoValidator _translationInfoValidator = new TranslationInfoValidator();

        private readonly EditorTranslationInfoRepository _translationInfoRepository;
        private readonly TranslationIOUnit _translationIOUnit;

        private TranslationInfoWindow _translationInfoWindow;

        public TranslationInfoPresenter(EditorTranslationInfoRepository translationInfoRepository, TranslationIOUnit translationIOUnit)
        {
            if (translationInfoRepository == null)
                throw new ArgumentNullException(nameof(EditorTranslationInfoRepository));

            if (translationIOUnit == null)
                throw new ArgumentNullException(nameof(TranslationIOUnit));

            this._translationIOUnit = translationIOUnit;
            this._translationInfoRepository = translationInfoRepository;
        }

        public void Run()
        {
            this._translationInfoWindow = EditorWindow.GetWindow<TranslationInfoWindow>("Translations");
            this._translationInfoWindow.Init(this);

            foreach (TranslationInfo ti in this._translationInfoRepository.Get().Translations)
            {
                this._translationInfoWindow.InfoListView.AddTranslation(ti, this);
            }
        }

        public void OnClose()
        {
            throw new NotImplementedException();
        }

        public void AddTranslation()
        {
            var newTranslation = new TranslationInfo(this._translationInfoWindow.TranslationNameToAdd, this._translationInfoWindow.CultureToAdd);

            if (!this._translationInfoValidator.Validate(newTranslation, out string error))
            {
                EditorUtility.DisplayDialog("Translation adding error", error, "OK");

                return;
            }

            if (this._translationInfoRepository.IsContain(newTranslation)) 
            {
                EditorUtility.DisplayDialog("Translation adding error",
                    $"Translation with Name: '{this._translationInfoWindow.TranslationNameToAdd}' already exist.", "OK");

                return;
            }

            this._translationInfoRepository.AddTranslation(newTranslation);
            
            this._translationInfoWindow.InfoListView.AddTranslation(newTranslation, this);

            this._translationInfoWindow.TranslationNameToAdd = string.Empty;
            this._translationInfoWindow.CultureToAdd = Culture.en_GB;
        }

        public void DeleteTranslation(TranslationInfoEditor editor)
        {
            if (!EditorUtility.DisplayDialog("Delete translation",
                $"Delete translation with Name: '{editor.TranslationInfo.TranslationName}'?", "OK", "Cancel"))
                return;

            this._translationInfoRepository.RemoveTranslation(editor.TranslationInfo);
            this._translationInfoWindow.InfoListView.RemoveTranslation(editor.TranslationInfo);
        }

        public void DoExport(TranslationInfo translationInfo)
        {
            this._translationIOUnit.DoExport(translationInfo);
        }

        public void DoImport(TranslationInfo translationInfo)
        {
            this._translationIOUnit.DoImport(translationInfo);
        }
    }
}
