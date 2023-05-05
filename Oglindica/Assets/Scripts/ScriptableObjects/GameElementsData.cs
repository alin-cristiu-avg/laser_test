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
        FilterYellow,
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
        Green,
        White,
        Other
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

    public bool GameElementIsDoor(int index)
    {
        return gameElements[index].type == GameElementType.Door;
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

    public List<string> GetColorNamesList()
    {
        List<string> colors = new List<string>();

        for(int i = 0;i < this.colors.Count; i++)
        {
            colors.Add(this.colors[i].type.ToString());
        }

        return colors;
    }

    public ColorStructure GetColor(ColorType colorType)
    {
        if (_colorsDictionary.Count == 0)
        {
            InitColorsDictionary();
        }

        return _colorsDictionary[colorType];
    }

    public ColorStructure GetCombinedColor(ColorType lastColor, ColorType currColor)
    {
        if(lastColor == ColorType.White && currColor != ColorType.White)
        {
            return new ColorStructure() { type = currColor, color = GetColor(currColor).color };
        }
        else if (lastColor != ColorType.White && currColor == ColorType.White)
        {
            return new ColorStructure() { type = lastColor, color = GetColor(lastColor).color };
        }
        else if(lastColor == currColor)
        {
            return new ColorStructure() { type = currColor, color = GetColor(currColor).color };
        }
        else if(lastColor != ColorType.White && currColor != ColorType.White)
        {
            switch (lastColor)
            {
                case ColorType.Yellow:
                    switch (currColor)
                    {
                        case ColorType.Red:
                            return new ColorStructure() { type = ColorType.Orange, color = GetColor(ColorType.Orange).color };

                        case ColorType.Blue:
                            return new ColorStructure() { type = ColorType.Green, color = GetColor(ColorType.Green).color };
                    }
                    break;

                case ColorType.Red:
                    switch (currColor)
                    {
                        case ColorType.Yellow:
                            return new ColorStructure() { type = ColorType.Orange, color = GetColor(ColorType.Orange).color };

                        case ColorType.Blue:
                            return new ColorStructure() { type = ColorType.Purple, color = GetColor(ColorType.Purple).color };
                    }
                    break;

                case ColorType.Blue:
                    switch (currColor)
                    {
                        case ColorType.Yellow:
                            return new ColorStructure() { type = ColorType.Green, color = GetColor(ColorType.Green).color };

                        case ColorType.Red:
                            return new ColorStructure() { type = ColorType.Purple, color = GetColor(ColorType.Purple).color };
                    }
                    break;

            }
        }
        else
        {
            return new ColorStructure() { type = ColorType.Other, color = Color.Lerp(GetColor(lastColor).color, GetColor(currColor).color, 0.5f) };
        }

        return GetColor(ColorType.White);
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
