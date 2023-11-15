using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float normalSpeed = 9.0f;
    public float speed = 9.0f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public Player pOne;
    public Player pTwo;
    private AudioSource audioSource;

    public AudioSource scoreAudioSource;
    public AudioClip scoreSound;

    public bool isClone = false;

    public TextMeshProUGUI countdownText;

    //For CurveBall
    private bool isCurveActive = false;
    private float curveTimer = 0f;
    private float curveDuration = 10f; // Duration of the curve effect
    private float curveStrength = 1f;

    //For ZigZag
    private bool isZigZagActive = false;
    private float zigZagTimer = 0f;
    private float zigZagDuration = 10f; // Duration of the zigzag effect
    private float zigZagChangeInterval = 0.25f; // Time interval to change direction
    private float nextZigZagChangeTime = 0f;

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
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1.0f); // Wait an additional second after displaying "GO!"
        countdownText.gameObject.SetActive(false);
        speed = 9.0f;
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
            if (col.gameObject.name == "PlayerOne")
            {
                if (direction.x > 0)
                {
                    return;
                }
            }
            else
            {
                if (direction.x < 0)
                {
                    return;
                }
            }
            speed += 0.5f;
            normalSpeed += 0.5f;
            direction.x = -direction.x;
            direction.Normalize();
            rb.velocity = direction * speed;
            audioSource.Play();
            Player player = col.gameObject.GetComponent<Player>();
            if (player != null && player.splitBallCount > 0)
            {
                Split(player.splitBallCount); // Pass the splitBallCount to the Split method
                player.UseSplitBallPowerUp(); // This should reset the splitBallCount to 0
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
                else
                {
                    rb.velocity = Vector2.zero;
                    Destroy(this.gameObject, 2.0f);
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
                else
                {
                    rb.velocity = Vector2.zero;
                    Destroy(this.gameObject, 2.0f);
                }
        }
    }

    // Call this method when a goal is scored to reset the ball
    public void ResetBall()
    {
        //TODO: Check if ball is a new ball (From the power up), if so, delete instead of starting coroutine
        transform.position = Vector2.zero;
        speed = 0.0f;
        normalSpeed = 9.0f;
        GetComponent<Renderer>().material.color = Color.white;
        StartCoroutine(StartAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (isCurveActive)
        {
            curveTimer -= Time.deltaTime;
            if (curveTimer <= 0)
            {
                StopCurveEffect();
            }

            ApplyCurve();
        }
        rb.velocity = direction * speed;

        if (pOne.isMagnetActive)
        {
            AdjustDirectionTowardsPaddle(pOne.gameObject);
        }

        if (pTwo.isMagnetActive)
        {
            AdjustDirectionTowardsPaddle(pTwo.gameObject);
        }

        if (isZigZagActive)
        {
            zigZagTimer -= Time.deltaTime;
            if (zigZagTimer <= 0)
            {
                StopZigZagEffect();
            }

            if (Time.time >= nextZigZagChangeTime)
            {
                nextZigZagChangeTime = Time.time + zigZagChangeInterval;
                ZigZagMovement();
            }
        }

        rb.velocity = direction * speed;
    }

    void PLayScoreSound() {
        scoreAudioSource.PlayOneShot(scoreSound);
    }

    public void Split(int splitCount)
    {
        float totalYChange = Mathf.Abs(2.0f * direction.y);
        float yIncrement = totalYChange / splitCount;
        for (int i = 0; i < splitCount; i++)
        {
            // Create a new ball at the current position
            GameObject newBallObject = Instantiate(gameObject, transform.position , Quaternion.identity);

            // Get the Ball script component of the new ball
            Ball newBall = newBallObject.GetComponent<Ball>();
            newBall.isClone = true;

            Vector2 newDirection = new Vector2(direction.x, direction.y - (yIncrement * (i+1))).normalized;
            newBall.SetDirection(newDirection);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        Debug.Log("SetDirection called on instance: " + this);
        if (rb == null)
        {
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
    public void StartCurveEffect()
    {
        isCurveActive = true;
        curveTimer = curveDuration;
    }

    public void StopCurveEffect()
    {
        isCurveActive = false;
    }
    private void ApplyCurve()
    {
        if (!isCurveActive) return;

        // Apply a simple sine wave based curve
        float curveAmount = Mathf.Sin(Time.time * curveStrength);
        direction.y += curveAmount * Time.deltaTime;
        direction.Normalize();
    }
    private void AdjustDirectionTowardsPaddle(GameObject paddle)
    {
        float distance = Vector2.Distance(paddle.transform.position, transform.position);
        float magnetRange = 7.5f; // Distance within which the magnet effect is active

        if (distance < magnetRange)
        {
            Vector2 directionToPaddle = (paddle.transform.position - transform.position).normalized;
            direction = Vector2.Lerp(direction, directionToPaddle, Time.deltaTime * 2f).normalized; // Adjust the lerp rate as needed
            rb.velocity = direction * speed;
        }
    }
    public void StartZigZagEffect()
    {
        isZigZagActive = true;
        zigZagTimer = zigZagDuration;
        nextZigZagChangeTime = Time.time + zigZagChangeInterval;
    }

    public void StopZigZagEffect()
    {
        isZigZagActive = false;
    }
    private void ZigZagMovement()
    {
        if (!isZigZagActive) return;

        // Invert the y direction to create a zigzag effect
        direction.y = -direction.y;
    }

}
