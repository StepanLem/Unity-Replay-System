namespace StepanLem.ReplaySystem
{
    public interface IPlaybackable
    {
        public void CreatePlaybackingTracksOnTimeline(RuntimeTimeline timeline, RootRecordWithInfo rootRecord);
    }
}