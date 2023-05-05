using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIDropDown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public void SetOptions(List<string> optionsStringList, int defaultValue = 0)
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        for(int i = 0;i < optionsStringList.Count; i++)
        {
            options.Add(new TMP_Dropdown.OptionData() { text = optionsStringList[i] });
        }
        dropdown.options = options;

        dropdown.value = defaultValue;
    }

    public void SetOnValueChanged(UnityAction<int> OnValueChanged)
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
}
