using OdinSerializer;
using System;
using System.Collections.Generic;

namespace StepanLem.ReplaySystem
{
    [Serializable]
    public class ValueRecord<T> : IValueRecord
    {
        [NonSerialized, OdinSerialize]
        public List<Keyframe<T>> Keyframes = new();

        public Keyframe<T> this[int index] => Keyframes[index];

        public int KeyframesCount => Keyframes.Count;

        public float GetKeyframeTime(int index)
        {
            return Keyframes[index].Time;
        }

        public List<IValueRecord> GetValueRecords()
        {
            return new List<IValueRecord> { this };
        }
    }

    public interface IValueRecord
    {
        public int KeyframesCount { get; }
        public float GetKeyframeTime(int index);
    }
}