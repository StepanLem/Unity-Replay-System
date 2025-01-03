using OdinSerializer;
using System;

namespace StepanLem.ReplaySystem
{
    public class RecordContainerSO : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize]
        public RootRecordWithInfo Record;
    }
}
