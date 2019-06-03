using UnityEditor;
using UnityEngine;

namespace UnityLocalize.Editor
{
    internal static class EditorPrefsHelper
    {
        public static void SaveToEditorPrefs<T>(string prefsKey, T objectToSave)
            where T : class, new()
        {
            string json = JsonUtility.ToJson(objectToSave);
            EditorPrefs.SetString(prefsKey, json);
        }

        public static T LoadFromEditorPrefs<T>(string prefsKey)
        {
            string json = EditorPrefs.GetString(prefsKey);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonUtility.FromJson<T>(json);
        }
    }
}
