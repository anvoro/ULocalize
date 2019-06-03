using EditorDraw;
using UnityEditor;
using UnityEngine;

namespace UnityLocalize.Editor
{
    [CustomPropertyDrawer(typeof(LocalizableString))]
    internal class LocalizableStringDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? 84f + property.FindPropertyRelative("_defaultStrings").arraySize * 110 : 40;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, GUIEx.LabelStyle(FontStyle.Bold));

            Rect foldoutRect = new Rect(position.x, position.y + 20, position.width, 20);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, new GUIContent("Show Localizable string"));
            if (property.isExpanded)
            {
                SerializedProperty arrayProperty = property.FindPropertyRelative("_defaultStrings");

                Rect buttonAddRect = new Rect(position.x, position.y + 40, position.width, 20);
                if (GUI.Button(buttonAddRect, "Add plural form"))
                {
                    arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
                }

                Rect buttonRemoveRect = new Rect(position.x, position.y + 64, position.width, 20);
                if (GUI.Button(buttonRemoveRect, "Remove last plural form"))
                {
                    if (arrayProperty.arraySize > 1)
                    {
                        arrayProperty.DeleteArrayElementAtIndex(arrayProperty.arraySize - 1);
                    }
                }

                this.ListIterator(position, arrayProperty);
            }

            EditorGUI.EndProperty();
        }

        private void ListIterator(Rect position, SerializedProperty listProperty)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);

                Rect labelRect = new Rect(position.x, position.y + 84 + 110 * i, position.width, 20);
                GUI.Label(labelRect, i == 0 ? "Single form" : $"Plural form: {i}");

                Rect textRect = new Rect(position.x, position.y + 104 + 110 * i, position.width - 10, 80);
                EditorGUI.PropertyField(textRect, elementProperty, GUIContent.none);
            }
            EditorGUI.indentLevel--;
        }
    }
}