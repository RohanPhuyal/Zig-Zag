using System.Collections.Generic;
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
    
    // Stores all platforms and their assigned colors
    public Dictionary<GameObject, Color> allPlatforms = new Dictionary<GameObject, Color>();

    void Start()
    {
        lastPos = platformPrefab.transform.position;
        size = platformPrefab.transform.localScale.x;

        // Initialize Platform Pool
        platformPool = new ObjectPool<GameObject>(
            createFunc: () => 
            {
                GameObject newPlatform = Instantiate(platformPrefab);

                // Store the platform reference and its default color
                Renderer rend = newPlatform.GetComponent<Renderer>();
                if (rend)
                {
                    if (GameManager.instance.platformColor != null)
                    {
                        if (GameManager.instance.platformColor != rend.material.color)
                        {
                            if (ScoreManager.instance != null && ScoreManager.instance.score > 49)
                            {
                                rend.material.color = GameManager.instance.platformColor;
                            }
                        }
                    }
                    allPlatforms[newPlatform] = rend.material.color;
                }
                return newPlatform;
            },
            actionOnGet: obj => obj.SetActive(true),  // No color changes here!
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 30,
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
        InvokeRepeating(nameof(SpawnPlatforms), 1f, 0.3f);
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
