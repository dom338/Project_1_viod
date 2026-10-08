using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int initialLife = 3;
    private int currentLife;
    private EnemyFlash enemyFlash;

    private void Awake()
    {
        enemyFlash = GetComponent<EnemyFlash>();
        currentLife = initialLife;
    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        Debug.Log(currentLife);
        StartCoroutine(enemyFlash.FlashRutine());
        StartCoroutine(CheckDetectDeathRoutine());

    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(enemyFlash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
