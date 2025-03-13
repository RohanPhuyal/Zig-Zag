using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool; // Required for EventSystem

public class BallController : MonoBehaviour
{
    public GameObject platformspawn;
    public GameObject particle;
    [SerializeField]
    private float speed;
    
    public Material ballMaterial; // Assign material in Inspector

    Rigidbody rb;

    bool started;

    bool gameOver;

    bool levelUp;
    
    private ObjectPool<GameObject> particlePool;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        started = false;
        gameOver = false;
        levelUp = false;
        TrailEffect();
        // Initialize Particle Pool
        particlePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(particle),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 5
        );
    }

    void TrailEffect()
    {
        TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();

        trail.time = 0.5f; // Duration the trail remains visible
        trail.startWidth = 0.2f;
        trail.endWidth = 0.05f;
        trail.material = new Material(Shader.Find("Sprites/Default")); // Basic material

        // Set a color gradient with transparency
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } // 50% opacity to 0%
        );

        trail.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                rb.linearVelocity = new Vector3(speed, 0, 0);
                started = true;
                
                GameManager.instance.StartGame();
            }
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if(!Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            GameOver();
        }

        if (Input.GetMouseButtonDown(0) && !gameOver && !levelUp)
        {
            SwitchDirection();
        }
        // Get ball velocity for scrolling effect
        Vector2 offset = new Vector2(Time.deltaTime * rb.linearVelocity.magnitude * speed, 0);
        ballMaterial.SetTextureOffset("_BaseMap", offset);
    }
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    

    void SwitchDirection()
    {
        if (rb.linearVelocity.z > 0)
        {
            rb.linearVelocity = new Vector3(speed, 0, 0);
        } else if (rb.linearVelocity.x > 0){
            rb.linearVelocity = new Vector3(0, 0, speed);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Diamond")
        {
            //GameObject part = Instantiate(particle, col.gameObject.transform.position, Quaternion.identity) as GameObject;
            GameObject plat = particlePool.Get();
            plat.transform.position = col.gameObject.transform.position;
            
            // Start a coroutine to handle delayed release
            StartCoroutine(DelayedReleaseCoroutine(plat, 1f));
            Destroy(col.gameObject);
        }
    }
    private IEnumerator DelayedReleaseCoroutine(GameObject plat, float delay)
    {
        yield return new WaitForSeconds(delay);
        particlePool.Release(plat);
    }
    void GameOver()
    {
        gameOver = true;
        rb.linearVelocity = new Vector3(0, -25f, 0);

        Camera.main.GetComponent<CameraFollow>().gameOver = true;
        platformspawn.GetComponent<PlatformSpawner>().gameOver = true;
        GameManager.instance.GameOver();
    }

    public void LevelUp()
    {
        levelUp = true;
        rb.linearVelocity = new Vector3(0, -25f, 0);
        Camera.main.GetComponent<CameraFollow>().levelUp = true;
        platformspawn.GetComponent<PlatformSpawner>().levelUp = true;
        GameManager.instance.LevelUp();
    }
}
