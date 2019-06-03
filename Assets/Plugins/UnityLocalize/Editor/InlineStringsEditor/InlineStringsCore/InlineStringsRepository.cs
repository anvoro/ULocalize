using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class InlineStringsRepository
    {
        private readonly InlineStringsSet _inlineStringsSet;

        public InlineStringsRepository()
        {
            this._inlineStringsSet = this.Get();
        }

        public InlineStringsSet Get()
        {
            string inlineStringsFile = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, InlineStringsSet.INLINE_STRINGS_STOGARE);

            InlineStringsSet result;

            if (!File.Exists(inlineStringsFile))
            {
                result = new InlineStringsSet();
            }
            else
            {
                string json = File.ReadAllText(inlineStringsFile);
                result = JsonUtility.FromJson<InlineStringsSet>(json);
            }

            return result;
        }

        public void AddInlineString(InlineString inlineString)
        {
            this._inlineStringsSet.InlineStrings.Add(inlineString);
        }

        public void UpdateInlineString(InlineString inlineString)
        {
            var oldInlineString = this._inlineStringsSet.InlineStrings.Find(e => e.GUID == inlineString.GUID);

            oldInlineString.Id = inlineString.Id;
            oldInlineString.Strings = inlineString.Strings;
        }

        public void RemoveInlineString(InlineString inlineString)
        {
            InlineString stringToRemove = this._inlineStringsSet.InlineStrings.Find(e => e.GUID == inlineString.GUID);
            this._inlineStringsSet.InlineStrings.Remove(stringToRemove);
        }

        public void Save()
        {
            if (this._inlineStringsSet?.InlineStrings == null)
                throw new NullReferenceException(nameof(InlineStringsSet));

            string json = JsonUtility.ToJson(this._inlineStringsSet);
            string path = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, InlineStringsSet.INLINE_STRINGS_STOGARE);
            File.WriteAllText(path, json);
        }

        public bool IsContain(string id)
        {
            return this._inlineStringsSet.InlineStrings.Any(s => s.Id == id);
        }
    }
}
