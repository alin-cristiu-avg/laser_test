using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public static Raycaster Instance;
    private float MAX_DEPTH = 100;

    [SerializeField] private Camera mainCam;
    [Space]
    [SerializeField] private Transform background;
    [SerializeField] private float depth = 10;

    private RaycastHit _hit;

    private void Awake()
    {
        Instance = this;
    }

    public MovementHelper GetHitObject()
    {
        MovementHelper hitObject = null;
        DoRacycast();
        if(_hit.transform != null)
        {
            hitObject = _hit.transform.GetComponent<MovementHelper>();
        }

        return hitObject;
        
    }

    public Vector3 GetRaycastPosition()
    {
        return GetRaycast();
    }

    private Vector3 GetRaycast()
    {
        if(background != null)
        {
            return DoBackgroundRaycast();
        }
        else
        {
            return DoDepthRaycast();
        }
    }

    private void DoRacycast()
    {
        Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out _hit, MAX_DEPTH);
    }

    private Vector3 DoBackgroundRaycast()
    {
        DoRacycast();
        if (_hit.transform != null)
        {
            return _hit.transform.position;
        }
        else
        {
            return DoDepthRaycast();
        }
    }

    private Vector3 DoDepthRaycast()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = depth;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        return mouseWorldPos;
    }
}
