using System;
using OdinSerializer;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StepanLem.ReplaySystem.SampleScene
{
    public class ReplayControllerSample : MonoBehaviour
    {
        [Header("Recording")]
        [SerializeField] private RecordableObject _recordableRoot;
        private ObjectRecording _currentRecording;

        [Tooltip("If False, will save record to SO")]
        public bool SaveInPersistentDataPath = false;


        [Header("Playbacking")]
        [SerializeField] private PlaybackableObject _playbackableRoot;
        private RuntimeTimeline _currentPlaybackingTimeline;


        [Tooltip("If False, will take record from SO")]
        public bool LoadFromPersistentDataPath = false;

        [ShowIf(nameof(LoadFromPersistentDataPath), false)]
        public RecordContainerSO _recordContainerSO;

        [ShowIf(nameof(LoadFromPersistentDataPath), true)]
        [Tooltip("Name of Record-File in RecordsFolder_InPersistentData")]
        [FileNameSelector(nameof(RecordsFolder_InPersistentData))]
        public string Record_FileName;


        [Header("Folder Path")]
        [Tooltip("Folder in Assets where to save ScriptableObjects")]
        [ProjectFolderSelector]
        public string RecordsFolder_InProject;

        [Tooltip("Folder in PersistantDataPath where to save JSON files")]
        [PersistentPathSelector]
        public string RecordsFolder_InPersistentData;


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

            if (record == null)
            {
                Debug.LogWarning("Failed to start playback.", this);
                return;
            }

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
                string folderPath = Path.Combine(Application.persistentDataPath, RecordsFolder_InPersistentData);
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
                    Debug.Log($"Record was saved at the path::\n{path}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while saving the record: {e.Message}");
                }
            }
            else // Save data to ScriptableObject
            {
#if UNITY_EDITOR
                var recordContainer = ScriptableObject.CreateInstance<RecordContainerSO>();
                recordContainer.Record = record;

                string folderPath = RecordsFolder_InProject;
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

                Debug.Log($"ScriptableObject with the record was saved at the path:\n{path}");

#else
    Debug.LogError("This code cannot work in Build because it saves data in a ScriptableObject. " +
                    "Make sure that the SaveInPersistentDataPath checkmark is true, when you call this method in the Build.");
#endif

            }
        }


        private void LoadRecord(out RootRecordWithInfo record)
        {
            if (!LoadFromPersistentDataPath)
            {//Take data from ScriptableObject

                if (_recordContainerSO == null)
                {
                    Debug.LogError("RecordContainerSO == null. Please specify RecordContainerSO first");
                    record = null;
                    return;
                }

                record = _recordContainerSO.Record;
                return;
            }


            var path = Path.Combine(Application.persistentDataPath, RecordsFolder_InPersistentData, Record_FileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning("The file is missing. Path: " + path, this);
                record = null;
                return;
            }

            byte[] data = File.ReadAllBytes(path);
            record = OdinSerializer.SerializationUtility.DeserializeValue<RootRecordWithInfo>(data, DataFormat.JSON);
        }
    }
}