using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityLocalize.DataStorage;
using Object = UnityEngine.Object;

namespace UnityLocalize.Editor
{
    internal class AssetsLocalizationProvider : LocalizationProviderBase
    {
        public override List<StoredString> GetLocalization()
        {
            List<StoredString> strings = new List<StoredString>();

            foreach (Type type in this.TypesToLocalization)
            {
                List<Object> localizables = new List<Object>();

                string[] localizableAssetsGUIDs = AssetDatabase.FindAssets($"t:{type.Name}");

                localizables.AddRange(
                    localizableAssetsGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid))));
                
                this.GetLocalizationInternal(localizables, ref strings);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return strings;
        }
    }
}
