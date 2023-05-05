using UnityEngine;

public class WallGameElement : GameElement
{
    [Header("Wall Vars")]
    [SerializeField] private Transform wallGeoMetry;
    [SerializeField] private BoxCollider boxCollider;

    public int GetWallSize()
    {
        return Mathf.RoundToInt(wallGeoMetry.localScale.y);
    }

    public void SetWallSize(int size)
    {
        wallGeoMetry.localScale = new Vector3(transform.localScale.x, size, transform.localScale.z);
        boxCollider.size = new Vector3(boxCollider.size.x, size, boxCollider.size.z);
    }
}
