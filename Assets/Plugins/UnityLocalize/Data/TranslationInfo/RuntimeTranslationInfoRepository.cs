using UnityLocalize.Utils;

namespace UnityLocalize.DataStorage
{
    internal class RuntimeTranslationInfoRepository
    {
        public TranslationInfo[] GetAll()
        {
            return PlayerPrefsHelper
                .LoadFromPrefs<TranslationInfoSet>(TranslationInfoSet.TRANSLATION_INFO_SET).Translations
                .ToArray();
        }
    }
}
