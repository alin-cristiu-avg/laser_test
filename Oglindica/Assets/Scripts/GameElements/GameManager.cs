using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ScreensData.ScreenType IsInEditor;

    [SerializeField] private GameElementsData gameElementsData;
    [SerializeField] private LevelsData levelsData;
    [SerializeField] private MovementManager movementManager;

    private List<GameElement> _spawnedGameElements = new List<GameElement>();

    public void Awake()
    {
        Instance = this;
        IsInEditor = ScreensData.ScreenType.MainMenu;
        Application.targetFrameRate = 60;
    }

    public void CreateGameElement(GameElementsData.GameElementType type)
    {
        _spawnedGameElements.Add(Instantiate(gameElementsData.GetGameElement(type).gameElement, movementManager.transform));
    }

    public void CreateInitialElements()
    {
        if(_spawnedGameElements.Count > 0)
        {
            for(int i = 0;i < _spawnedGameElements.Count; i++)
            {
                Destroy(_spawnedGameElements[i].gameObject);
            }

            _spawnedGameElements.Clear();
        }

        List<GameElementPositioning> selectedLevelObjects = levelsData.GetSelectedLevelObjects();

        for (int i = 0; i < selectedLevelObjects.Count; i++)
        {
            CreateGameElement(selectedLevelObjects[i].type);
            _spawnedGameElements[_spawnedGameElements.Count - 1].transform.position = selectedLevelObjects[i].position;
            _spawnedGameElements[_spawnedGameElements.Count - 1].transform.eulerAngles = selectedLevelObjects[i].rotation;
        }

        GameElement.SetIsEditor?.Invoke(IsInEditor);
        StartCoroutine(UpdateLasers());
    }

    private IEnumerator UpdateLasers()
    {
        yield return new WaitForSeconds(0.1f);
        LaserGameElement.UpdateLasers?.Invoke();
    }

    public void SaveGameElements()
    {
        List<GameElementPositioning> currentPositions = new List<GameElementPositioning>();

        for(int i = 0;i < _spawnedGameElements.Count; i++)
        {
            currentPositions.Add(new GameElementPositioning() { type = _spawnedGameElements[i].GameElementType, position = _spawnedGameElements[i].transform.position, rotation = _spawnedGameElements[i].transform.eulerAngles });
        }

        levelsData.SaveSelectedLevelObjects(currentPositions);
    }
}
