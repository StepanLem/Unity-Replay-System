using UnityEditor;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    [CustomPropertyDrawer(typeof(PersistentPathSelectorAttribute))]
    public class PersistentPathSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                Rect textFieldPosition = position;
                textFieldPosition.width -= 80;

                property.stringValue = EditorGUI.TextField(textFieldPosition, label, property.stringValue);

                Rect buttonPosition = position;
                buttonPosition.x += textFieldPosition.width;
                buttonPosition.width = 80;

                if (GUI.Button(buttonPosition, "Browse"))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("Select Folder", Application.persistentDataPath, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        if (folderPath.StartsWith(Application.persistentDataPath))
                        {
                            property.stringValue = folderPath.Substring(Application.persistentDataPath.Length + 1);
                        }
                        else
                        {
                            Debug.LogWarning("The folder must be inside Application.persistentDataPath!");
                        }
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use PersistentPathSelector with string.");
            }

            EditorGUI.EndProperty();
        }
    }
}
