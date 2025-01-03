using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class FileNameSelectorAttribute : PropertyAttribute
    {
        public string StartPathProperty;

        public FileNameSelectorAttribute(string startPathProperty)
        {
            StartPathProperty = startPathProperty;
        }
    }
}
