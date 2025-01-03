using OdinSerializer;
using System;
using System.Collections.Generic;

namespace StepanLem.ReplaySystem
{
    [Serializable]
    public class RootRecordWithInfo
    {
        [NonSerialized, OdinSerialize]
        private Dictionary<string, IValueRecord> _valueRecordsByID = new();

        public float Duration;

        public RootRecordWithInfo(Dictionary<string, IValueRecord> valueRecordsByID, float duration)
        {
            _valueRecordsByID = valueRecordsByID;
            Duration = duration;
        }

        public void AddValueRecordByID(IValueRecord record, string ID)
        {
            _valueRecordsByID.Add(ID, record);
        }

        public IValueRecord GetValueRecordByID(string ID)
        {
            if (!_valueRecordsByID.TryGetValue(ID, out IValueRecord record))
                return null;

            return record;
        }
    }
}