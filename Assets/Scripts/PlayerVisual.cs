using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private const string IS_RUNNING = "IsRunning";
    private const string FACE_X = "FaceX";
    private const string FACE_Y = "FaceY";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, PlayerControl.Instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector2 lookDirection = PlayerControl.Instance.GetLookDirection();

        if (lookDirection.magnitude < 0.01f)
        {
            lookDirection = Vector2.right;
        }

        Debug.Log($"=== ОТЛАДКА ===");
        Debug.Log($"Направление: X={lookDirection.x:F3}, Y={lookDirection.y:F3}");
        Debug.Log($"Должно быть: мышь слева -> угол ~180°, мышь справа -> угол ~0°");

        animator.SetFloat(FACE_X, lookDirection.x);
        animator.SetFloat(FACE_Y, lookDirection.y);
    }
}
