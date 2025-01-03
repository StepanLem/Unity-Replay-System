using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public class RuntimeTimeline
    {
        public event Action<float> OnTimelineTicked;
        public event Action OnPlaybackCompleted;

        private List<ITimelineTrack> _tracks = new();
        private List<TrackKeyframe> _allKeyframes;

        private Coroutine _playingRoutine;
        private float timeSinceReplayStart;

        private bool _isPaused;

        public void CreateTrack<T>(ValueRecord<T> record, Action<ValueRecord<T>, int, float> keyframeActivationHandler)
        {
            var track = new ValueTrack<T>(record, keyframeActivationHandler);
            _tracks.Add(track);
        }

        private void MergeKeyframes()
        {
            _allKeyframes = new();

            foreach (var track in _tracks)
            {
                for (int i = 0; i < track.KeyframesCount; i++)
                {
                    _allKeyframes.Add(new TrackKeyframe(track.GetKeyframeTime(i), track, i));
                }
            }

            _allKeyframes.Sort((a, b) => a.Time.CompareTo(b.Time));

        }

        private void MergeKeyframesOptimized()
        {
            //since each track is already sorted, it is possible to connect them in a more optimized way.
        }

        public void PlayFromStart()
        {
            MergeKeyframes();

            timeSinceReplayStart = 0;
            _isPaused = false;

            _playingRoutine = CoroutineProcessor.StartRoutine(Play(0));
        }

        private IEnumerator Play(int currentKeyframeIndex)
        {
            if (_allKeyframes == null || _allKeyframes.Count == 0)
            {
                OnPlaybackCompleted?.Invoke();
                yield break;
            }

            if (currentKeyframeIndex < 0 || currentKeyframeIndex >= _allKeyframes.Count)
            {
                Debug.LogError("Выбранный индекс за пределами листа.");
            }

            while (true)
            {
                while (_isPaused)
                {
                    yield return null;
                }

                while (true)
                {
                    var keyframe = _allKeyframes[currentKeyframeIndex];

                    // If the keyframe time is greater than the current time, wait for the next frame.
                    if (keyframe.Time > timeSinceReplayStart)
                        break;

                    keyframe.Track.HandleKeyframeActivation(keyframe.IndexInTrack, timeSinceReplayStart);
                    currentKeyframeIndex++;

                    if (currentKeyframeIndex >= _allKeyframes.Count)
                    {
                        OnPlaybackCompleted?.Invoke();
                        yield break;
                    }
                }

                OnTimelineTicked?.Invoke(timeSinceReplayStart);

                yield return null;

                timeSinceReplayStart += Time.deltaTime;
            }
        }

        public void Break()
        {
            CoroutineProcessor.StopRoutine(_playingRoutine);
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }


    internal struct TrackKeyframe
    {
        public float Time;
        public ITimelineTrack Track;
        public int IndexInTrack;

        public TrackKeyframe(float time, ITimelineTrack track, int indexInTrack)
        {
            Time = time;
            Track = track;
            IndexInTrack = indexInTrack;
        }
    }
}