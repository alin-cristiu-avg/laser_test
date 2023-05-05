using System.Collections.Generic;
using UnityEngine;

public class DoorSensorGameElement : GameElement
{
    [Header("Door Sensor Vars")]
    [SerializeField] private GameElementsData.ColorType neededColor;
    [SerializeField] private Renderer sensorRenderer;

    private GameElementsData.ColorType _receivedColor = GameElementsData.ColorType.Other;

    protected override void Start()
    {
        base.Start();
        _receivedColor = GameElementsData.ColorType.Other;
        SetNeededColorToRenderer();
    }

    public List<string> GetColorList()
    {
        return gameElementsData.GetColorNamesList();
    }

    public int GetCurrSelectedColor()
    {
        return (int)neededColor;
    }

    public void SetColorTypeByIndex(int colorTypeIndex)
    {
        neededColor = (GameElementsData.ColorType)colorTypeIndex;
        SetNeededColorToRenderer();
    }

    private void SetNeededColorToRenderer()
    {
        sensorRenderer.material.SetColor("_Color", gameElementsData.GetColor(neededColor).color);
    }

    public void SetReceivedColor(GameElementsData.ColorType rayColor)
    {
        _receivedColor = rayColor;
    }

    public bool IsCorrectColor()
    {
        return _receivedColor == neededColor;
    }
}
