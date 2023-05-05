using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private LevelsData levelsData;

    [SerializeField] private LevelElement levelPrefab;
    [SerializeField] private Transform levelContainer;

    private Dictionary<int, LevelElement> _levels = new Dictionary<int, LevelElement>();
    private int _selectedLevel;
    private Action m_UpdateMainMenuBtns;

    public void InitLevels(bool init, Action UpdateMainMenuBtns)
    {
        m_UpdateMainMenuBtns = UpdateMainMenuBtns;

        if (init)
        {
            _selectedLevel = -1;
            levelsData.SetSelectedLevel(_selectedLevel);
        }
        else
        {
            _selectedLevel = levelsData.GetSelectedLevel();
        }

        if (_levels.Count > 0)
        {
            foreach (KeyValuePair<int, LevelElement> level in _levels)
            {
                Destroy(level.Value.gameObject);
            }
            _levels.Clear();
        }

        for (int i = 0; i < levelsData.levels.Count; i++)
        {
            _levels.Add(i, Instantiate(levelPrefab, levelContainer));
            _levels[_levels.Count - 1].InitLevelElement(levelsData.levels[i]);
            SetLevelButtonDelegate(_levels[_levels.Count - 1].LevelButton, i);
            _levels[_levels.Count - 1].LevelButton.interactable = i != _selectedLevel;
        }

        _levels.Add(-1, Instantiate(levelPrefab, levelContainer));
        _levels[-1].InitLevelElement(null);
        _levels[-1].LevelButton.onClick.RemoveAllListeners();
        _levels[-1].LevelButton.onClick.AddListener(AddNewLevel);
    }

    public void UpdateLevelImages()
    {
        foreach (KeyValuePair<int, LevelElement> level in _levels)
        {
            level.Value.UpdateImages();
        }
    }

    public int GetSelectedLevel()
    {
        return _selectedLevel;
    }

    private void SetLevelButtonDelegate(Button levelButton, int index)
    {
        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(() => SelectLevel(index));
    }

    private void SelectLevel(int newLevelIndex)
    {
        if (_selectedLevel >= 0)
        {
            if (_levels[_selectedLevel].LevelButton.interactable == false)
            {
                _levels[_selectedLevel].LevelButton.interactable = true;
            }
        }

        _selectedLevel = newLevelIndex;
        levelsData.SetSelectedLevel(_selectedLevel);

        _levels[_selectedLevel].LevelButton.interactable = false;

        GameManager.Instance.CreateInitialElements();

        m_UpdateMainMenuBtns?.Invoke();
    }

    private void AddNewLevel()
    {
        levelsData.CreateNewLevel();

        int newLevelElementIndex = levelsData.levels.Count - 1;

        _levels.Add(newLevelElementIndex, Instantiate(levelPrefab, levelContainer));
        _levels[newLevelElementIndex].transform.SetSiblingIndex(_levels.Count - 2);
        _levels[newLevelElementIndex].InitLevelElement(levelsData.levels[newLevelElementIndex]);
        SetLevelButtonDelegate(_levels[newLevelElementIndex].LevelButton, newLevelElementIndex);
    }
}
