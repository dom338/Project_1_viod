using System;
using System.Collections;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    private enum State
    {
        roaming
    }

    private State state;
    private EnemyShipPathFinding enemyShipPathFinding;

    private void Awake()
    {
        enemyShipPathFinding = GetComponent<EnemyShipPathFinding>();
        state = State.roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRutine());
    }

    private IEnumerator RoamingRutine()
    {
        while (state == State.roaming)
        {
            Vector2 roamPos = GetRoamingPosition();
            enemyShipPathFinding.MoveTo(roamPos);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }


}
