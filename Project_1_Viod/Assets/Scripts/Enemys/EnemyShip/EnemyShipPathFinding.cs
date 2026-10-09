using UnityEngine;

public class EnemyShipPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 180f;

    private Rigidbody2D RB;
    private Animator animator;
    private Vector2 moveDir;
    private Vector2 targetDirection = Vector2.up;
    private float currentAngle = 90f;


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Calcola l'angolo verso cui la nave deve girarsi.
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);

        // Ricava la direzione di movimento dall'angolo attuale.
        Vector2 moveDir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

        RB.MovePosition(RB.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        UpdateAnimation();
    }

    public void MoveTo(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            targetDirection = direction.normalized;
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        // Arrotonda la direzione a uno degli 8 orientamenti.
        float snappedAngle = Mathf.Round(currentAngle / 45f) * 45f;

        Vector2 animationDirection = new Vector2(Mathf.Cos(snappedAngle * Mathf.Deg2Rad), Mathf.Sin(snappedAngle * Mathf.Deg2Rad)).normalized;

        animator.SetFloat("InputX", animationDirection.x);
        animator.SetFloat("InputY", animationDirection.y);

        animator.SetFloat("LastInputX", animationDirection.x);
        animator.SetFloat("LastInputY", animationDirection.y);

        animator.SetBool("IsWalking", true);

    }
}
