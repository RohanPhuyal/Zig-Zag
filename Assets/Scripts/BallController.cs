using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool; // Required for EventSystem

public class BallController : MonoBehaviour
{
    public GameObject platformspawn;
    public GameObject particle;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject tapText;

    Rigidbody rb;

    bool started;

    bool gameOver;

    bool levelUp;
    
    private ObjectPool<GameObject> particlePool;
    
    private string moveDirection;
    private Vector3 previousPosition;
    public float ballRadius = 0.5f; // Set your ball's radius here


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
        previousPosition = transform.position;
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
            // Mouse input
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                //rb.linearVelocity = new Vector3(speed, 0, 0);
                //rb.AddForce(Vector3.right * speed);
                started = true;
                moveDirection = "x";
                GameManager.instance.StartGame();
            }

            // Mobile touch input
            if (Input.touchCount > 0 && !IsPointerOverUI())
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    //rb.linearVelocity = new Vector3(speed, 0, 0);
                    //rb.AddForce(Vector3.right * speed);
                    started = true;
                    moveDirection = "x";
                    GameManager.instance.StartGame();
                }
            }
        }
        else
        {
            Move();
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if(!Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                GameOver();
            }
        
            // Mouse or touch input for direction switch
            if ((Input.GetMouseButtonDown(0) && !gameOver && !levelUp) || 
                (Input.touchCount > 0 && !gameOver && !levelUp && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                SwitchDirection();
            }
        }
    }

    /*void FixedUpdate()
    {
        if (started)
        {
            if (moveDirection == "x")
            {
                //rb.AddForce(new Vector3(speed, 0, 0), ForceMode.Impulse);
                rb.linearVelocity = new Vector3(speed, 0, 0);
            }
            else if (moveDirection == "z")
            {
                //rb.AddForce(new Vector3(0, 0, speed), ForceMode.Impulse);
                rb.linearVelocity = new Vector3(0, 0, speed);
            }
        }
    }*/


    public void IncreaseSpeed()
    {
        if (GameManager.instance.gameStarted && ScoreManager.instance.score % 500 == 0 && ScoreManager.instance.score != 0)
        {
            speed++;
            GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>().spawnSpeed-=0.1f;
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == tapText) // Check if tapText was clicked
            {
                return false; // Return false if clicked on tapText
            }
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;

        switch (moveDirection)
        {
            case "x":
                movement = new Vector3(speed, 0, 0) * Time.deltaTime;
                break;
            case "z":
                movement = new Vector3(0, 0, speed) * Time.deltaTime;
                break;
        }

        transform.position += movement;

        // Calculate movement delta
        Vector3 deltaPos = transform.position - previousPosition;
        float distanceMoved = deltaPos.magnitude;

        if (distanceMoved > 0.0001f) // avoid tiny jitters
        {
            // Get rotation axis (cross product of movement direction and up)
            Vector3 rotationAxis = Vector3.Cross(deltaPos.normalized, Vector3.up);
        
            // Calculate rotation angle in degrees: angle = distance / radius (in radians) → convert to degrees
            float angle = (distanceMoved / ballRadius) * Mathf.Rad2Deg;

            // Apply rotation
            transform.Rotate(rotationAxis, -angle, Space.World);
        }

        // Store for next frame
        previousPosition = transform.position;

    }
    void SwitchDirection()
    {
        // On tap to change direction
        AudioManager.Instance.PlayBallTap();
        if (moveDirection == "x")
        {
            moveDirection = "z";
        }else if (moveDirection == "z")
        {
            moveDirection = "x";
        }
        /*if (rb.linearVelocity.z > 0)
        {
            moveDirection = "x";
            //rb.linearVelocity = Vector3.zero; // ← Reset velocity for instant turn
            //rb.linearVelocity = new Vector3(speed, 0, 0);
        } else if (rb.linearVelocity.x > 0){
            moveDirection = "z";
            //rb.linearVelocity = Vector3.zero; // ← Reset velocity for instant turn
            //rb.linearVelocity = new Vector3(0, 0, speed);
        }*/
        Debug.Log(moveDirection);
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
            platformspawn.GetComponent<PlatformSpawner>().ReleaseDiamond(col.gameObject);
        }
    }
    private IEnumerator DelayedReleaseCoroutine(GameObject plat, float delay)
    {
        yield return new WaitForSeconds(delay);
        particlePool.Release(plat);
    }
    void GameOver()
    {
        started = false;
        gameOver = true;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.linearVelocity = new Vector3(0, -25f, 0);

        Camera.main.GetComponent<CameraFollow>().gameOver = true;
        platformspawn.GetComponent<PlatformSpawner>().gameOver = true;
        GameManager.instance.GameOver();
    }

    /*public void LevelUp()
    {
        levelUp = true;
        rb.linearVelocity = new Vector3(0, -25f, 0);
        Camera.main.GetComponent<CameraFollow>().levelUp = true;
        platformspawn.GetComponent<PlatformSpawner>().levelUp = true;
        GameManager.instance.LevelUp();
    }*/
}
