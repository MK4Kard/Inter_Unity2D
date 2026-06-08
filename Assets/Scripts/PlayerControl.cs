using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance { get; private set; }

    [SerializeField] private float speed = 4f;

    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private Vector2 lookDirection;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed) {
            isRunning = true;
        } else {
            isRunning = false;
        }

        UpdateLookDirection();
    }

    private void UpdateLookDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPos = transform.position;
        lookDirection = (mousePos - playerPos).normalized;

        Debug.Log($"Игрок={playerPos}, Мышь={mousePos}, Направление={lookDirection}");
    }

    public bool IsRunning() {
        return isRunning;
    }

    public Vector3 GetLookDirection()
    {
        return lookDirection;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        return transform.position;
    }
}
