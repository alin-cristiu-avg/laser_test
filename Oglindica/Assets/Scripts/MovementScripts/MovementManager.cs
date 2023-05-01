using System.Collections;
using System.Collections.Generic;
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
            _objectToMove = Raycaster.Instance.GetHitObject();
            if (_objectToMove != null)
            {
                _objectToMove.ManageCollider(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _objectToRotate = Raycaster.Instance.GetHitObject();
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
            }
            _objectToMove = null;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_objectToRotate != null)
            {
                _objectToRotate.ManageCollider(true);
            }
            _objectToRotate = null;
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
