
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipPathFinding : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float waypointDistance = 0.2f;

    [Header("Navigation")]
    [SerializeField] private EnemyNavigationGrid navigationGrid;

    private Rigidbody2D RB;
    private Animator animator;

    private List<Vector2> path = new List<Vector2>();
    private int pathIndex;
    private float currentAngle;

    public bool HasPath => pathIndex < path.Count;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentAngle = transform.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        if (!HasPath)
        {
            UpdateAnimation(false);
            return;
        }

        // Salta i waypoint già raggiunti.
        while (HasPath)
        {
            Vector2 nextPoint = path[pathIndex];

            if (Vector2.Distance(RB.position, nextPoint) >
                waypointDistance)
            {
                break;
            }

            pathIndex++;
        }

        if (!HasPath)
        {
            UpdateAnimation(false);
            return;
        }

        Vector2 targetPoint = path[pathIndex];
        Vector2 toPoint = targetPoint - RB.position;

        if (toPoint.sqrMagnitude < 0.0001f)
            return;

        // Direzione verso il waypoint.
        Vector2 moveDirection = toPoint.normalized;

        // Aggiorna gradualmente l'orientamento dell'animazione.
        float targetAngle =
            Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        currentAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            rotationSpeed * Time.fixedDeltaTime
        );

        // Avanza direttamente verso il waypoint.
        Vector2 newPosition = Vector2.MoveTowards(
            RB.position,
            targetPoint,
            moveSpeed * Time.fixedDeltaTime
        );

        RB.MovePosition(newPosition);

        UpdateAnimation(true);
    }

    public void SetDestination(Vector2 destination)
    {
        if (navigationGrid == null)
        {
            Debug.LogError(
                "EnemyNavigationGrid non assegnata!",
                this
            );
            StopMoving();
            return;
        }

        path = navigationGrid.FindPath(RB.position, destination);
        pathIndex = 0;

        if (path.Count == 0)
        {
            Debug.LogWarning(
                "Nessun percorso trovato per la nave nemica.",
                this
            );
        }
    }

    // Mantiene il metodo usato dal tuo script originale.
    public void MoveTo(Vector2 targetPosition)
    {
        SetDestination(targetPosition);
    }

    public void StopMoving()
    {
        path.Clear();
        pathIndex = 0;
        UpdateAnimation(false);
    }

    private void UpdateAnimation(bool IsMoving)
    {
        if (animator == null)
            return;

        // Otto direzioni: una ogni 45 gradi.
        float snappedAngle =
            Mathf.Round(currentAngle / 45f) * 45f;

        Vector2 direction = new Vector2(
            Mathf.Cos(snappedAngle * Mathf.Deg2Rad),
            Mathf.Sin(snappedAngle * Mathf.Deg2Rad)
        ).normalized;

        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetFloat("LastInputX", direction.x);
        animator.SetFloat("LastInputY", direction.y);
        animator.SetBool("IsMoving", IsMoving);
    }
}
