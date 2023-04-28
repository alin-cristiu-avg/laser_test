using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScreen : ScreenObject
{
    [SerializeField] private LevelsData levelsData;

    [SerializeField] private UIButton mainMenuButton;
    [SerializeField] private UIField levelNameField;

    private int _selectedLevel;

    protected override void InitUI()
    {
        mainMenuButton.Text.text = "Main Menu";
        mainMenuButton.Button.onClick.RemoveAllListeners();
        mainMenuButton.Button.onClick.AddListener(GoToMainMenu);

        levelNameField.Field.onEndEdit.RemoveAllListeners();
        levelNameField.Field.onEndEdit.AddListener(SetLevelName);
    }

    protected override void InitAdditionalData() { }

    private void OnEnable()
    {
        InitSelectedLevel();
    }

    private void InitSelectedLevel()
    {
        _selectedLevel = levelsData.GetSelectedLevel();

        if (_selectedLevel < 0)
        {
            levelNameField.Field.SetTextWithoutNotify("");
        }
        else
        {
            levelNameField.Field.SetTextWithoutNotify(levelsData.levels[_selectedLevel].levelName);
        }
    }

    private void SetLevelName(string value)
    {
        levelsData.levels[_selectedLevel].SetLevelName(value);
    }
}
