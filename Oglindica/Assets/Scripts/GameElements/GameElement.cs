using UnityEngine;

public class GameElement : MonoBehaviour
{
    public bool CanMove;
    public bool CanRotate;
    public GameElementsData.GameElementType GameElementType => gameElementType;

    [SerializeField] private GameElementsData.GameElementType gameElementType;
    [SerializeField] private MovementHelper movementHelper;

    public MovementHelper GetMovementHelperForMovement()
    {
        if (CanMove)
        {
            return movementHelper;
        }
        else
        {
            return null;
        }
    }

    public MovementHelper GetMovementHelperForRotation()
    {
        if (CanRotate)
        {
            return movementHelper;
        }
        else
        {
            return null;
        }
    }
}
