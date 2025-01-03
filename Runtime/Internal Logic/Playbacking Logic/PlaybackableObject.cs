using System.Collections.Generic;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class PlaybackableObject : MonoBehaviour, IPlaybackable
    {
        [SerializeField, InterfaceType(typeof(IPlaybackable))]
        internal List<Object> Playbackables;

        public void CreatePlaybackingTracksOnTimeline(RuntimeTimeline timeline, RootRecordWithInfo rootRecord)
        {
            foreach (var playbackableObj in Playbackables)
            {
                var playbackable = playbackableObj as IPlaybackable;
                playbackable.CreatePlaybackingTracksOnTimeline(timeline, rootRecord);
            }
        }
    }
}
