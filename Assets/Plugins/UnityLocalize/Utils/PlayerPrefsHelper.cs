using UnityEngine;

namespace UnityLocalize.Utils
{
    public static class PlayerPrefsHelper
    {
        public static void SaveToPrefs<T>(string prefsKey, T objectToSave)
            where T : class, new()
        {
            string json = JsonUtility.ToJson(objectToSave);
            PlayerPrefs.SetString(prefsKey, json);
        }

        public static T LoadFromPrefs<T>(string prefsKey)
        {
            string json = PlayerPrefs.GetString(prefsKey);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonUtility.FromJson<T>(json);
        }
    }
}
