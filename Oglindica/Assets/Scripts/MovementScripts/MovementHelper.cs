using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementHelper : MonoBehaviour
{
    private BoxCollider _collider;
    private GameElement _gameElement;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _gameElement = GetComponent<GameElement>();
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetRotation(Vector3 pointToRotateTo)
    {
        Quaternion lookDir = Quaternion.LookRotation((transform.position - pointToRotateTo).normalized);

        transform.rotation = lookDir * Quaternion.Euler(new Vector3(0, 90, 0));
    }

    public void ManageCollider(bool active)
    {
        _collider.enabled = active;

        if (!active)
        {
            if (_gameElement.GameElementType == GameElementsData.GameElementType.Wall)
            {
                WallGameElement wallGameElement = _gameElement as WallGameElement;

                AdditionalInfoHelper.ActivateAdditionalInfo?.Invoke(
                    AdditionalInfoHelper.AdditionalInfoRequired.WallSizeInput,
                    new AdditionalData() { gameElement = _gameElement, values = null, defaultValue = wallGameElement.GetWallSize() },
                    wallGameElement.SetWallSize);
            }
            else if (_gameElement.GameElementType == GameElementsData.GameElementType.DoorSensor)
            {
                DoorSensorGameElement doorSensorGameElement = _gameElement as DoorSensorGameElement;

                AdditionalInfoHelper.ActivateAdditionalInfo?.Invoke(
                    AdditionalInfoHelper.AdditionalInfoRequired.SensorColors,
                    new AdditionalData() { gameElement = _gameElement, values = doorSensorGameElement.GetColorList(), defaultValue = doorSensorGameElement.GetCurrSelectedColor() },
                    doorSensorGameElement.SetColorTypeByIndex);
            }
            else
            {
                AdditionalInfoHelper.ActivateAdditionalInfo?.Invoke(AdditionalInfoHelper.AdditionalInfoRequired.None, null, null);
            }
        }
    }

    public void DeleteElement()
    {
        GameManager.Instance.DeleteGameElement(_gameElement);
    }
}
