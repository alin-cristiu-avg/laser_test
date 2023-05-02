using System;
using UnityEngine;

public class MovementHelper : MonoBehaviour
{
    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
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
    }
}
