using UnityEngine;
using UnityEngine.Pool;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject diamondPrefab;
    
    private Vector3 lastPos;
    private float size;
    
    public bool gameOver;
    public bool levelUp;

    private ObjectPool<GameObject> platformPool;
    private ObjectPool<GameObject> diamondPool;

    void Start()
    {
        lastPos = platformPrefab.transform.position;
        size = platformPrefab.transform.localScale.x;

        // Initialize Platform Pool
        platformPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(platformPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 40,
            maxSize: 50
        );

        // Initialize Diamond Pool
        diamondPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(diamondPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 30
        );

        // Spawn initial platforms
        for (int i = 0; i < 20; i++)
        {
            SpawnPlatforms();
        }
    }

    public void StartSpawningPlatforms()
    {
        InvokeRepeating(nameof(SpawnPlatforms), 0.3f, 0.2f);
    }

    public void StopSpawningPlatforms()
    {
        CancelInvoke(nameof(SpawnPlatforms));
    }

    void Update()
    {
        if (GameManager.instance.gameOver)
        {
            StopSpawningPlatforms();
        }
    }

    void SpawnPlatforms()
    {
        int rand = Random.Range(0, 6);
        if (rand < 3)
        {
            SpawnX();
        }
        else
        {
            SpawnZ();
        }
    }

    void SpawnX()
    {
        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = pos;

        GameObject platform = platformPool.Get();
        platform.transform.position = pos;

        // Assign spawner reference to the platform's TriggerChecker
        platform.GetComponentInChildren<TriggerChecker>().SetSpawner(this);

        SpawnDiamonds(pos);
    }

    void SpawnZ()
    {
        Vector3 pos = lastPos;
        pos.z += size;
        lastPos = pos;

        GameObject platform = platformPool.Get();
        platform.transform.position = pos;

        // Assign spawner reference to the platform's TriggerChecker
        platform.GetComponentInChildren<TriggerChecker>().SetSpawner(this);

        SpawnDiamonds(pos);
    }

    void SpawnDiamonds(Vector3 pos)
    {
        pos.y += 1;
        int rand = Random.Range(0, 7);
        if (rand < 1)
        {
            Instantiate(diamondPrefab, pos, diamondPrefab.transform.rotation);
        }
    }

    public void ReleasePlatform(GameObject platform)
    {
        platformPool.Release(platform);
    }

    public void ReleaseDiamond(GameObject diamond)
    {
        diamondPool.Release(diamond);
    }
}
