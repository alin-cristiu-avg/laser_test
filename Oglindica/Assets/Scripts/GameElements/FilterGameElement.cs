using UnityEngine;

public class FilterGameElement : GameElement
{
    [Header("Filter Vars")]
    [SerializeField] private GameElementsData.ColorType filterColor;
    [SerializeField] private Renderer filterRenderer;

    public GameElementsData.ColorType FilterColor => filterColor;

    protected override void Start()
    {
        base.Start();
        SetColor();
    }

    private void SetColor()
    {
        filterRenderer.material.SetColor("_Color", gameElementsData.GetColor(filterColor).color);
    }
}
