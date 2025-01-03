using UnityEditor;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    [CustomPropertyDrawer(typeof(ProjectFolderSelectorAttribute))]
    public class ProjectFolderSelectorDrawer : PropertyDrawer
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
                    string folderPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        if (folderPath.StartsWith(Application.dataPath))
                        {
                            property.stringValue = "Assets" + folderPath.Substring(Application.dataPath.Length);
                        }
                        else
                        {
                            Debug.LogWarning("The folder must be located inside the project folder!");
                        }
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use FolderSelector with string.");
            }

            EditorGUI.EndProperty();
        }
    }
}