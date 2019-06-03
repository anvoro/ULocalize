using System;
using System.Collections.Generic;
using System.Linq;
using UnityLocalize.DataStorage;
using UnityLocalize.Exceptions;

namespace UnityLocalize.Editor
{
    [Serializable]
    internal class DefaultLocalizationSet
    {
        public List<StoredString> Strings = new List<StoredString>();

        [NonSerialized]
        private readonly HashSet<string> _ids = new HashSet<string>();

        [NonSerialized]
        private bool _isInit;

        public void Init()
        {
            if (this._isInit)
                throw new Exception("DefaultLocalizationSet already initialized.");

            foreach (StoredString s in this.Strings)
            {
                this._ids.Add(s.Id);
            }

            this._isInit = true;
        }

        public void Add(StoredString storedString)
        {
            if (!this._isInit)
                throw new NotInitializedException(typeof(DefaultLocalizationSet));

            if (string.IsNullOrEmpty(storedString.Id))
                throw new NullReferenceException(nameof(StoredString.Id));

            if (storedString.Strings == null)
                throw new NullReferenceException($"LocalizableString with ID:'{storedString.Id}' is not contain DefaultStrings.");

            if (this._ids.Contains(storedString.Id))
            {
                this.Strings.First(s => s.Id == storedString.Id).Strings = storedString.Strings;

                return;
            }

            this.Strings.Add(storedString);
            this._ids.Add(storedString.Id);
        }
    }
}
