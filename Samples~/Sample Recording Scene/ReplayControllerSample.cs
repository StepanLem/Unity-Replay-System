using System;
using OdinSerializer;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace StepanLem.ReplaySystem.SampleScene
{
    public class ReplayControllerSample : MonoBehaviour
    {
        [Header("Recording")]
        [SerializeField] private RecordableObject _recordableRoot;
        private ObjectRecording _currentRecording;

        [Header("Playbacking")]
        [SerializeField] private PlaybackableObject _playbackableRoot;
        private RuntimeTimeline _currentPlaybackingTimeline;

        [Header("Control")]
        [Tooltip("If False, will save record to SO")]
        public bool SaveInPersistentDataPath = false;

        [Tooltip("If False, will take record from SO")]
        public bool LoadFromPersistentDataPath = false;
        [Tooltip("Index in FileName in Records folder")]
        public int ChoosenReplayIndexInPersistantDataPath;
        public RecordContainerSO _recordContainerSO;


        [ContextMenu("Start Recording")]
        public void StartRecording()
        {
            _currentRecording = (ObjectRecording)_recordableRoot.CreateRecording();
            _currentRecording.StartAsRoot();
        }

        [ContextMenu("Finish Recording")]
        public void FinishRecording()
        {
            var recordWithInfo = _currentRecording.FinishAsRoot();

            SaveRecord(recordWithInfo);

            _currentRecording = null;
        }

        [ContextMenu("Start Playbacking")]
        public void StartPlaybacking()
        {
            LoadRecord(out RootRecordWithInfo record);

            _currentPlaybackingTimeline = new RuntimeTimeline();
            _currentPlaybackingTimeline.OnPlaybackCompleted += () => Debug.Log("Playback complete", this);

            _playbackableRoot.CreatePlaybackingTracksOnTimeline(_currentPlaybackingTimeline, record);

            _currentPlaybackingTimeline.PlayFromStart();
        }

        [ContextMenu("Break Playbacking")]
        public void BreakPlaybacking()
        {
            _currentPlaybackingTimeline.Break();
        }

        [ContextMenu("Pause Playbacking")]
        public void PausePlaybacking()
        {
            _currentPlaybackingTimeline.Pause();
        }

        [ContextMenu("Continue Playbacking")]
        public void ContinuePlaybacking()
        {
            _currentPlaybackingTimeline.Resume();
        }

        private void SaveRecord(RootRecordWithInfo record)
        {
            if (SaveInPersistentDataPath)
            {
                string folderPath = Path.Combine(Application.persistentDataPath, "Records");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string path;
                var fileIndex = 0;
                do
                {
                    path = Path.Combine(folderPath, $"Record_{fileIndex:000}.json");
                    fileIndex++;
                } while (File.Exists(path));

                try
                {
                    var serializedData = OdinSerializer.SerializationUtility.SerializeValue(record, DataFormat.JSON);
                    File.WriteAllBytes(path, serializedData);
                    Debug.Log($"Запись была сохранена по пути:\n{path}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка при сохранении записи: {e.Message}");
                }
            }
            else // Save data to ScriptableObject
            {
                var recordContainer = ScriptableObject.CreateInstance<RecordContainerSO>();
                recordContainer.Record = record;

                string folderPath = Path.Combine("Packages", "ReplaySystem", "Runtime", "Samples", "Records");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string path;
                var fileIndex = 0;
                do
                {
                    path = Path.Combine(folderPath, $"RootRecordWithInfo Container_{fileIndex:000}.asset");
                    fileIndex++;
                } while (File.Exists(path));

                AssetDatabase.CreateAsset(recordContainer, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log($"ScriptableObject с записью был сохранён по пути:\n{path}");
            }
        }


        private void LoadRecord(out RootRecordWithInfo record)
        {
            if (!LoadFromPersistentDataPath)
            {
                //Take data from ScriptableObject
                record = _recordContainerSO.Record;
                return;
            }


            var path = Path.Combine(Application.persistentDataPath, "Records", $"Record_{ChoosenReplayIndexInPersistantDataPath:000}.json");
            if (!File.Exists(path))
            {
                Debug.LogWarning("Файл отсуствует. Путь: " + path, this);
                record = null;
                return;
            }

            byte[] data = File.ReadAllBytes(path);
            record = OdinSerializer.SerializationUtility.DeserializeValue<RootRecordWithInfo>(data, DataFormat.JSON);
        }
    }
}