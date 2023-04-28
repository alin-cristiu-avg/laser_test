using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuScreen : ScreenObject
{
    [SerializeField] private LevelsData levelsData;

    [SerializeField] private UIButton mainMenuButton;
    [SerializeField] private UILabel levelName;

    private int _selectedLevel;

    protected override void InitUI()
    {
        mainMenuButton.Text.text = "Main Menu";
        mainMenuButton.Button.onClick.RemoveAllListeners();
        mainMenuButton.Button.onClick.AddListener(GoToMainMenu);
    }

    protected override void InitAdditionalData()
    {
        levelName.Text.text = levelsData.levels[levelsData.GetSelectedLevel()].levelName;
    }

    private void OnEnable()
    {
        InitSelectedLevel();
    }

    private void InitSelectedLevel()
    {
        _selectedLevel = levelsData.GetSelectedLevel();

        if (_selectedLevel < 0)
        {
            levelName.Text.text = "";
        }
        else
        {
            levelName.Text.text = levelsData.levels[_selectedLevel].levelName;
        }
    }
}
