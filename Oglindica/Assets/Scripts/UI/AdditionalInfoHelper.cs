using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalInfoHelper : MonoBehaviour
{
    public static Action<AdditionalInfoRequired, AdditionalData, Action<int>> ActivateAdditionalInfo = delegate { };

    [SerializeField] private UIField wallField;
    [SerializeField] private UIDropDown sensorColorsDropdown;

    public enum AdditionalInfoRequired
    {
        None,
        WallSizeInput,
        SensorColors
    }

    private void Awake()
    {
        ActivateAdditionalInfo += OnActivateAdditionalInfo;
    }

    private void OnDestroy()
    {
        ActivateAdditionalInfo -= OnActivateAdditionalInfo;
    }

    private void OnActivateAdditionalInfo(AdditionalInfoRequired additionalInfoRequired, AdditionalData data, Action<int> callback)
    {
        switch (additionalInfoRequired)
        {
            case AdditionalInfoRequired.None:
                wallField.gameObject.SetActive(false);
                sensorColorsDropdown.gameObject.SetActive(false);
                break;

            case AdditionalInfoRequired.WallSizeInput:
                wallField.gameObject.SetActive(true);
                sensorColorsDropdown.gameObject.SetActive(false);

                wallField.Field.SetTextWithoutNotify(data.defaultValue.ToString());
                wallField.Field.onEndEdit.RemoveAllListeners();
                wallField.Field.onEndEdit.AddListener((value) =>
                {
                    int.TryParse(value, out int intValue);
                    callback?.Invoke(intValue);
                });
                break;

            case AdditionalInfoRequired.SensorColors:
                sensorColorsDropdown.gameObject.SetActive(true);
                wallField.gameObject.SetActive(false);

                sensorColorsDropdown.SetOptions(data.values, data.defaultValue);
                sensorColorsDropdown.SetOnValueChanged((intValue) =>
                {
                    callback?.Invoke(intValue);
                });
                break;
        }
    }
}

public class AdditionalData
{
    public GameElement gameElement;
    public List<string> values = new List<string>();
    public int defaultValue;
}
