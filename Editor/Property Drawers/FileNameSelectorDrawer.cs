using UnityEditor;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    [CustomPropertyDrawer(typeof(FileNameSelectorAttribute))]
    public class FileNameSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var attribute = (FileNameSelectorAttribute)base.attribute;

            var targetObject = property.serializedObject.targetObject;
            var startPathField = targetObject.GetType().GetField(attribute.StartPathProperty);

            string startPath = Application.persistentDataPath;
            if (startPathField != null)
            {
                string folderValue = startPathField.GetValue(targetObject) as string;
                if (!string.IsNullOrEmpty(folderValue))
                    startPath = System.IO.Path.Combine(Application.persistentDataPath, folderValue);
            }

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (GUI.Button(position, string.IsNullOrEmpty(property.stringValue) ? "Select File" : property.stringValue))
            {
                string path = EditorUtility.OpenFilePanel("Select File", startPath, "*");
                if (!string.IsNullOrEmpty(path))
                {
                    property.stringValue = System.IO.Path.GetFileName(path);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
