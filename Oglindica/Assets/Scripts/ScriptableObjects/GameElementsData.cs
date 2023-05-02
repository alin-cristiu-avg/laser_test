using System;
using System.Collections.Generic;
using UnityEngine;
using static GameElementsData;

[CreateAssetMenu(fileName = "GameElementsData", menuName = "Scriptable Objects/Game Elements", order = 2)]
public class GameElementsData : ScriptableObject
{
    public enum GameElementType
    {
        None,
        Laser,
        Mirror,
        FilterRed,
        FilterGreen,
        FilterBlue,
        Wall,
        Door,
        DoorSensor
    }

    public enum ColorType
    {
        Yellow,
        Orange,
        Red,
        Purple,
        Blue,
        Green
    }

    [SerializeField] private List<GameElementStructure> gameElements = new List<GameElementStructure>();
    [SerializeField] private List<ColorStructure> colors = new List<ColorStructure>();

    private Dictionary<GameElementType, GameElementStructure> _gameElementsDictionary = new Dictionary<GameElementType, GameElementStructure>();
    private Dictionary<ColorType, ColorStructure> _colorsDictionary = new Dictionary<ColorType, ColorStructure>();

    public int GetGameElementsCount()
    {
        return gameElements.Count;
    }

    public GameElementStructure GetGameElementByIndex(int index)
    {
        return gameElements[index];
    }

    public GameElementStructure GetGameElement(GameElementType elementType)
    {
        if(_gameElementsDictionary.Count == 0)
        {
            InitGameElementsDictionary();
        }

        return _gameElementsDictionary[elementType];
    }

    private void InitGameElementsDictionary()
    {
        for(int i =0; i < gameElements.Count; i++)
        {
            if (!_gameElementsDictionary.ContainsKey(gameElements[i].type))
            {
                _gameElementsDictionary.Add(gameElements[i].type, gameElements[i]);
            }
            else
            {
                Debug.LogError("This - " + gameElements[i].type + " - is a duplicate screen type!");
            }
        }
    }

    public int GetColorsCount()
    {
        return colors.Count;
    }

    public ColorStructure GetColor(ColorType colorType)
    {
        if (_colorsDictionary.Count == 0)
        {
            InitColorsDictionary();
        }

        return _colorsDictionary[colorType];
    }

    private void InitColorsDictionary()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            if (!_colorsDictionary.ContainsKey(colors[i].type))
            {
                _colorsDictionary.Add(colors[i].type, colors[i]);
            }
            else
            {
                Debug.LogError("This - " + colors[i].type + " - is a duplicate screen type!");
            }
        }
    }
}

[Serializable]
public struct GameElementStructure
{
    public GameElementType type;
    public Sprite preview;
    public GameElement gameElement;
}

[Serializable]
public struct ColorStructure
{
    public ColorType type;
    public Color color;
}
