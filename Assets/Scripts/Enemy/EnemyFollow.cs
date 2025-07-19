using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _followSpeed = 0.5f;
    [SerializeField] private float _minDistance = 5f;

    private void Update()
    {
        transform.LookAt(_player.transform);

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance > _minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, _player.position, _followSpeed * Time.deltaTime);
        }
    }
}
