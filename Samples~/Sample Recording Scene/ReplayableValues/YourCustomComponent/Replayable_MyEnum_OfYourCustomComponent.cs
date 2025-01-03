namespace StepanLem.ReplaySystem.SampleScene
{
    public class Replayable_MyEnum_OfYourCustomComponent : BaseReplayableValue<EnumOfSomething>
    {
        public YourCustomComponent component;

        public override void HanldeRecordingTick(ValueRecord<EnumOfSomething> record, float recordingTime)
        {
            var value = component.MyEnum;

            if (record.KeyframesCount != 0)
            {
                var prewiousValue = record.Keyframes[^1].Value;

                if (prewiousValue == value)
                    return;
            }

            record.Keyframes.Add(new Keyframe<EnumOfSomething>(value, recordingTime));
        }

        public override void HandleKeyframeActivation(ValueRecord<EnumOfSomething> record, int keyframeIndex, float playbackingTime)
        {
            var value = record[keyframeIndex].Value;

            component.MyEnum = value;
        }

        protected override void Reset()
        {
            base.Reset();

            component = GetComponent<YourCustomComponent>();
        }
    }
}



