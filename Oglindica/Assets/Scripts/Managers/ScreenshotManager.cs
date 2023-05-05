using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    public static ScreenshotManager Instance;

    [SerializeField] private LevelsData levelsData;
    [SerializeField] private Camera rendCamera;

    private void Awake()
    {
        Instance = this;
    }

    public void TakeScreenshot()
    {
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        rendCamera.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        rendCamera.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        byte[] screenshotData = renderedTexture.EncodeToPNG();

        levelsData.SaveLevelPreview(screenshotData);
    }
}
