using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Paddle
{
    public enum PlayerNum { Player1, Player2 };
    public PlayerNum playerNum;
    public Vector2 direction { get; private set; }
    private int score = 0;
    public TMP_Text scoreText;

    public List<PowerUp> powerUpInventory = new List<PowerUp>();


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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivatePowerUpOfType<ExpandPaddle>();
        }

        // Activate the first "Faster Ball" power-up in the inventory when '2' is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivatePowerUpOfType<LightningBall>();
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
        scoreText.text = "Score: " + score;
    }

    private IEnumerator GenerateRandomPowerUp()
    {
        //TODO: Add the SplitBall Powerup
        while (true)
        {
            yield return new WaitForSeconds(5f);
            
            //TODO: Change range to 0,3 once implemented splitball
            int randomPowerUp = Random.Range(0, 2);
            switch (randomPowerUp)
            {
                case 0:
                    powerUpInventory.Add(new ExpandPaddle());
                    break;
                case 1:
                    powerUpInventory.Add(new LightningBall());
                    break;
                    //case 2:
                    //    powerUpInventory.Add(new FasterBallPowerUp());
                    //    break;
            }
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

}
