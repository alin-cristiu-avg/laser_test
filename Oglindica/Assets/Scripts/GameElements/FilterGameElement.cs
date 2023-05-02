using UnityEngine;

public class FilterGameElement : GameElement
{
    [Header("Filter Vars")]
    [SerializeField] private GameElementsData gameElementsData;

    [SerializeField] private GameElementsData.ColorType filterColor;
    [SerializeField] private Renderer filterRenderer;

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        filterRenderer.material.SetColor("_Color", gameElementsData.GetColor(filterColor).color);
    }
}
