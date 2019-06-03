using System.IO;
using System.Linq;
using UnityEditor;
using UnityLocalize.DataStorage;
using UnityLocalize.Utils;

namespace UnityLocalize.Editor
{
    internal class EditorTranslationInfoRepository
    {
        private readonly TranslationInfoSet _translationInfoSet;

        public EditorTranslationInfoRepository()
        {
            this._translationInfoSet = this.Get();
        }

        public TranslationInfoSet Get()
        {
            return PlayerPrefsHelper
                       .LoadFromPrefs<TranslationInfoSet>(TranslationInfoSet.TRANSLATION_INFO_SET);
        }

        public void CreateModel()
        {
            PlayerPrefsHelper
                .SaveToPrefs(TranslationInfoSet.TRANSLATION_INFO_SET, new TranslationInfoSet());
        }

        public void RemoveTranslation(TranslationInfo translationInfo)
        {
            this._translationInfoSet.Translations.RemoveAll(e => e.GUID == translationInfo.GUID);

            PlayerPrefsHelper.SaveToPrefs(TranslationInfoSet.TRANSLATION_INFO_SET, this._translationInfoSet);

            string translationDirectory = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, translationInfo.TranslationName);
            string softDeletedTranslationDirectory = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, "~" + translationInfo.TranslationName);

            Directory.Move(translationDirectory, softDeletedTranslationDirectory);
            AssetDatabase.Refresh();
        }

        public void AddTranslation(TranslationInfo translation)
        {
            this._translationInfoSet.Translations.Add(translation);

            PlayerPrefsHelper.SaveToPrefs(TranslationInfoSet.TRANSLATION_INFO_SET, this._translationInfoSet);

            string translationDirectory = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, translation.TranslationName);

            Directory.CreateDirectory(translationDirectory);
            AssetDatabase.Refresh();
        }

        public bool IsContain(TranslationInfo translationInfo)
        {
            return this._translationInfoSet.Translations.Any(t => t.GUID == translationInfo.GUID);
        }
    }
}
