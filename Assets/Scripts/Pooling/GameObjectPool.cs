using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 30;

    private GameObject[] pool;
    private int currentIndex = 0;

    void Awake()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    public GameObject GetNext(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool[currentIndex];

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(false); // reset (isteğe bağlı)
        obj.SetActive(true);  // yeniden aktif et

        currentIndex = (currentIndex + 1) % poolSize;

        return obj;
    }
}