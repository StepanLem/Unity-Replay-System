using System;
using System.Collections.Generic;

namespace StepanLem.ReplaySystem
{
    public interface IRecording
    {
        public void Start();

        public void Finish();

        public Func<float> RecordingTimeGetter { get; set; }

        public void AddValueRecordsToDictionary(Dictionary<string, IValueRecord> valueRecordsByID);
    }
}


