using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Button Button => buttonComponent;
    public TMP_Text Text => buttonText;

    [SerializeField] private Button buttonComponent;
    [SerializeField] private TMP_Text buttonText;
}
