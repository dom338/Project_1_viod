
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float damage = 10f;

    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
