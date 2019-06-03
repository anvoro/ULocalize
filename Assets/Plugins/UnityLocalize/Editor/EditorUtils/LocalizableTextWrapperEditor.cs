using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityLocalize.Utils;

namespace UnityLocalize.Editor
{
    [CustomEditor(typeof(LocalizableTextWrapper))]
    public class LocalizableTextWrapperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LocalizableTextWrapper localizableTextWrapper = this.target as LocalizableTextWrapper;

            if (GUILayout.Button("Apply Localized Text"))
            {
                if (localizableTextWrapper != null)
                {
                    localizableTextWrapper.Text.text = localizableTextWrapper.LocalizableString.DefaultStrings[0];

                    EditorUtility.SetDirty(localizableTextWrapper.Text);
                    localizableTextWrapper.Text.Rebuild(CanvasUpdate.Layout);
                    SceneView.RepaintAll();

                    EditorSceneManager.MarkAllScenesDirty();
                    EditorSceneManager.SaveOpenScenes();
                }
            }

            base.OnInspectorGUI();
        }
    }
}
