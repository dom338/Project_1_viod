

using System.Collections;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chasing
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyShipPathFinding pathFinding;
    [SerializeField] private EnemyNavigationGrid navigationGrid;
    [SerializeField] private EnemyShipShooting shooting;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float loseTargetRange = 14f;
    [SerializeField] private float attackRange = 7f;

    [Header("Movement")]
    [SerializeField] private float roamingWaitTime = 1f;
    [SerializeField] private float minimumDestinationDistance = 4f;
    [SerializeField] private float repathInterval = 0.75f;

    private State state;
    private Coroutine aiCoroutine;
    private float repathTimer;

    private void Awake()
    {
        if (pathFinding == null)
            pathFinding = GetComponent<EnemyShipPathFinding>();

        if (shooting == null)
            shooting = GetComponent<EnemyShipShooting>();

        state = State.Roaming;
    }

    private void Start()
    {
        if (pathFinding == null || navigationGrid == null)
        {
            Debug.LogError(
                "Assegna PathFinding e NavigationGrid nell'Inspector!",
                this);
            enabled = false;
            return;
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (player == null)
        {
            Debug.LogError(
                "Giocatore non trovato! Assegna il Player o controlla il tag Player.",
                this);
            enabled = false;
            return;
        }

        aiCoroutine = StartCoroutine(RoamingRoutine());
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position, player.position);

        if (state == State.Roaming && distance <= detectionRange)
        {
            state = State.Chasing;
            repathTimer = 0f;

            if (aiCoroutine != null)
            {
                StopCoroutine(aiCoroutine);
                aiCoroutine = null;
            }
        }
        else if (state == State.Chasing && distance > loseTargetRange)
        {
            state = State.Roaming;

            if (shooting != null)
                shooting.StopShooting();

            aiCoroutine = StartCoroutine(RoamingRoutine());
            return;
        }

        if (state == State.Chasing)
        {
            // La nave continua a inseguire anche mentre spara.
            repathTimer -= Time.deltaTime;

            if (repathTimer <= 0f)
            {
                Vector2 directionFromPlayer =
                    (Vector2)transform.position -
                    (Vector2)player.position;

                if (directionFromPlayer.sqrMagnitude < 0.01f)
                    directionFromPlayer = Vector2.right;

                directionFromPlayer.Normalize();

                // Cerca di mantenere una certa distanza dal giocatore.
                float desiredDistance = attackRange * 0.65f;

                Vector2 destination =
                    (Vector2)player.position +
                    directionFromPlayer * desiredDistance;

                pathFinding.SetDestination(destination);
                repathTimer = repathInterval;
            }

            // Sparare non interrompe il movimento.
            if (shooting != null)
            {
                if (distance <= attackRange)
                    shooting.StartShooting(player);
                else
                    shooting.StopShooting();
            }
        }
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            while (state == State.Roaming && pathFinding.HasPath)
                yield return new WaitForSeconds(0.25f);

            if (state != State.Roaming)
                yield break;

            yield return new WaitForSeconds(roamingWaitTime);

            if (state != State.Roaming)
                yield break;

            bool destinationFound = false;

            for (int i = 0; i < 10; i++)
            {
                Vector2 destination =
                    navigationGrid.GetRandomWalkablePosition();

                if (float.IsNaN(destination.x) ||
                    float.IsNaN(destination.y))
                    continue;

                float distance = Vector2.Distance(
                    transform.position, destination);

                if (distance < minimumDestinationDistance)
                    continue;

                pathFinding.SetDestination(destination);

                if (pathFinding.HasPath)
                {
                    destinationFound = true;
                    break;
                }
            }

            if (!destinationFound)
                yield return new WaitForSeconds(1f);
        }
    }

    private void OnDisable()
    {
        if (aiCoroutine != null)
        {
            StopCoroutine(aiCoroutine);
            aiCoroutine = null;
        }

        if (shooting != null)
            shooting.StopShooting();
    }
}
