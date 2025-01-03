using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StepanLem.ReplaySystem
{
    public abstract class BaseReplayableValue<T> : MonoBehaviour, IRecordable, IPlaybackable
    {
        public bool Recordable;
        public bool Playbackable;
        public string ValueDataID;

        public MonoTicker RecordingTicker;

        /// <returns>Null if cant create</returns>
        public IRecording CreateRecording()
        {
            if (!Recordable) return null;

            if (RecordingTicker == null)
            {
                Debug.LogError("RecordingTicker == null. Please specify RecordingTicker first.", this);
                return null;
            }

            var recording = new ValueRecording<T>(RecordingTicker, HanldeRecordingTick, ValueDataID);
            return recording;
        }

        public abstract void HanldeRecordingTick(ValueRecord<T> record, float recordingTime);


        public void CreatePlaybackingTracksOnTimeline(RuntimeTimeline timeline, RootRecordWithInfo rootRecord)
        {
            if (!Playbackable) return;

            var iValueRecord = rootRecord.GetValueRecordByID(ValueDataID);

            if (iValueRecord == null)
            {
                Debug.LogWarning($"There is no item with key {ValueDataID} in RootRecordWithInfo.");
                return;
            }

            if (iValueRecord is not ValueRecord<T> valueRecord)
            {
                Debug.LogError($"RootRecordWithInfo item with key {ValueDataID} is not a ValueRecord<{nameof(T)}>.");
                return;
            }

            timeline.CreateTrack<T>(valueRecord, HandleKeyframeActivation);
        }

        public abstract void HandleKeyframeActivation(ValueRecord<T> record, int keyframeIndex, float playbackingTime);


        protected virtual void Reset()
        {

            Recordable = true;
            Playbackable = true;
#if UNITY_EDITOR
            ValueDataID = GUID.Generate().ToString();
#endif

            Utilities.TryGetComponentInAllParents(this.transform, out RecordableObject recordableObject);

            if (recordableObject != null)
            {
                recordableObject._recordables.Add(this);
            }

            Utilities.TryGetComponentInAllParents(this.transform, out PlaybackableObject playbackableObject);

            if (playbackableObject != null)
            {
                playbackableObject.Playbackables.Add(this);
            }
        }
    }
}