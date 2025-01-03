using System;

namespace StepanLem.ReplaySystem
{
    internal class ValueTrack<T> : ITimelineTrack
    {
        public ValueTrack(ValueRecord<T> record, Action<ValueRecord<T>, int, float> keyframeActivationHandler)
        {
            Record = record;
            KeyframeActivationHandler = keyframeActivationHandler;
        }

        public ValueRecord<T> Record { get; }

        public Action<ValueRecord<T>, int, float> KeyframeActivationHandler { get; }

        public int KeyframesCount => Record.Keyframes.Count;

        public float GetKeyframeTime(int index) => Record.Keyframes[index].Time;

        public void HandleKeyframeActivation(int indexInTrack, float currentTimeSinceReplayStart)
        {
            KeyframeActivationHandler(Record, indexInTrack, currentTimeSinceReplayStart);
        }
    }

    internal interface ITimelineTrack
    {
        public int KeyframesCount { get; }
        public float GetKeyframeTime(int index);
        public void HandleKeyframeActivation(int indexInTrack, float currentTimeSinceReplayStart);
    }
}