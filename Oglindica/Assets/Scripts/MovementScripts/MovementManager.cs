using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private MovementHelper _objectToMove;
    private MovementHelper _objectToRotate;

    private void Update()
    {
        HandleObjectSelection();
        MovementObjects();
        RotateObjects();
    }

    private void HandleObjectSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _objectToMove = Raycaster.Instance.GetHitObject(true);
            if (_objectToMove != null)
            {
                _objectToMove.ManageCollider(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _objectToRotate = Raycaster.Instance.GetHitObject(false);
            if (_objectToMove != null)
            {
                _objectToMove.ManageCollider(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(_objectToMove != null)
            {
                _objectToMove.ManageCollider(true);
                if (GameManager.Instance.IsInEditor == ScreensData.ScreenType.Editor)
                {
                    GameManager.Instance.SaveGameElements();
                }
            }
            _objectToMove = null;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_objectToRotate != null)
            {
                _objectToRotate.ManageCollider(true);
                if (GameManager.Instance.IsInEditor == ScreensData.ScreenType.Editor)
                {
                    GameManager.Instance.SaveGameElements();
                }
            }
            _objectToRotate = null;
        }

        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            LaserGameElement.UpdateLasers?.Invoke();
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
            {
                _objectToMove.DeleteElement();
            }
        }
    }

    private void MovementObjects()
    {
        if(_objectToMove == null)
        {
            return;
        }

        _objectToMove.SetPosition(Raycaster.Instance.GetRaycastPosition());
    }

    private void RotateObjects()
    {
        if(_objectToRotate == null)
        {
            return;
        }

        _objectToRotate.SetRotation(Raycaster.Instance.GetRaycastPosition());
    }
}
