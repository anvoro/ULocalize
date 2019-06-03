using UnityLocalize.Utils;

namespace UnityLocalize.DataStorage
{
    internal class RuntimeConfigRepository
    {
        public LocalizationConfig Get()
        {
            return PlayerPrefsHelper
                       .LoadFromPrefs<LocalizationConfig>(LocalizationConfig.LOCALIZATION_CONFIG) ?? new LocalizationConfig();
        }
    }
}
