using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platform;
    public GameObject diamonds;
    Vector3 lastPos;
    float size;
    public bool gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPos = platform.transform.position;
        size = platform.transform.localScale.x;

        for (int i = 0; i < 20; i++)
        {
            SpawnPlatforms();
        }

        InvokeRepeating("SpawnPlatforms",1f,0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            CancelInvoke("SpawnPlatforms");
        }
    }
    void SpawnPlatforms()
    {

        int rand = Random.Range(0, 6);
        if (rand < 3)
        {
            SpawnX();
        }
        else if (rand <= 3)
        {
            SpawnZ();
        }
    }
    void SpawnX()
    {
        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = pos;
        Instantiate(platform,pos, Quaternion.identity);

        SpawnDiamonds(pos);

    }
    void SpawnZ()
    {
        Vector3 pos = lastPos;
        pos.z += size;
        lastPos = pos;
        Instantiate(platform, pos, Quaternion.identity);

        SpawnDiamonds(pos);
    }

    void SpawnDiamonds (Vector3 pos)
    {
        pos.y += 1;
        int rand = Random.Range(0, 4);
        if (rand < 1)
        {
            Instantiate(diamonds, pos, diamonds.transform.rotation);
        }
    }
}