using UnityEngine;

public abstract class ScreenObject : MonoBehaviour
{
    [SerializeField] private ScreensData.ScreenType screenType;

    private void Start()
    {
        InitUI();
        InitAdditionalData();
    }

    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    protected abstract void InitUI();
    protected abstract void InitAdditionalData();

    protected void GoToMainMenu()
    {
        UIManager.Instance.LoadScreen(ScreensData.ScreenType.MainMenu);
        GameManager.Instance.IsInEditor = ScreensData.ScreenType.MainMenu;
        GameElement.SetIsEditor?.Invoke(GameManager.Instance.IsInEditor);
    }
}
