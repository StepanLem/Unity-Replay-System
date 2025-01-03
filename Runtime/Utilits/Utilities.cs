using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class Utilities
    {
        public static bool TryGetComponentInAllParents<T>(Transform startSearchTransform, out T component) where T : Component
        {
            var currentCheckingTransform = startSearchTransform;

            while (currentCheckingTransform != null)
            {
                var hasComponent = currentCheckingTransform.TryGetComponent<T>(out component);
                if (hasComponent)
                {
                    return true;
                }

                currentCheckingTransform = currentCheckingTransform.parent;
            }

            component = null;
            return false;
        }
    }
}