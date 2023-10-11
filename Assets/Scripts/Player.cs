using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Paddle
{
    public enum PlayerNum { Player1, Player2 };
    public PlayerNum playerNum;
    public Vector2 direction { get; private set; }
    public int score = 0;
    public TMP_Text scoreText;

    public List<PowerUp> powerUpInventory = new List<PowerUp>();

    public Image powerUp1Image;
    public TMP_Text powerUp1StockText;
    public Image powerUp2Image;
    public TMP_Text powerUp2StockText;
    public Image powerUp3Image;
    public TMP_Text powerUp3StockText;

    private int powerUp1Stock = 0;
    private int powerUp2Stock = 0;
    private int powerUp3Stock = 0;

    public bool readySplit = false;
    private void Start()
    {
        StartCoroutine(GenerateRandomPowerUp());
        UpdateScoreboard();
        GameObject textObject;
        switch (playerNum)
        {
            case PlayerNum.Player2:
                textObject = GameObject.Find("PlayerTwoScore");
                scoreText = textObject.GetComponent<TMP_Text>();
                break;
            case PlayerNum.Player1:
                textObject = GameObject.Find("PlayerOneScore");
                scoreText = textObject.GetComponent<TMP_Text>();
                break;
        }
        UpdatePowerUpUI();
    }

    private void Update()
    {
        switch (playerNum)
        {
            case PlayerNum.Player2:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    direction = Vector2.down;
                }
                else
                {
                    direction = Vector2.zero;
                }

                // Controller joystick controls
                float vertical = Input.GetAxis("Vertical");
                if (Mathf.Abs(vertical) > 0.1f)  // Deadzone
                {
                    direction = new Vector2(0, vertical).normalized;
                }
                break;


            case PlayerNum.Player1:
                if (Input.GetKey(KeyCode.W))
                {
                    direction = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    direction = Vector2.down;
                }
                else
                {
                    direction = Vector2.zero;
                }
                break;
        }

        if(playerNum == PlayerNum.Player1)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ActivatePowerUpOfType<ExpandPaddle>();
                if (powerUp1Stock > 0) powerUp1Stock--;
                UpdatePowerUpUI();
            }

            // Activate the first "Faster Ball" power-up in the inventory when '2' is pressed
            if (Input.GetKeyDown(KeyCode.O))
            {
                ActivatePowerUpOfType<LightningBall>();
                if (powerUp2Stock > 0) powerUp2Stock--;
                UpdatePowerUpUI();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ActivatePowerUpOfType<SplitBall>();
                if (powerUp3Stock > 0) powerUp3Stock--;
                UpdatePowerUpUI();
            }
        }

        if(playerNum == PlayerNum.Player2)
        {
            if (Input.GetKeyDown(KeyCode.Comma) || Input.GetButtonDown("Fire1"))
            {
                ActivatePowerUpOfType<ExpandPaddle>();
                if (powerUp1Stock > 0) powerUp1Stock--;
                UpdatePowerUpUI();
            }

            // Activate the first "Faster Ball" power-up in the inventory when '2' is pressed
            if (Input.GetKeyDown(KeyCode.Period) || Input.GetButtonDown("Fire2"))
            {
                ActivatePowerUpOfType<LightningBall>();
                if (powerUp2Stock > 0) powerUp2Stock--;
                UpdatePowerUpUI();
            }

            if (Input.GetKeyDown(KeyCode.Slash) || Input.GetButtonDown("Fire3"))
            {
                ActivatePowerUpOfType<SplitBall>();
                if (powerUp3Stock > 0) powerUp3Stock--;
                UpdatePowerUpUI();
            }
        }
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            // Directly set the velocity for more immediate control
            rigidbody.velocity = direction * speed;
        }
        else
        {
            // Stop the paddle when no input is given
            rigidbody.velocity = Vector2.zero;
        }
    }

    public void Score()
    {
        score++;
        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        scoreText.text = score.ToString();
    }

    private IEnumerator GenerateRandomPowerUp()
    {
        //TODO: Add the SplitBall Powerup
        while (true)
        {
            yield return new WaitForSeconds(10f);
            AddPowerUp();
            UpdatePowerUpUI();
        }
    }

    private IEnumerator DeactivatePowerUpAfterDuration(PowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.duration);
        powerUp.Deactivate(this);
    }


    private void ActivatePowerUpOfType<T>() where T : PowerUp
    {
        // Find the first power-up of the specified type
        PowerUp powerUpToActivate = powerUpInventory.Find(p => p is T);

        // If we found a power-up, activate it and remove it from the inventory
        if (powerUpToActivate != null)
        {
            powerUpToActivate.Activate(this);
            if (powerUpToActivate.duration > 0)
            {
                StartCoroutine(DeactivatePowerUpAfterDuration(powerUpToActivate));
            }
            powerUpInventory.Remove(powerUpToActivate);
        }
        else
        {
            Debug.Log("No power-up of type " + typeof(T) + " found in inventory.");
        }
    }

    public void UpdatePowerUpUI()
    {
        // Update the UI for Power-Up 1
        powerUp1StockText.text = powerUp1Stock.ToString();
        powerUp1Image.color = (powerUp1Stock > 0) ? Color.green : Color.gray;

        // Update the UI for Power-Up 2
        powerUp2StockText.text = powerUp2Stock.ToString();
        powerUp2Image.color = (powerUp2Stock > 0) ? Color.green : Color.gray;

        // Update the UI for Power-Up 3
        powerUp3StockText.text = powerUp3Stock.ToString();
        powerUp3Image.color = (powerUp3Stock > 0) ? Color.green : Color.gray;
    }

    public void AddPowerUp()
    {
        //TODO: Change range to 0,3 once implemented splitball
        Debug.Log("Adding powerup");
        int randomPowerUp = Random.Range(0, 3);
        switch (randomPowerUp)
        {
            case 0:
                powerUpInventory.Add(new ExpandPaddle());
                powerUp1Stock++;
                break;
            case 1:
                powerUpInventory.Add(new LightningBall());
                powerUp2Stock++;
                break;
            case 2:
                powerUpInventory.Add(new SplitBall());
                powerUp3Stock++;
                break;
        }
    }

}
