using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Gun/SurfaceImpactDatabase")]
public class SurfaceImpactDatabase : ScriptableObject
{
    [System.Serializable]
    public class SurfaceImpactEntry
    {
        public SurfaceType type;
        public GameObject decalPrefab;
    }

    public SurfaceImpactEntry[] entries;

    public GameObject GetDecal(SurfaceType type)
    {
        foreach (var entry in entries)
        {
            if (entry.type == type)
                return entry.decalPrefab;
        }
        return null;
    }
}
