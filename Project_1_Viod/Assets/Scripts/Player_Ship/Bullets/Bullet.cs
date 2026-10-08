using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;
    private Vector2 shipVelocity;


    public void SetDirection(Vector2 newDirection, Vector2 newShipVelocity)
    {
        direction = newDirection.normalized;
        shipVelocity = newShipVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 finalVelocity = direction * speed + shipVelocity;

        transform.position += (Vector3)(finalVelocity * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
        }
    }
}
