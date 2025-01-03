using UnityEditor;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(property, condHAtt);

            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(property, condHAtt);

            return enabled ? EditorGUI.GetPropertyHeight(property, label) : 0f;
        }

        private bool GetConditionalHideAttributeResult(SerializedProperty property, ShowIfAttribute condHAtt)
        {
            SerializedProperty sourceProperty = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
            if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Boolean)
            {
                return condHAtt.HideIfFalse ? sourceProperty.boolValue : !sourceProperty.boolValue;
            }

            return true;
        }
    }
}