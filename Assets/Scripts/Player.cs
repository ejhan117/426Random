using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// The Player class, derived from the Paddle class
public class Player : Paddle
{
    // Enumeration for player numbers
    public enum PlayerNum { Player1, Player2 };

    // Public variables accessible from other scripts or in the Unity Inspector
    public PlayerNum playerNum;
    public Vector2 direction { get; private set; }
    public int score = 0;
    public TMP_Text scoreText;

    // List to store PowerUp objects in the player's inventory
    public List<PowerUp> powerUpInventory = new List<PowerUp>();
    public int splitBallCount = 0;

    public bool readySplit = false;
    bool isControlsEnabled;
    public Player otherPlayer;

    // Private variables used within the class
    private int numSizeIncreases = 0;
    private const int numBins = 3; // Number of bins for power-ups
    public PowerUp[] powerUpBins = new PowerUp[numBins]; // Array to store power-ups in bins

    // Arrays to store UI elements for power-ups
    public Image[] powerUpImages = new Image[numBins];
    public TMP_Text[] powerUpNames = new TMP_Text[numBins];

    // List of available power-up types
    private List<System.Type> availablePowerUps = new List<System.Type>
    {
        typeof(ExpandPaddle),
        typeof(LightningBall),
        typeof(SplitBall),
        typeof(ShrinkPaddle),
        typeof(SlowPaddle),
        typeof(GhostPaddle),
        //Add More Powerups Here
        typeof(TimeWarpPowerup),
        typeof(ReverseControlPowerup)
    };

    private int numSizeDecreases = 0;
    public GameObject timeWarpPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        // Coroutine to generate random power-ups over time
        StartCoroutine(GenerateRandomPowerUp());
        // Update the player's score on the scoreboard
        UpdateScoreboard();

        // Find and set the score text object based on the player number
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

        // Update the UI for power-ups
        UpdatePowerUpUI();
    }

    // Update is called once per frame
    private void Update()
    {
        // Input handling for player movement
        switch (playerNum)
        {
            case PlayerNum.Player2:
                // Keyboard controls for Player2
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

                // Controller joystick controls for Player2
                float vertical = Input.GetAxis("Vertical");
                if (Mathf.Abs(vertical) > 0.1f)  // Deadzone
                {
                    direction = new Vector2(0, vertical).normalized;
                }
                break;

            case PlayerNum.Player1:
                // Keyboard controls for Player1
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

        // Input handling for activating power-ups
        if (playerNum == PlayerNum.Player1)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ActivatePowerUpFromBin(0);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ActivatePowerUpFromBin(1);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ActivatePowerUpFromBin(2);
            }
        }

        if (playerNum == PlayerNum.Player2)
        {
            if (Input.GetKeyDown(KeyCode.Comma) || Input.GetButtonDown("Fire1"))
            {
                ActivatePowerUpFromBin(0);
            }

            if (Input.GetKeyDown(KeyCode.Period) || Input.GetButtonDown("Fire2"))
            {
                ActivatePowerUpFromBin(1);
            }

            if (Input.GetKeyDown(KeyCode.Slash) || Input.GetButtonDown("Fire3"))
            {
                ActivatePowerUpFromBin(2);
            }
        }
    }

    // FixedUpdate is called at a fixed interval and is often used for physics calculations
    private void FixedUpdate()
    {
        // Move the paddle based on the input direction
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

    // Increase the player's score and update the scoreboard
    public void Score()
    {
        score++;
        UpdateScoreboard();
    }

    // Update the displayed score on the scoreboard UI
    private void UpdateScoreboard()
    {
        scoreText.text = score.ToString();
    }

    // Coroutine to generate random power-ups at intervals
    private IEnumerator GenerateRandomPowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            AddPowerUp();
            UpdatePowerUpUI();
        }
    }

    private void InstantiateTimeWarpPowerUp()
    {
        // Instantiate the TimeWarp power-up prefab at a random position
        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f), 0f);
        Instantiate(timeWarpPrefab, randomPosition, Quaternion.identity);
    }
    public void DisableControls()
    {
        // Disable player controls here (e.g., set a flag)
        isControlsEnabled = false;
    }
    
    public void EnableControls()
    {
        // Enable player controls here (e.g., reset the flag)
        isControlsEnabled = true;
    }


    // Coroutine to deactivate a power-up after its duration
    private IEnumerator DeactivatePowerUpAfterDuration(PowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.duration);
        powerUp.Deactivate(this);

        // Decrease the size increase count specifically for ExpandPaddle power-up
        if (powerUp is ExpandPaddle)
        {
            numSizeIncreases--;
        }
    }

    // Update the UI for all power-up bins
    public void UpdatePowerUpUI()
    {
        for (int i = 0; i < numBins; i++)
        {
            UpdatePowerUpUIForBin(i);
        }
    }

    // Add a randomly selected power-up to the player's inventory
    public void AddPowerUp()
    {
        int randomIndex = UnityEngine.Random.Range(0, availablePowerUps.Count);
        System.Type selectedType = availablePowerUps[randomIndex];

        // Use reflection to create an instance of the selected powerup
        PowerUp newPower = (PowerUp)Activator.CreateInstance(selectedType);

        // Add the power-up to the first available bin
        for (int i = 0; i < numBins; i++)
        {
            if (powerUpBins[i] == null)
            {
                powerUpBins[i] = newPower;
                break;
            }
        }
    }

    // Increase the count of the SplitBall power-up
    public void AddSplitBallPowerUp()
    {
        splitBallCount++;
    }

    // Reset the SplitBall count and change the paddle color after using the power-up
    public void UseSplitBallPowerUp()
    {
        splitBallCount = 0;
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.material.color = Color.white;
        }
    }

    // Increase the count of size increases (used for ExpandPaddle power-up)
    public void SizeIncrease()
    {
        numSizeIncreases++;
    }

    // Decrease the count of size decreases (used for ShrinkPaddle power-up)
    public void SizeDecrease()
    {
        numSizeDecreases++;
    }

    // Activate a power-up from a specific bin
    public void ActivatePowerUpFromBin(int binIndex)
    {
        // Check for valid bin index and non-empty bin
        if (binIndex < 0 || binIndex >= numBins || powerUpBins[binIndex] == null)
        {
            Debug.Log("Invalid bin index or empty bin.");
            return;
        }

        // Activate the power-up, start the deactivation coroutine, and clear the bin
        PowerUp powerUpToActivate = powerUpBins[binIndex];
        powerUpToActivate.Activate(this);

        if (powerUpToActivate.duration > 0)
        {
            StartCoroutine(DeactivatePowerUpAfterDuration(powerUpToActivate));
        }

        powerUpBins[binIndex] = null;

        // Update the UI to reflect the changes
        UpdatePowerUpUI();
    }

    // Update the UI for a specific power-up bin
    private void UpdatePowerUpUIForBin(int binIndex)
    {
        if (powerUpBins[binIndex] != null)
        {
            // Display the power-up name and set the image color to green
            string powerName = powerUpBins[binIndex].powerName;
            Debug.Log(powerName);
            powerUpNames[binIndex].text = powerName;
            powerUpImages[binIndex].color = Color.green;
            // Set the image sprite based on powerUp type if you have different sprites
        }
        else
        {
            // Clear the power-up name and set the image color to grey for an empty bin
            powerUpNames[binIndex].text = "";
            powerUpImages[binIndex].color = Color.grey;
        }
    }
}

