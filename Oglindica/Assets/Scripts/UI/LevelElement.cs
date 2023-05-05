using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelElement : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private UILabel levelName;
    [SerializeField] private GameObject levelDataObject;
    [SerializeField] private UILabel newLevelText;

    [SerializeField] private Button levelButton;

    public Button LevelButton => levelButton;

    private LevelData _levelData;

    public void InitLevelElement(LevelData levelData)
    {
        if(levelData == null)
        {
            levelDataObject.SetActive(false);
            newLevelText.gameObject.SetActive(true);
        }
        else
        {
            _levelData = levelData;
            levelDataObject.SetActive(true);
            levelImage.sprite = LoadPreview(levelData.levelPreviewLocation);
            levelName.Text.text = levelData.levelName;
            newLevelText.gameObject.SetActive(false);
        }
    }

    public void UpdateImages()
    {
        if (_levelData != null)
        {
            StartCoroutine(UpdateImagesDelayed());
        }
    }

    private IEnumerator UpdateImagesDelayed()
    {
        yield return new WaitForSeconds(1);
        LoadPreview(_levelData.levelPreviewLocation);
    }

    private Sprite LoadPreview(string path)
    {
        Texture2D loadedImage = new Texture2D(2,2);
        byte[] previewData;

        if (File.Exists(path))
        {
            previewData = File.ReadAllBytes(path);
            loadedImage.LoadImage(previewData);
            return Sprite.Create(loadedImage, new Rect(0, 0, loadedImage.width, loadedImage.height), new Vector2(0.5f, 0.5f));
        }

        return null;
    }
}
