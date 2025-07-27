using UnityEngine;

[CreateAssetMenu(menuName = "Impact/Surface Impact Database")]
public class SurfaceImpactDatabase : ScriptableObject
{
    public SurfaceImpactData[] SurfaceImpacts;

    public SurfaceImpactData GetData(SurfaceType type)
    {
        foreach (var data in SurfaceImpacts)
        {
            if (data.SurfaceType == type)
                return data;
        }
        return null;
    }
}