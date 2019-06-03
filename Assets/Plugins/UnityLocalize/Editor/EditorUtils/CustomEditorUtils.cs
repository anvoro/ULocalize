using UnityEditor;
using UnityEngine;

namespace UnityLocalize.Editor
{
    internal static class CustomEditorUtils
    {
        public static void SaveAssetWithStrongName(ScriptableObject so, string path, string fileName)
        {
            so.name = fileName;
            AssetDatabase.CreateAsset(so, path + '/' + so.name + ".asset");

            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void UpdateAssetDatabase(UnityEngine.Object uObj)
        {
            EditorUtility.SetDirty(uObj);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
