using System;
using System.Collections.Generic;
using UnityEditor;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal abstract class LocalizationProviderBase
    {
        public List<Type> TypesToLocalization { get; set; }

        public LocalizationProviderCache LocalizationProviderCache { get; set; }

        public abstract List<StoredString> GetLocalization();

        protected void GetLocalizationInternal(IEnumerable<UnityEngine.Object> localizables, ref List<StoredString> strings)
        {
            foreach (UnityEngine.Object localizable in localizables)
            {
                foreach (var get in this.LocalizationProviderCache.GetCachedMethods(localizable.GetType()))
                {
                    LocalizableString ls = (LocalizableString)get.Method.Invoke(localizable, null);
                    strings.Add(ls);
                }

                EditorUtility.SetDirty(localizable);
            }
        }
    }
}
