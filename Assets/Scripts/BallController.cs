using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Input.GetMouseButtonDown(0))
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
