using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_LEVELS_DIRECTORY = "{0}/SavedLevels";
    private const string SAVE_LEVELS_FILE_NAME_EXTENSION = ".txt";

    [SerializeField] private LevelsData levelsData;

    private void Start()
    {
        LoadFiles();
    }

    private void LoadFiles()
    {
        string path = GetPath();

        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();
        levelsData.levels = new List<LevelData>();
        LevelData levelData = new LevelData();
        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Substring(file.Name.Length - 5, 5) != LevelsData.META_EXTENSION)
            {
                string fileContent = File.ReadAllText(file.FullName);

                JsonUtility.FromJsonOverwrite(fileContent, levelData);
                levelsData.levels.Add(levelData);
            }
        }
    }

    private string GetPath()
    {
        string path = string.Format(SAVE_LEVELS_DIRECTORY, Application.dataPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

    private void OnDestroy()
    {
        SaveLevelsToFiles();
    }

    private void SaveLevelsToFiles()
    {
        string path = GetPath();
        string fileName = "";
        string levelJson = "";

        for (int i = 0; i < levelsData.levels.Count; i++)
        {
            fileName = $"/{levelsData.levels[i].levelName}{SAVE_LEVELS_FILE_NAME_EXTENSION}";
            levelJson = JsonUtility.ToJson(levelsData.levels[i]);
            Debug.LogError(levelJson);

            File.WriteAllText(path + fileName, levelJson);
        }
        
#if UNITY_EDITOR
        Debug.Log($"Write record at {path}");
        AssetDatabase.Refresh();
#endif
    }
}
