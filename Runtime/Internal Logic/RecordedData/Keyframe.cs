using OdinSerializer;
using System;

namespace StepanLem.ReplaySystem
{
    [Serializable]
    public struct Keyframe<T>
    {
        public Keyframe(T value, float time)
        {
            Value = value;
            Time = time;
        }

        [NonSerialized, OdinSerialize]
        public T Value;

        public float Time;
    }
}