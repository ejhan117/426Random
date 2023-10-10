using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public Player pOne;
    public Player pTwo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartAfterDelay());
        GameObject pOneObject = GameObject.Find("PlayerOne");
        GameObject pTwoObject = GameObject.Find("PlayerTwo");
        pOne = pOneObject.GetComponent<Player>();
        pTwo = pTwoObject.GetComponent<Player>();
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(2.0f);
        LaunchBall();
    }
    void LaunchBall()
    {
        // Randomly decide the initial direction of the ball
        if (Random.Range(0, 2) == 0)
        {
            direction = new Vector2(1, Random.Range(-1.0f, 1.0f)).normalized;
        }
        else
        {
            direction = new Vector2(-1, Random.Range(-1.0f, 1.0f)).normalized;
        }

        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the ball hits the top or bottom wall, invert its y-direction
        if (col.gameObject.name == "Top Wall" || col.gameObject.name == "Bottom Wall")
        {
            direction.y = -direction.y;
            rb.velocity = direction * speed;
        }

        // If the ball hits a paddle, invert its x-direction
        if (col.gameObject.name == "PlayerOne" || col.gameObject.name == "PlayerTwo")
        {
            direction.x = -direction.x;
            rb.velocity = direction * speed;
        }

        // If the ball hits the Left or Right Wall , reset ball and give point
        if (col.gameObject.name == "Left Wall")
        {
            pTwo.Score();
            ResetBall();
        }
        else if(col.gameObject.name == "Right Wall")
        {
            pOne.Score();
            ResetBall();
        }
    }

    // Call this method when a goal is scored to reset the ball
    public void ResetBall()
    {
        //TODO: Check if ball is a new ball (From the power up), if so, delete instead of starting coroutine
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        StartCoroutine(StartAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
    }
}
