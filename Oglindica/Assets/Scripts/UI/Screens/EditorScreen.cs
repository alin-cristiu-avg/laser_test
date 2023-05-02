using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditorScreen : ScreenObject
{
    [SerializeField] private LevelsData levelsData;
    [SerializeField] private GameElementsData gameElementsData;
    [SerializeField] private GameElementUI gameElementPrefab;
    [SerializeField] private Transform gameElementContainer;

    [SerializeField] private UIButton mainMenuButton;
    [SerializeField] private UIField levelNameField;

    private int _selectedLevel;

    private List<GameElementUI> _spawnedGameElements = new List<GameElementUI>();

    protected override void InitUI()
    {
        mainMenuButton.Text.text = "Main Menu";
        mainMenuButton.Button.onClick.RemoveAllListeners();
        mainMenuButton.Button.onClick.AddListener(GoToMainMenu);

        levelNameField.Field.onEndEdit.RemoveAllListeners();
        levelNameField.Field.onEndEdit.AddListener(SetLevelName);
    }

    protected override void InitAdditionalData()
    {
        GenerateGameElements();
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

    private void GenerateGameElements()
    {
        for (int i = 0; i < gameElementsData.GetGameElementsCount(); i++)
        {
            _spawnedGameElements.Add(Instantiate(gameElementPrefab, gameElementContainer));
            _spawnedGameElements[_spawnedGameElements.Count - 1].InitGameElement(gameElementsData.GetGameElementByIndex(i));
            _spawnedGameElements[_spawnedGameElements.Count - 1].SetButtonDelegate();
        }
    }
}
