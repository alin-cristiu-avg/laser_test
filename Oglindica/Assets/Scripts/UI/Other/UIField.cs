using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIField : MonoBehaviour
{
    [SerializeField] private TMP_InputField field;

    public TMP_InputField Field => field;
}
