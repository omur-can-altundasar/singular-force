using UnityEngine;

public class PlayerLooker : MonoBehaviour
{
    [Header("Player Look Settings")]
    [SerializeField] private float _lookSpeed = 10f;
    private float _rotationX;
    private float _rotationY;
    [SerializeField] private Transform upperPivotPoint;

    public void Look(Vector2 inputRotation)
    {
        _rotationX += _lookSpeed * Time.deltaTime * inputRotation.y;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        _rotationY += _lookSpeed * Time.deltaTime * inputRotation.x;

        upperPivotPoint.localRotation = Quaternion.Euler(-_rotationX, 0, 0);
        transform.rotation = Quaternion.Euler(0, _rotationY, 0);
    }
}
