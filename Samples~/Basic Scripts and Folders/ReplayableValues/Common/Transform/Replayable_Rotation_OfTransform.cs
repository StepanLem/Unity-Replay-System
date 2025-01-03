using StepanLem.ReplaySystem;
using UnityEngine;

public class Replayable_Rotation_OfTransform : BaseReplayableValue<Quaternion>
{
    public Transform _transform;

    [SerializeField] private float MinAngleChangeThreshold = 0.1f;

    public override void HanldeRecordingTick(ValueRecord<Quaternion> record, float recordingTime)
    {
        var value = _transform.localRotation;

        if (record.Keyframes.Count != 0)
        {
            var prewiousValue = record.Keyframes[^1].Value;

            if (Quaternion.Angle(value, prewiousValue) < MinAngleChangeThreshold)
                return;
        }

        record.Keyframes.Add(new Keyframe<Quaternion>(value, recordingTime));
    }

    public override void HandleKeyframeActivation(ValueRecord<Quaternion> record, int keyframeIndex, float playbackingTime)
    {
        var value = record[keyframeIndex].Value;

        _transform.localRotation = value;
    }

    protected override void Reset()
    {
        base.Reset();

        _transform = GetComponent<Transform>();
    }
}
