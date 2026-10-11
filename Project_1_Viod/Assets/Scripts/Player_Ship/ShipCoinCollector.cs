using UnityEngine;

public class ShipCoinCollector : MonoBehaviour
{
    public int coins = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coins++;

            Debug.Log("coin collected = " + coins);

            Destroy(collision.gameObject);

        }
    }

}
