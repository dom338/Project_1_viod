using System.Collections;
using UnityEngine;

public class KrakenAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float emergeDistance = 3f;
    [SerializeField] private float emergeSpeed = 4f;
    [SerializeField] private float timeBetweenAmbushes = 3f;
    [SerializeField] private float attackRadius = 2.5f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float warningTime = 1.5f;
    [SerializeField] private float attackRecovery = 0.8f;
    [SerializeField] private GameObject attackZone;

    private Rigidbody2D RB;
    private bool isActive = true;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        if (attackZone != null)
        {
            attackZone.SetActive(false);
        }
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");


        if (playerObject == null)
        {
            Debug.LogError("KrakenAI: nessun oggetto con tag Player trovato!", this);
            return;
        }

        player = playerObject.transform;

        StartCoroutine(AmbushRoutine());
    }

    private IEnumerator AmbushRoutine()
    {
        while (isActive)
        {
            while (player != null && Vector2.Distance(transform.position, player.position) > detectionRadius)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Vector2 offset = Random.insideUnitCircle.normalized * emergeDistance;
            Vector2 emergePosition = (Vector2)player.position + offset;

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("KrakenAI: il campo Visual non è assegnato nell'Inspector!");
            }


            yield return MoveTo(emergePosition);

            Vector2 impactPosition = player.position;

            if (attackZone != null)
            {
                attackZone.transform.position = impactPosition;
                attackZone.transform.localScale = Vector3.one * attackRadius * 2f;
                attackZone.SetActive(true);
            }

            yield return new WaitForSeconds(warningTime);

            if (player != null)
            {
                float distance = Vector2.Distance(player.position, impactPosition);

                if (distance <= attackRadius)
                {
                    PlayerShipHealth health = player.GetComponentInParent<PlayerShipHealth>();

                    if (health != null)
                    {
                        health.ApplyDamage(damage);
                    }
                    else
                    {
                        Debug.LogWarning(
                            "PlayerShipHealth non trovato sul Player.",
                            this
                        );
                    }
                }
            }

            if (attackZone != null)
                attackZone.SetActive(false);

            yield return new WaitForSeconds(attackRecovery);

            // Si immerge.
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            yield return new WaitForSeconds(timeBetweenAmbushes);
        }
    }

    private IEnumerator MoveTo(Vector2 destination)
    {
        while (Vector2.Distance(RB.position, destination) > 0.5f)
        {
            Vector2 nextPosition = Vector2.MoveTowards(RB.position, destination, emergeSpeed * Time.fixedDeltaTime);

            RB.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        RB.MovePosition(destination);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
