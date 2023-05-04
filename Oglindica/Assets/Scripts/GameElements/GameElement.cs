using System;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public static Action<ScreensData.ScreenType> SetIsEditor = delegate { };

    public bool CanMove => canMove;
    public bool CanRotate => canRotate;
    public GameElementsData.GameElementType GameElementType => gameElementType;

    [SerializeField] protected GameElementsData gameElementsData;
    [SerializeField] public bool canMove;
    [SerializeField] public bool canRotate;
    [SerializeField] private GameElementsData.GameElementType gameElementType;
    [SerializeField] private MovementHelper movementHelper;

    protected bool isInEditorMode = false;

    protected void Awake()
    {
        SubscribeEvents();
    }

    protected virtual void Start()
    {
        OnSetIsEditor(GameManager.Instance.IsInEditor);
    }

    protected void OnDestroy()
    {
        UnsubscribeEvents();
    }

    protected virtual void SubscribeEvents()
    {
        SetIsEditor += OnSetIsEditor;
    }

    protected virtual void UnsubscribeEvents()
    {
        SetIsEditor -= OnSetIsEditor;
    }

    protected void OnSetIsEditor(ScreensData.ScreenType screen)
    {
        switch (gameElementType)
        {
            case GameElementsData.GameElementType.Laser:
            case GameElementsData.GameElementType.Wall:
            case GameElementsData.GameElementType.Door:
            case GameElementsData.GameElementType.DoorSensor:
                canMove = screen == ScreensData.ScreenType.Editor;
                canRotate = screen == ScreensData.ScreenType.Editor;
                break;

            default:
                canMove = screen == ScreensData.ScreenType.Editor || screen == ScreensData.ScreenType.PlayMenu;
                canRotate = screen == ScreensData.ScreenType.Editor || screen == ScreensData.ScreenType.PlayMenu;
                break;
        }
    }

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
