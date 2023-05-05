using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{
    [SerializeField] private GameElementsData gameElementsData;

    [SerializeField] private LineRenderer laser;

    private ColorStructure _currColor;

    public void SetPosition(int posIndex, Vector3 pos)
    {
        laser.SetPosition(posIndex, pos);
    }

    public void SetColor(GameElementsData.ColorType lastColor, GameElementsData.ColorType currColor)
    {
        _currColor = gameElementsData.GetCombinedColor(lastColor, currColor);

        laser.startColor = _currColor.color;
        laser.endColor = _currColor.color;
    }

    public ColorStructure GetCurrColor()
    {
        return _currColor;
    }
}
