using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private const float FLIP_BACK = -180f;
    private const float FLIP_DEFAULT = 0f;

    [SerializeField] private GroundCheckLogic _groundCheckLogic;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private BoxCollider2D _playerCollider;
    [SerializeField] private float _playerSpeed = 2f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _velocityScale = 2f;

    private InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    private void OnEnable()
    {
        _inputActions.Player.Jump.performed += OnJumpButtonPerformed;
    }

    private void OnDisable()
    {
        _inputActions.Player.Jump.performed -= OnJumpButtonPerformed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_groundCheckLogic.IsGrounded is false) _playerRb.linearVelocityY -= _velocityScale;
    }

    private void OnJumpButtonPerformed(InputAction.CallbackContext context)
    {
        if (_groundCheckLogic.IsGrounded) _playerRb.linearVelocityY = _jumpHeight;         
    }

    private void OnDestroy()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        var inputDirection = _inputActions.Player.Move.ReadValue<Vector2>();
        print(_groundCheckLogic.IsGrounded);
        Movement(inputDirection);
    }

    private void Movement(Vector2 moveDirection)
    {
        if (moveDirection.x < 0)
        {
            transform.localEulerAngles = new Vector3(0f, FLIP_BACK);
        }
        else if (moveDirection.x > 0)
        {
            transform.localEulerAngles = new Vector3(0f, FLIP_DEFAULT);
        }

        _playerRb.linearVelocityX = moveDirection.x * _playerSpeed;
    }
}