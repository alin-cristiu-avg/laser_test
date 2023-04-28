using System.Collections;
using System.Collections.Generic;
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

    public void InitLevelElement(LevelData levelData)
    {
        if(levelData == null)
        {
            levelDataObject.SetActive(false);
            newLevelText.gameObject.SetActive(true);
        }
        else
        {
            levelDataObject.SetActive(true);
            //levelImage.sprite = ;
            levelName.Text.text = levelData.levelName;
            newLevelText.gameObject.SetActive(false);
        }
    }
}
