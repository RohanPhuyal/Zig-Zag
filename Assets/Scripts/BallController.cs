using UnityEngine;
using UnityEngine.EventSystems; // Required for EventSystem

public class BallController : MonoBehaviour
{
    public GameObject platformspawn;
    public GameObject particle;
    [SerializeField]
    private float speed;

    Rigidbody rb;

    bool started;

    bool gameOver;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        started = false;
        gameOver = false;
        TrailEffect();
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

        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            SwitchDirection();
        }
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
            GameObject part = Instantiate(particle, col.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(part,1f);
            Destroy(col.gameObject);
        }
    }
    void GameOver()
    {
        gameOver = true;
        rb.linearVelocity = new Vector3(0, -25f, 0);

        Camera.main.GetComponent<CameraFollow>().gameOver = true;
        platformspawn.GetComponent<PlatformSpawner>().gameOver = true;
        GameManager.instance.GameOver();
    }
}
