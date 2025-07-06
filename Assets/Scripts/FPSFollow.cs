using UnityEngine;

public class FPSFollow : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }

}
