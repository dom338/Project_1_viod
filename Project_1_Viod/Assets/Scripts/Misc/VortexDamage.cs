using UnityEngine;

public class VortexDamage : MonoBehaviour
{
    [SerializeField] private float attractionForce = 12f;
    [SerializeField] private float timeBeforeDamage = 1.5f;

    private Rigidbody2D playerRB;
    private PlayerShipHealth playerHealth;
    private float timeInside;
    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRB = collision.attachedRigidbody;
            playerHealth = collision.GetComponent<PlayerShipHealth>();

            isPlayerInside = true;
            timeInside = 0f;

        }
    }

    private void FixedUpdate()
    {

        if (isPlayerInside)
        {
            Vector2 whirlpoolPosition = new Vector2(transform.position.x, transform.position.y);

            Vector2 direction = whirlpoolPosition - playerRB.position;

            if (direction.sqrMagnitude > 0.01f)
            {
                playerRB.AddForce(direction.normalized * attractionForce, ForceMode2D.Force);
            }
        }

    }

    private void Update()
    {
        if (isPlayerInside)
        {
            timeInside += Time.deltaTime;
        }

        if (timeInside >= timeBeforeDamage && isPlayerInside)
        {
            playerHealth.ApplyDamage(1);
            timeInside = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            timeInside = 0;
            playerRB = null;
            playerHealth = null;
        }

    }
}

