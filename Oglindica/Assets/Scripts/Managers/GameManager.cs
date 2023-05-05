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
        if (IsInEditor == ScreensData.ScreenType.Editor && type == GameElementsData.GameElementType.DoorSensor)
        {
            CreateDoor();

            levelsData.AddNewGoal(_spawnedGameElements[_spawnedGameElements.Count - 2] as DoorSensorGameElement, _spawnedGameElements[_spawnedGameElements.Count - 1] as DoorGameElement);
        }
        else if(IsInEditor == ScreensData.ScreenType.MainMenu || IsInEditor == ScreensData.ScreenType.PlayMenu)
        {
            if(type == GameElementsData.GameElementType.Door)
            {
                levelsData.AddNewGoal(_spawnedGameElements[_spawnedGameElements.Count - 2] as DoorSensorGameElement, _spawnedGameElements[_spawnedGameElements.Count - 1] as DoorGameElement);
            }
        }
    }

    private void CreateDoor()
    {
        _spawnedGameElements.Add(Instantiate(gameElementsData.GetGameElement(GameElementsData.GameElementType.Door).gameElement, movementManager.transform));
        _spawnedGameElements[_spawnedGameElements.Count - 1].transform.position += Vector3.right;
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

        levelsData.ClearGoals();

        List<GameElementPositioning> selectedLevelObjects = levelsData.GetSelectedLevelObjects();

        for (int i = 0; i < selectedLevelObjects.Count; i++)
        {
            CreateGameElement(selectedLevelObjects[i].type);
            _spawnedGameElements[_spawnedGameElements.Count - 1].transform.position = selectedLevelObjects[i].position;
            _spawnedGameElements[_spawnedGameElements.Count - 1].transform.eulerAngles = selectedLevelObjects[i].rotation;
            SetAdditionalDataToGameElement(_spawnedGameElements[_spawnedGameElements.Count - 1], selectedLevelObjects[i].additionalData);
        }

        GameElement.SetIsEditor?.Invoke(IsInEditor);
        StartCoroutine(UpdateLasers());
    }

    private Vector3 FormatVector3(Vector3 input)
    {
        float x = float.Parse(input.x.ToString("F2"));
        float y = float.Parse(input.y.ToString("F2"));
        float z = float.Parse(input.z.ToString("F2"));

        return new Vector3(x, y, z);
    }

    private void SetAdditionalDataToGameElement(GameElement gameElement, string additionalData)
    {
        if(additionalData == "")
        {
            return;
        }
        if (gameElement is WallGameElement)
        {
            WallGameElement wallGameElement = (WallGameElement)gameElement;
            wallGameElement.SetWallSize(int.Parse(additionalData));
        }
        else if (gameElement is DoorSensorGameElement)
        {
            DoorSensorGameElement doorSensorGameElement = (DoorSensorGameElement)gameElement;
            doorSensorGameElement.SetColorTypeByIndex(int.Parse(additionalData));
        }
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
            currentPositions.Add(new GameElementPositioning() { 
                type = _spawnedGameElements[i].GameElementType, 
                position = FormatVector3(_spawnedGameElements[i].transform.position), 
                rotation = FormatVector3(_spawnedGameElements[i].transform.eulerAngles), 
                additionalData = GetAdditionalDataForGameElement(_spawnedGameElements[i])
            });
        }

        levelsData.SaveSelectedLevelObjects(currentPositions);
    }

    public string GetAdditionalDataForGameElement(GameElement gameElement)
    {
        if(gameElement is WallGameElement)
        {
            WallGameElement wallGameElement = (WallGameElement)gameElement;
            return wallGameElement.GetWallSize().ToString();
        }
        else if (gameElement is DoorSensorGameElement)
        {
            DoorSensorGameElement doorSensorGameElement = (DoorSensorGameElement)gameElement;
            return doorSensorGameElement.GetCurrSelectedColor().ToString();
        }

        return "";
    }

    public void DeleteGameElement(GameElement gameElementToDelete)
    {
        if(gameElementToDelete.GameElementType == GameElementsData.GameElementType.DoorSensor || gameElementToDelete.GameElementType == GameElementsData.GameElementType.Door)
        {
            levelsData.RemoveGoal(gameElementToDelete);
        }

        for (int i = 0; i < _spawnedGameElements.Count; i++)
        {
            if (_spawnedGameElements[i] == gameElementToDelete)
            {
                Destroy(_spawnedGameElements[i].gameObject);
                _spawnedGameElements.RemoveAt(i);
                break;
            }
        }

        SaveGameElements();
    }

    public void CheckGoalsReached()
    {
        if (IsInEditor == ScreensData.ScreenType.PlayMenu)
        {
            if (levelsData.CheckGoalsReached())
            {
                UIManager.Instance.LoadScreen(ScreensData.ScreenType.WonMenu);
                IsInEditor = ScreensData.ScreenType.MainMenu;
                GameElement.SetIsEditor?.Invoke(IsInEditor);
            }
        }
    }
}
