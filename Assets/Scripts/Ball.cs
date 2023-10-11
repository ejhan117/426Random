using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float normalSpeed = 7.5f;
    public float speed = 9.0f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public Player pOne;
    public Player pTwo;
    private AudioSource audioSource;

    public AudioSource scoreAudioSource;
    public AudioClip scoreSound;

    public bool isClone = false;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start called on instance: " + this);
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.Log("Couldn't find rb");
        }
        if (!isClone)
        {
            StartCoroutine(StartAfterDelay());
        }
        audioSource = GetComponent<AudioSource>();
        GameObject pOneObject = GameObject.Find("PlayerOne");
        GameObject pTwoObject = GameObject.Find("PlayerTwo");
        pOne = pOneObject.GetComponent<Player>();
        pTwo = pTwoObject.GetComponent<Player>();
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
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
            audioSource.Play();
        }

        // If the ball hits a paddle, invert its x-direction
        if (col.gameObject.name == "PlayerOne" || col.gameObject.name == "PlayerTwo")
        {
            direction.x = -direction.x;
            rb.velocity = direction * speed;
            audioSource.Play();
            Player player = col.gameObject.GetComponent<Player>();
            if(player != null && player.readySplit)
            {
                //if (isClone)
                //{
                //    return;
                //}
                Split();
                player.readySplit = false;
            }
        }

        // If the ball hits the Left or Right Wall , reset ball and give point
        if (col.gameObject.name == "Left Wall")
        {
            pTwo.Score();
            pOne.AddPowerUp();
            pOne.UpdatePowerUpUI();
            PLayScoreSound();
            if (!isClone)
            {
                ResetBall();
            }
        }
        else if(col.gameObject.name == "Right Wall")
        {
            pOne.Score();
            pTwo.AddPowerUp();
            pTwo.UpdatePowerUpUI();
            PLayScoreSound();
            if (!isClone)
            {
                ResetBall();
            }
        }
    }

    // Call this method when a goal is scored to reset the ball
    public void ResetBall()
    {
        //TODO: Check if ball is a new ball (From the power up), if so, delete instead of starting coroutine
        Debug.Log("HERE!");
        rb.velocity = Vector2.zero;
        Debug.Log(rb.velocity);
        transform.position = Vector2.zero;
        speed = 7.5f;
        StartCoroutine(StartAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = direction * speed;
    }

    void PLayScoreSound() {
        scoreAudioSource.PlayOneShot(scoreSound);
    
    }

    public void Split()
    {
        // Create a new ball at the current position
        GameObject newBallObject = Instantiate(gameObject, transform.position + new Vector3(0.1f, 0.1f, 0), Quaternion.identity);

        // Get the Ball script component of the new ball
        Ball newBall = newBallObject.GetComponent<Ball>();
        newBall.isClone = true;

        // Temporarily disable colliders
        Collider2D newBallCollider = newBallObject.GetComponent<Collider2D>();
        Collider2D originalBallCollider = GetComponent<Collider2D>();
        newBallCollider.enabled = false;
        originalBallCollider.enabled = false;

        // Re-enable colliders after a short delay
        StartCoroutine(EnableCollidersAfterDelay(newBallCollider, originalBallCollider, 0.1f));


        // Optionally, you can set the new ball's direction to be different from the current ball
        Vector2 newDirection = new Vector2(direction.x, -direction.y).normalized;
        newBall.SetDirection(newDirection);
    }

    public void SetDirection(Vector2 newDirection)
    {
        Debug.Log("SetDirection called on instance: " + this);
        if (rb == null)
        {
            Debug.Log("No rigidBody!");
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        direction = newDirection;
        rb.velocity = direction * speed;
    }

    IEnumerator EnableCollidersAfterDelay(Collider2D newBallCollider, Collider2D originalBallCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        newBallCollider.enabled = true;
        originalBallCollider.enabled = true;
    }

    public void UpdateSpeed()
    {
        rb.velocity = direction * speed;
    }
}
