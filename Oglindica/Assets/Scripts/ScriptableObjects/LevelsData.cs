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
}

[Serializable]
public class LevelData
{
    public string levelName;

    public void SetLevelName(string levelName)
    {
        this.levelName = levelName;
    }
}
