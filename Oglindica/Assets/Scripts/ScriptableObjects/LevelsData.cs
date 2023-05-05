using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/Level Data", order = 1)]
public class LevelsData : ScriptableObject
{
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
