using System;
using System.Collections.Generic;

namespace StepanLem.ReplaySystem
{
    public class ValueRecording<T> : IValueRecording
    {
        public ValueRecord<T> Record;
        public MonoTicker Ticker { get; set; }

        private Action<ValueRecord<T>, float> RecordingTickHandler;

        public string ValueID;

        public Func<float> RecordingTimeGetter { get; set; }

        public ValueRecording(MonoTicker ticker, Action<ValueRecord<T>, float> recordingTickHandler, string valueID)
        {
            Ticker = ticker;
            RecordingTickHandler = recordingTickHandler;
            ValueID = valueID;
        }
        public void Start()
        {
            Record = new ValueRecord<T>();
            Ticker.OnTick += HandleRecordingTick;
        }

        public void HandleRecordingTick()
        {
            RecordingTickHandler(Record, RecordingTimeGetter());
        }

        public void AddValueRecordsToDictionary(Dictionary<string, IValueRecord> valueRecordsByID)
        {
            valueRecordsByID.Add(ValueID, Record);
        }

        public void Finish()
        {
            Ticker.OnTick -= HandleRecordingTick;
        }
    }

    public interface IValueRecording : IRecording
    {
        MonoTicker Ticker { get; set; }
    }
}
