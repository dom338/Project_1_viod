using UnityEngine;
using UnityEngine.InputSystem;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] private Transform FirePointRoot;
    [SerializeField] private Transform FirePointUP;
    [SerializeField] private Transform FirePointLeft;
    [SerializeField] private Transform FirePointRight;
    private Vector2 shootForward;
    private Vector2 shootRight;
    private Vector2 shootLeft;

    private MovingShip_Player ship;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        ship = GetComponent<MovingShip_Player>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Attack.performed += OnShoot;
    }
    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnShoot;

        inputActions.Disable();

    }

    private void Update()
    {
        UpdateFirePointRotation();
    }

    private void UpdateFirePointRotation()
    {
        Vector2 direction = ship.CurrentDirection;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        float snappedAngle = Mathf.Round(angle / 45f) * 45f;

        FirePointRoot.localRotation = Quaternion.Euler(
            0,
            0,
            snappedAngle - 90f
        );

        float angleRad = snappedAngle * Mathf.Deg2Rad;

        shootForward = new Vector2(
            Mathf.Cos(angleRad),
            Mathf.Sin(angleRad)
        );

        shootRight = new Vector2(
            shootForward.y,
            -shootForward.x
        );

        shootLeft = -shootRight;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        CreateBullet(
       FirePointUP,
       shootForward
   );

        CreateBullet(
            FirePointLeft,
            shootLeft
        );

        CreateBullet(
            FirePointRight,
            shootRight
        );
    }

    private void CreateBullet(Transform firePoint, Vector2 direction)
    {
        GameObject newBullet = Instantiate(
            BulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Bullet bullet = newBullet.GetComponent<Bullet>();

        Vector2 shipVelocity = ship.CurrentVelocity;

        bullet.SetDirection(
            direction,
            shipVelocity
        );

    }
}



