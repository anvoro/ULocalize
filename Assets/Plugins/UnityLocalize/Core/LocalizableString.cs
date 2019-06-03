using System;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize
{
    [Serializable]
    public class LocalizableString
    {
        [HideInInspector]
        [SerializeField]
        private string[] _defaultStrings;

        [HideInInspector]
        [SerializeField]
        private string _id;

        public LocalizableString()
        {
            this._id = Guid.NewGuid().ToString();
            this._defaultStrings = new string[1];
        }

        public LocalizableString(string id, string[] strings)
        {
            this._id = id;
            this._defaultStrings = strings;
        }

        /// <summary>
        /// Returns translated into the selected language.
        /// </summary>
        /// <returns>Translated strings.</returns>
        public virtual string GetString()
        {
            if (LocalizationManager.IsDefaultCulture)
            {
                return this._defaultStrings[0];
            }

            string[] translations = LocalizationCache.GetStringsById(this._id);

            return translations?[0] ?? this._defaultStrings[0];
        }

        /// <summary>
        /// Returns translated into the selected language.
        /// </summary>
        /// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
        /// <returns>Translated strings.</returns>
        public virtual string GetString(params object[] args)
        {
            return string.Format(LocalizationManager.CurrentCulture, this.GetString(), args);
        }

        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation.
        /// </summary>
        /// <param name="n">Value that determines the plural form.</param>
        /// <returns>Translated strings.</returns>
        public virtual string GetPString(int n)
        {
            int pluralIndex = LocalizationManager.PluralForm.EvaluatePlural(n);

            if (pluralIndex < 0 || pluralIndex >= LocalizationManager.PluralForm.PluralsCount)
            {
                throw new IndexOutOfRangeException(
                    $"Calculated plural form index ({pluralIndex}) is out of allowed range (0~{LocalizationManager.PluralForm.PluralsCount - 1}).");
            }

            if (LocalizationManager.IsDefaultCulture)
            {
                return this._defaultStrings[pluralIndex];
            }

            string[] translations = LocalizationCache.GetStringsById(this._id);

            if (translations == null || translations.Length <= pluralIndex)
            {
                return (n == 1) ? this._defaultStrings[0] : this._defaultStrings[1];
            }

            return translations[pluralIndex];
        }

        /// <summary>
        /// Returns the plural form for <paramref name="n"/> of the translation.
        /// </summary>
        /// <param name="n">Value that determines the plural form.</param>
        /// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
        /// <returns>Translated strings.</returns>
        public virtual string GetPString(int n, params object[] args)
        {
            return string.Format(LocalizationManager.CurrentCulture, this.GetPString(n), args);
        }

#if UNITY_EDITOR
        public string[] DefaultStrings => this._defaultStrings;
        public string Id => this._id;

        public static implicit operator StoredString(LocalizableString ls)
        {
            return new StoredString(ls._id, ls._defaultStrings);
        }
#endif
    }
}
