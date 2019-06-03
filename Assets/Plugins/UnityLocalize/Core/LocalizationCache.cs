using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityLocalize
{
    internal static class LocalizationCache
    {
        private static Dictionary<string, string[]> translationsById;

        public static void Init(Dictionary<string, string[]> translationDictionary)
        {
            translationsById = translationDictionary;
        }

        public static string[] GetStringsById(string id)
        {
            if (translationsById == null)
                throw new NullReferenceException(nameof(LocalizationCache));

            if (translationsById.ContainsKey(id))
                return translationsById[id];

            Debug.LogWarning($"Strings with Id: '{id}' not found.");

            return null;
        }
    }
}
