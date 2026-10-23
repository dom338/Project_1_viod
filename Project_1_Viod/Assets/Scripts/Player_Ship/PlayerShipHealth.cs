using UnityEngine;

public class PlayerShipHealth : MonoBehaviour
{
    [SerializeField] private int max_Life = 10;
    [SerializeField] private int current_Life;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_Life = max_Life;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            ApplyDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void ApplyDamage(int damage)
    {
        current_Life -= damage;

        // Evita che la vita diventi negativa.
        current_Life = Mathf.Max(current_Life, 0);

        Debug.Log("Vita nave giocatore: " + current_Life);

        if (current_Life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("La nave del giocatore è stata distrutta!");

        Destroy(gameObject);
    }
}
