using UnityEngine;
using UnityEngine.InputSystem;

public class MovingShip_Player : MonoBehaviour
{
    [SerializeField] float movingSpeed = 5;
    public float MovingSpeed => movingSpeed;
    [SerializeField] private float rotationSpeed = 180f;

    private Rigidbody2D RB;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 currentDirection = Vector2.up;
    private InputSystem_Actions inputActions;

    public Vector2 CurrentDirection => currentDirection;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        RotateShip();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            RB.MovePosition(
                RB.position + currentDirection * movingSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void RotateShip()
    {
        if (moveInput.sqrMagnitude < 0.01f)
            return;

        Vector2 targetDirection = moveInput.normalized;

        float currentAngle = Mathf.Atan2(
            currentDirection.y,
            currentDirection.x
        ) * Mathf.Rad2Deg;

        float targetAngle = Mathf.Atan2(
            targetDirection.y,
            targetDirection.x
        ) * Mathf.Rad2Deg;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            rotationSpeed * Time.fixedDeltaTime
        );

        float angleRad = newAngle * Mathf.Deg2Rad;

        currentDirection = new Vector2(
            Mathf.Cos(angleRad),
            Mathf.Sin(angleRad)
        );
    }

    public Vector2 CurrentVelocity
    {
        get
        {
            if (moveInput.sqrMagnitude > 0.01f)
            {
                return currentDirection * movingSpeed;
            }

            return Vector2.zero;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("InputX", currentDirection.x);
        animator.SetFloat("InputY", currentDirection.y);

        bool IsWalking = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool("IsWalking", IsWalking);

        if (IsWalking)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }
}
