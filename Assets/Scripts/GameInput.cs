using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerControls playerInputActions;
    private Vector2 mouseScreenPosition;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerControls();
        playerInputActions.Enable();

        playerInputActions.Gameplay.Mouse.performed += OnMouseMove;
    }

    private void OnDestroy()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Gameplay.Mouse.performed -= OnMouseMove;
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Gameplay.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector2 screenPos = mouseScreenPosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        worldPos.z = 0;

        Debug.Log($"Мышь: экран={mouseScreenPosition}, мир={worldPos}");

        return worldPos;
    }
}
