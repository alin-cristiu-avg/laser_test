using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameElementUI : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private UILabel levelName;

    [SerializeField] private Button gameElementButton;

    public Button GameElementButton => gameElementButton;

    private GameElementStructure _gameElementStructure;

    public void InitGameElement(GameElementStructure gameElementData)
    {
        _gameElementStructure = gameElementData;

        //levelImage.sprite = ;
        levelName.Text.text = _gameElementStructure.type.ToString();
    }

    public void SetButtonDelegate()
    {
        gameElementButton.onClick.RemoveAllListeners();
        gameElementButton.onClick.AddListener(() =>
        {
            GameManager.Instance.CreateGameElement(_gameElementStructure.type);
        });
    }
}
