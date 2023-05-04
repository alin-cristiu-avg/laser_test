using System.Collections.Generic;
using UnityEngine;

public class DoorSensorGameElement : GameElement
{
    [Header("Door Sensor Vars")]
    [SerializeField] private ColorStructure neededColor;

    public bool IsCorrectColor(Color rayColor)
    {
        return rayColor == neededColor.color;
    }
}
