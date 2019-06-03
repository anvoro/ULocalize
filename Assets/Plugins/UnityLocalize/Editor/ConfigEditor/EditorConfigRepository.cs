using UnityLocalize.DataStorage;
using UnityLocalize.Utils;

namespace UnityLocalize.Editor
{
    internal class EditorConfigRepository
    {
        public LocalizationConfig Get()
        {
            return PlayerPrefsHelper
                       .LoadFromPrefs<LocalizationConfig>(LocalizationConfig.LOCALIZATION_CONFIG) ?? new LocalizationConfig();
        }

        public void Save(LocalizationConfig config)
        {
            PlayerPrefsHelper.SaveToPrefs(LocalizationConfig.LOCALIZATION_CONFIG, config);
        }

        public EditorInternalConfig GetInternalConfig()
        {
            return PlayerPrefsHelper
                       .LoadFromPrefs<EditorInternalConfig>(EditorInternalConfig.INTERNAL_LOCALIZATION_CONFIG) ?? new EditorInternalConfig();
        }

        public void SaveInternalConfig(EditorInternalConfig config)
        {
            PlayerPrefsHelper.SaveToPrefs(EditorInternalConfig.INTERNAL_LOCALIZATION_CONFIG, config);
        }
    }
}
