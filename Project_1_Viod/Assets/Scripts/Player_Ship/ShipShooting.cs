using UnityEngine;
using UnityEngine.InputSystem;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] private Transform FirepointUP;
    [SerializeField] private Transform FirePointLeft;
    [SerializeField] private Transform FirePointRight;

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

    private void OnShoot(InputAction.CallbackContext context)
    {
        Vector2 forward = ship.CurrentDirection;

        Vector2 right = new Vector2(
            forward.y,
            -forward.x
        );

        Vector2 left = -right;

        CreateBullet(FirepointUP, forward);
        CreateBullet(FirePointLeft, left);
        CreateBullet(FirePointRight, right);

    }

    private void CreateBullet(Transform firePoint, Vector2 direction)
    {
        GameObject newBullet = Instantiate(BulletPrefab, firePoint.position, Quaternion.identity);

        Bullet bullet = newBullet.GetComponent<Bullet>();

        bullet.SetDirection(direction);

    }
}
