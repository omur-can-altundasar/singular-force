using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMover : MonoBehaviour
{
    [Header("Player Movement Settings")]
    private CharacterController _characterController;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void ApplyGravity()
    {
        _verticalVelocity += _gravity * Time.deltaTime;
        _verticalVelocity = Mathf.Max(_verticalVelocity, -20f);
        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    public void Move(Vector2 inputMovement)
    {
        Vector3 move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        _characterController.Move(_moveSpeed * Time.deltaTime * move);

        ApplyGravity();
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
        }
    }
}
