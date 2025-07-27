using UnityEngine;

public class ImpactHandler : MonoBehaviour
{
    [SerializeField] private SurfaceImpactDatabase _database;

    [Header("Pools")]
    [SerializeField] private GameObjectPool _decalPool;
    [SerializeField] private GameObjectPool _particlePool;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSourcePrefab;

    public void HandleImpact(RaycastHit hit)
    {
        // Yüzey tipi belirleniyor
        SurfaceTypeIdentifier surface = hit.collider.GetComponent<SurfaceTypeIdentifier>();
        SurfaceType type = surface ? surface.SurfaceType : SurfaceType.Default;

        var data = _database.GetData(type);
        if (data == null) return;

        Vector3 pos = hit.point + hit.normal * 0.01f;
        Quaternion rot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0, 180f, 0);

        // Decal
        if (data.DecalPrefab != null && _decalPool != null)
            _decalPool.GetNext(pos, rot);

        // Partikül
        if (data.ParticlePrefab != null && _particlePool != null)
            _particlePool.GetNext(pos, rot);

        // Ses
        if (data.ImpactSound != null && _audioSourcePrefab != null)
        {
            AudioSource audio = Instantiate(_audioSourcePrefab, pos, Quaternion.identity);
            audio.clip = data.ImpactSound;
            audio.Play();
            Destroy(audio.gameObject, audio.clip.length);
        }
    }
}