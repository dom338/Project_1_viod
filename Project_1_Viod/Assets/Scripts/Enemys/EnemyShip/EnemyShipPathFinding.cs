using UnityEngine;

public class EnemyShipPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D RB;
    private Vector2 moveDir;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RB.MovePosition(RB.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }
}
