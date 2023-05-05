using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonMenuScreen : ScreenObject
{
    [SerializeField] private UIButton mainMenuButton;

    protected override void InitUI()
    {
        mainMenuButton.Text.text = "Main Menu";
        mainMenuButton.Button.onClick.RemoveAllListeners();
        mainMenuButton.Button.onClick.AddListener(GoToMainMenu);
    }

    protected override void InitAdditionalData() { }
}
