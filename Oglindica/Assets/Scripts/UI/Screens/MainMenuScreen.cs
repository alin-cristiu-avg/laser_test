using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : ScreenObject
{
    [SerializeField] private UIButton playButton;
    [SerializeField] private UIButton editButton;
    
    private LevelsHandler _levelsHandler;

    protected override void InitUI()
    {
        playButton.Text.text = "Play";
        playButton.Button.interactable = false;
        playButton.Button.onClick.RemoveAllListeners();
        playButton.Button.onClick.AddListener(GoToPlay);

        editButton.Text.text = "Edit";
        editButton.Button.interactable = false;
        editButton.Button.onClick.RemoveAllListeners();
        editButton.Button.onClick.AddListener(GoToEdit);
    }

    protected override void InitAdditionalData()
    {
        _levelsHandler = GetComponent<LevelsHandler>();
        InitLevels(true);
    }

    private void OnEnable()
    {
        if (_levelsHandler != null)
        {
            InitLevels(false);
        }
    }

    private void InitLevels(bool init)
    {
        _levelsHandler.InitLevels(init, UpdateMainMenuBtns);
    }

    private void UpdateMainMenuBtns()
    {
        if(_levelsHandler.GetSelectedLevel() == -1)
        {
            playButton.Button.interactable = false;
            editButton.Button.interactable = false;
        }
        else
        {
            playButton.Button.interactable = true;
            editButton.Button.interactable = true;
        }
    }

    private void GoToPlay()
    {
        UIManager.Instance.LoadScreen(ScreensData.ScreenType.PlayMenu);
        GameManager.Instance.IsInEditor = ScreensData.ScreenType.PlayMenu;
        GameElement.SetIsEditor?.Invoke(GameManager.Instance.IsInEditor);
    }

    private void GoToEdit()
    {
        UIManager.Instance.LoadScreen(ScreensData.ScreenType.Editor);
        GameManager.Instance.IsInEditor = ScreensData.ScreenType.Editor;
        GameElement.SetIsEditor?.Invoke(GameManager.Instance.IsInEditor);
    }

    
}
