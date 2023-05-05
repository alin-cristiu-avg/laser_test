using UnityEngine;

public class DoorGameElement : GameElement
{
    [Header("Door Vars")]
    [SerializeField] private Transform doorHinge;
    [SerializeField] private float openSpeed;

    private bool _openDoor = false;
    private BoxCollider _boxCollider;

    protected override void Start()
    {
        base.Start();
        doorHinge.localScale = Vector3.one;
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = true;
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
            _boxCollider.enabled = false;
        }
    }
}
