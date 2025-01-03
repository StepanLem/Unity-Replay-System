using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionalSourceField;
        public bool HideIfFalse;

        public ShowIfAttribute(string conditionalSourceField, bool equalBool)
        {
            ConditionalSourceField = conditionalSourceField;
            HideIfFalse = equalBool;
        }
    }
}