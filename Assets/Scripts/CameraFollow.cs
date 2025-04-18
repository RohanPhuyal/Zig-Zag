using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ball;
    Vector3 offset;

    public float lerpRate;
    public bool gameOver;
    public bool levelUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = ball.transform.position - transform.position;
        gameOver = false;
        levelUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && !levelUp)
        {
            Follow();
        }
        
    }
    void Follow()
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = ball.transform.position - offset;
        pos = Vector3.Lerp(pos,targetPos,lerpRate*Time.deltaTime);
        transform.position = pos;
    }
}
