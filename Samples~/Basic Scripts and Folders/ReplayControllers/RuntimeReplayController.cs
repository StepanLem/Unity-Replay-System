using System;
using OdinSerializer;
using System.IO;
using UnityEngine;
using StepanLem.ReplaySystem;

public class RuntimeReplayController : MonoBehaviour
{
    [Header("Recording")]
    [SerializeField] private RecordableObject _recordableRoot;
    private ObjectRecording _currentRecording;

    [Header("Playbacking")]
    [SerializeField] private PlaybackableObject _playbackableRoot;
    private RuntimeTimeline _currentPlaybackingTimeline;

    [Tooltip("Name of File in RecordsFolder_InPersistentData")]
    [FileNameSelector(nameof(RecordsFolder_InPersistentData))]
    public string Record_FileName;

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
            Debug.Log($"Запись была сохранена по пути:\n{path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка при сохранении записи: {e.Message}");
        }
    }


    private void LoadRecord(out RootRecordWithInfo record)
    {
        var path = Path.Combine(Application.persistentDataPath, RecordsFolder_InPersistentData, Record_FileName);
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