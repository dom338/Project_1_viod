using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float lookAheadDistance = 2f;

    private MovingShip_Player ship;

    private void Awake()
    {
        ship = player.GetComponent<MovingShip_Player>();
    }

    private void LateUpdate()
    {
        Vector3 lookAhead =
            (Vector3)(ship.CurrentDirection * lookAheadDistance);

        Vector3 targetPosition =
            player.position + lookAhead;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
