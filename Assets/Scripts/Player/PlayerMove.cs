using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private const float FLIP_BACK = -180f;
    private const float FLIP_DEFAULT = 0f;

    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private BoxCollider2D _playerCollider;
    [SerializeField] private float _playerSpeed = 2f;
    [SerializeField] private float _jumpHeight = 2f;

    private InputActions _inputActions;
    private List<Collider2D> _groundColliders;

    bool IsGrounded
    {
        get
        {
            return _groundColliders.Count > 0;
        }
    }

    private void Awake()
    {
        _groundColliders = new List<Collider2D>();
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

    private void OnJumpButtonPerformed(InputAction.CallbackContext context)
    {
        if (IsGrounded) _playerRb.linearVelocityY = _jumpHeight;
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!_groundColliders.Contains(coll.collider))
            foreach (var p in coll.contacts)
                if (p.point.y < _playerCollider.bounds.min.y)
                {
                    _groundColliders.Add(coll.collider);
                    break;
                }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (_groundColliders.Contains(coll.collider))
            _groundColliders.Remove(coll.collider);
    }

    private void OnDestroy()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        var inputDirection = _inputActions.Player.Move.ReadValue<Vector2>();

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