
using System.Collections;
using UnityEngine;

public class EnemyShipShooting : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 1.5f;

    private Transform target;
    private Coroutine shootingCoroutine;

    public void StartShooting(Transform newTarget)
    {
        target = newTarget;

        // Evita di avviare più coroutine contemporaneamente.
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootingRoutine());
        }
    }

    public void StopShooting()
    {
        target = null;

        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootingRoutine()
    {
        while (target != null)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                // Calcola la direzione verso la posizione attuale
                // del giocatore.
                Vector2 direction =
                    ((Vector2)target.position -
                     (Vector2)firePoint.position).normalized;

                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.identity
                );

                EnemyProjectile cannonball =
                    projectile.GetComponent<EnemyProjectile>();

                if (cannonball != null)
                {
                    cannonball.SetDirection(direction);
                }
                else
                {
                    Debug.LogError(
                        "Il prefab del proiettile non contiene EnemyCannonball!"
                    );
                    Destroy(projectile);
                }
            }

            yield return new WaitForSeconds(fireRate);
        }

        shootingCoroutine = null;
    }

    private void OnDisable()
    {
        StopShooting();
    }
}

