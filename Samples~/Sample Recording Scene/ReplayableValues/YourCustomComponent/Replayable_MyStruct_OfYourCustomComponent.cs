namespace StepanLem.ReplaySystem.SampleScene
{
    public class Replayable_MyStruct_OfYourCustomComponent : BaseReplayableValue<SomeStruct>
    {
        public YourCustomComponent component;

        public override void HanldeRecordingTick(ValueRecord<SomeStruct> record, float recordingTime)
        {
            var value = component.MyStruct;

            if (record.KeyframesCount != 0)
            {
                var prewiousValue = record.Keyframes[^1].Value;

                if (prewiousValue.Property1 == value.Property1 && prewiousValue.Property2 == value.Property2)
                    return;
            }

            record.Keyframes.Add(new Keyframe<SomeStruct>(value, recordingTime));
        }

        public override void HandleKeyframeActivation(ValueRecord<SomeStruct> record, int keyframeIndex, float playbackingTime)
        {
            var value = record[keyframeIndex].Value;

            component.MyStruct = value;
        }

        protected override void Reset()
        {
            base.Reset();

            component = GetComponent<YourCustomComponent>();
        }
    }
}

