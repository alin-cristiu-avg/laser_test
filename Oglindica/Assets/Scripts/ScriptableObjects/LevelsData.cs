using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/Level Data", order = 1)]
public class LevelsData : ScriptableObject
{
    public const string META_EXTENSION = ".meta";
    private const string LEVELS_PREVIEW_DIRECTORY = "{0}/LevelsPreview";

    public List<LevelData> levels = new List<LevelData>();

    private int _selectedLevel;

    public void CreateNewLevel()
    {
        levels.Add(new LevelData());
        InitNewLevelData(levels[levels.Count - 1]);
    }

    private void InitNewLevelData(LevelData newLevel)
    {
        newLevel.levelName = $"Level {levels.Count - 1}";
    }

    public void UpdateLevelData()
    {

    }

    public void SetSelectedLevel(int level)
    {
        _selectedLevel = level;
    }

    public int GetSelectedLevel()
    {
        return _selectedLevel;
    }

    public List<GameElementPositioning> GetSelectedLevelObjects()
    {
        return levels[_selectedLevel].gameElementsData;
    }

    public void SaveSelectedLevelObjects(List<GameElementPositioning> objectsPositions)
    {
        levels[_selectedLevel].gameElementsData = objectsPositions;
    }

    public void SaveLevelPreview(byte[] screenshotData)
    {
        string path = string.Format(LEVELS_PREVIEW_DIRECTORY, Application.dataPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = $"/Preview_{levels[_selectedLevel].levelName}.jpg";
        levels[_selectedLevel].levelPreviewLocation = path + fileName;

        File.WriteAllBytes(path + fileName, screenshotData);

#if UNITY_EDITOR
        Debug.Log($"Write record at {path}");
        AssetDatabase.Refresh();
#endif

        CheckForUnusedFiles(path);
    }

    private void CheckForUnusedFiles(string previewDirectory)
    {
        DirectoryInfo info = new DirectoryInfo(previewDirectory);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if(file.Name.Substring(file.Name.Length-5,5) != META_EXTENSION)
            {
                bool isUsed = false;
                string previewFileName;
                string[] previewFileSegments;
                for(int i = 0;i < levels.Count; i++)
                {
                    previewFileSegments = levels[i].levelPreviewLocation.Split('/');
                    previewFileName = previewFileSegments[previewFileSegments.Length - 1];
                    if (previewFileName == file.Name)
                    {
                        isUsed = true;
                    }
                }

                if (!isUsed)
                {
                    Debug.LogError("Deleting unused file : " + file.FullName);
                    File.Delete(file.FullName);
                }
            }
        }
    }

    public void ClearGoals()
    {
        levels[_selectedLevel].gameGoals.Clear();
    }

    public void AddNewGoal(DoorSensorGameElement sensor, DoorGameElement door)
    {
        levels[_selectedLevel].gameGoals.Add(new GameGoals() { door = door, sensor = sensor, isCorrect = false });
    }

    public void RemoveGoal(GameElement gameElementToDelete)
    {
        for (int i = 0; i < levels[_selectedLevel].gameGoals.Count; i++)
        {
            if(gameElementToDelete is DoorSensorGameElement)
            {
                if (gameElementToDelete == levels[_selectedLevel].gameGoals[i].sensor)
                {
                    levels[_selectedLevel].gameGoals.RemoveAt(i);
                    break;
                }
            }
            else if(gameElementToDelete is DoorGameElement)
            {
                if (gameElementToDelete == levels[_selectedLevel].gameGoals[i].sensor)
                {
                    levels[_selectedLevel].gameGoals.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public bool CheckGoalsReached()
    {
        bool levelIsDone = true;
        for(int i = 0;i < levels[_selectedLevel].gameGoals.Count; i++)
        {
            if (!levels[_selectedLevel].gameGoals[i].CheckCorrect())
            {
                levelIsDone = false;
            }
        }

        return levelIsDone;
    }
}

[Serializable]
public class LevelData
{
    public string levelName;
    public string levelPreviewLocation;
    public List<GameElementPositioning> gameElementsData = new List<GameElementPositioning>();
    public List<GameGoals> gameGoals = new List<GameGoals>();

    public void SetLevelName(string levelName)
    {
        this.levelName = levelName;
    }
}

[Serializable]
public struct GameElementPositioning
{
    public GameElementsData.GameElementType type;
    public Vector3 position;
    public Vector3 rotation;
    public string additionalData;
}

[Serializable]
public struct GameGoals
{
    public DoorSensorGameElement sensor;
    public DoorGameElement door;
    public bool isCorrect;

    public bool CheckCorrect()
    {
        if (isCorrect == false && sensor.IsCorrectColor())
        {
            door.SetOpenDoor();
            isCorrect = true;

            return true;
        }

        return false;
    }
}
