using UnityEngine;

public class DoorGameElement : GameElement
{
    [SerializeField] private Transform doorHinge;
    [SerializeField] private float openSpeed;

    private bool _openDoor = false;

    private void Start()
    {
        doorHinge.localScale = Vector3.one;
    }

    public void SetOpenDoor()
    {
        _openDoor = true;
    }

    public void Update()
    {
        if (_openDoor)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        doorHinge.localScale = new Vector3(doorHinge.localScale.x, Mathf.MoveTowards(doorHinge.localScale.y, 0, openSpeed * Time.deltaTime), doorHinge.localScale.z);

        if(doorHinge.localScale.y <= 0.1f)
        {
            _openDoor = false;
            Debug.Log("WON");
        }
    }
}
