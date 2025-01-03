using StepanLem.ReplaySystem;
using UnityEngine;

public class Replayable_Position_OfTransform : BaseReplayableValue<Vector3>
{
    public Transform _transform;

    [SerializeField] private float MinChangeThreshold = 0.01f;

    public override void HanldeRecordingTick(ValueRecord<Vector3> record, float recordingTime)
    {
        var value = _transform.localPosition;

        if (record.KeyframesCount != 0)
        {
            var prewiousValue = record.Keyframes[^1].Value;

            if (Vector3.Distance(value, prewiousValue) < MinChangeThreshold)
                return;
        }

        record.Keyframes.Add(new Keyframe<Vector3>(value, recordingTime));
    }

    public override void HandleKeyframeActivation(ValueRecord<Vector3> record, int keyframeIndex, float playbackingTime)
    {
        var value = record[keyframeIndex].Value;

        _transform.localPosition = value;
    }

    protected override void Reset()
    {
        base.Reset();

        _transform = GetComponent<Transform>();
    }
}
