using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private ScreensData.ScreenType startScreen;

    private UIHandler _uiHandler;

    public void Awake()
    {
        Instance = this;
        _uiHandler = GetComponent<UIHandler>();
    }

    private void Start()
    {
        LoadScreen(startScreen);
    }

    public void LoadScreen(ScreensData.ScreenType screenType)
    {
        _uiHandler.LoadScreen(screenType);
    }
}
