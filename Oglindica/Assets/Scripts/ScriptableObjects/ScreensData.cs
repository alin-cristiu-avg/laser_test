using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreenData", menuName = "Scriptable Objects/Screen Data", order = 0)]
public class ScreensData : ScriptableObject
{
    public enum ScreenType
    {
        None,
        MainMenu,
        Editor,
        PlayMenu,
        WonMenu,
        SaveImage
    }

    public List<Screens> screens = new List<Screens>();
    public Dictionary<ScreenType, Screens> _screensDictionary = new Dictionary<ScreenType, Screens>();

    public ScreenObject GetScreenObjectByType(ScreenType screenType)
    {
        if(_screensDictionary.Count == 0)
        {
            InitDictionary();
        }

        return _screensDictionary[screenType].screen;
    }

    private void InitDictionary()
    {
        for(int i = 0;i < screens.Count; i++)
        {
            if (!_screensDictionary.ContainsKey(screens[i].screenType))
            {
                _screensDictionary.Add(screens[i].screenType, screens[i]);
            }
            else
            {
                Debug.LogError("This - " + screens[i].screenType + " - is a duplicate screen type!");
            }
        }
    }
}

[Serializable]
public struct Screens
{
    public ScreensData.ScreenType screenType;
    public ScreenObject screen; 
}
