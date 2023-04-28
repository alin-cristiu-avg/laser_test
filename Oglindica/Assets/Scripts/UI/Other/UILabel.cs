using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILabel : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public TMP_Text Text => text;
}
