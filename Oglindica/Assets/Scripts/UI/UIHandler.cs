using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private ScreensData screensData;
    [SerializeField] private Transform mainScreensContainer;
    [SerializeField] private Transform popupsContainer;

    private ScreensData.ScreenType _lastLoadedScreenType = ScreensData.ScreenType.None;
    private Dictionary<ScreensData.ScreenType, ScreenObject> loadedScreens = new Dictionary<ScreensData.ScreenType, ScreenObject>();

    public void LoadScreen(ScreensData.ScreenType screenType)
    {
        if (_lastLoadedScreenType != ScreensData.ScreenType.None)
        {
            loadedScreens[_lastLoadedScreenType].Hide();
        }

        if (!loadedScreens.ContainsKey(screenType))
        {
            loadedScreens.Add(screenType, Instantiate(screensData.GetScreenObjectByType(screenType), mainScreensContainer));
            loadedScreens[screenType].Init();
        }
        else
        {
            loadedScreens[screenType].Show();
        }

        _lastLoadedScreenType = screenType;
    }
}
