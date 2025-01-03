namespace StepanLem.ReplaySystem.SampleScene
{
    public class Replayable_MyString_OfYourCustomComponent : BaseReplayableValue<string>
    {
        public YourCustomComponent component;

        public override void HanldeRecordingTick(ValueRecord<string> record, float recordingTime)
        {
            var value = component.MyString;

            if (record.KeyframesCount != 0)
            {
                var prewiousValue = record.Keyframes[^1].Value;

                if (prewiousValue == value)
                    return;
            }

            record.Keyframes.Add(new Keyframe<string>(value, recordingTime));
        }

        public override void HandleKeyframeActivation(ValueRecord<string> record, int keyframeIndex, float playbackingTime)
        {
            var value = record[keyframeIndex].Value;

            component.MyString = value;
        }

        protected override void Reset()
        {
            base.Reset();

            component = GetComponent<YourCustomComponent>();
        }
    }
}