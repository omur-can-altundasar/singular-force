using UnityEngine;

[System.Serializable]
public class SurfaceImpactData
{
    public SurfaceType SurfaceType;
    public GameObject DecalPrefab;
    public GameObject ParticlePrefab;
    public AudioClip ImpactSound;
}