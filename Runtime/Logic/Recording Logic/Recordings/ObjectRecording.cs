using System;
using System.Collections.Generic;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class ObjectRecording : IRecording
    {
        private readonly List<IRecording> Recordings = new();//тут другие objectRecording или valueRecording
        private readonly List<IRecordable> _replayables;

        private float _recordingStartTime;

        public Func<float> RecordingTimeGetter { get; set; }

        public ObjectRecording(List<IRecordable> replayableValues)
        {
            _replayables = replayableValues;
        }

        public void Start()
        {
            foreach (var replayable in _replayables)
            {
                var recording = replayable.CreateRecording();
                if (recording == null) continue;

                recording.RecordingTimeGetter = this.RecordingTimeGetter;
                Recordings.Add(recording);

                recording.Start();
            }
        }

        public void StartAsRoot()
        {
            _recordingStartTime = Time.realtimeSinceStartup;
            RecordingTimeGetter = () => Time.realtimeSinceStartup - _recordingStartTime;

            Start();
        }

        public void Finish()
        {
            foreach (var recording in Recordings)
            {
                recording.Finish();
            }
        }

        public RootRecordWithInfo FinishAsRoot()
        {
            Dictionary<string, IValueRecord> valueRecordsByID = new();
            foreach (var recording in Recordings)
            {
                recording.Finish();
                recording.AddValueRecordsToDictionary(valueRecordsByID);
            }

            return new RootRecordWithInfo(valueRecordsByID, RecordingTimeGetter());
        }

        public void AddValueRecordsToDictionary(Dictionary<string, IValueRecord> valueRecordsByID)
        {
            foreach (var recording in Recordings)
            {
                recording.AddValueRecordsToDictionary(valueRecordsByID);
            }
        }
    }
}
