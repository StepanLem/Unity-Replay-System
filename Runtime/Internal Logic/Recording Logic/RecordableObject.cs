using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class RecordableObject : MonoBehaviour, IRecordable
    {
        public List<IRecordable> Recordables;

        [SerializeField, InterfaceType(typeof(IRecordable))]
        internal List<Object> _recordables;
        public void CacheRecordablesAsInterface() => Recordables = _recordables.OfType<IRecordable>().ToList();

        public IRecording CreateRecording()
        {
            CacheRecordablesAsInterface();

            var recording = new ObjectRecording(Recordables);
            return recording;
        }
    }
}
