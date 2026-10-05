using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    private Rigidbody2D RB;
    private Animator animator;
    private Vector2 moveInput;
    private InputSystem_Actions inputActions;

    [SerializeField] private float mouvmentSpeed = 5f;


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

    private void ODisable()
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
        RB.MovePosition(RB.position + moveInput * mouvmentSpeed * Time.fixedDeltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }
}
