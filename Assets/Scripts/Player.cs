using System;
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
    public int splitBallCount = 0;

    public bool readySplit = false;
    public Player otherPlayer;

    //Sound Map - TO BE IMPLEMENTED
    // public Dictionary<string, AudioSource> soundEffects;
    public AudioSource soundEffectSnail;

    private int numSizeIncreases = 0;
    private const int numBins = 3;
    public PowerUp[] powerUpBins = new PowerUp[numBins];

    // public Image[] powerUpImages = new Image[numBins];
    public TMP_Text[] powerUpNames = new TMP_Text[numBins];

    public bool isMagnetActive = false;

    private List<System.Type> availablePowerUps = new List<System.Type>
    {
        typeof(ExpandPaddle),
        typeof(LightningBall),
        typeof(SplitBall),
        typeof(ShrinkPaddle),
        typeof(SlowPaddle),
        typeof(GhostPaddle),
        typeof(CurveBall),
        typeof(TimeWarp),
        typeof(ZigZagBall),
        typeof(HomingBall),
        typeof(InvisiBall),
        typeof(HorizontalWallTrap),
        //Add More Powerups Here
    };
    private int numSizeDecreases = 0;
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
                ActivatePowerUpFromBin(0);
            }

            // Activate the first "Faster Ball" power-up in the inventory when '2' is pressed
            if (Input.GetKeyDown(KeyCode.O))
            {
                ActivatePowerUpFromBin(1);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ActivatePowerUpFromBin(2);
            }

        }

        if(playerNum == PlayerNum.Player2)
        {
            if (Input.GetKeyDown(KeyCode.Comma) || Input.GetButtonDown("Fire1"))
            {
                ActivatePowerUpFromBin(0);
            }

            // Activate the first "Faster Ball" power-up in the inventory when '2' is pressed
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
        if(powerUp is ExpandPaddle)
        {
            numSizeIncreases--;
        }
    }
    public void UpdatePowerUpUI()
    {
        for(int i = 0; i < numBins; i++)
        {
            UpdatePowerUpUIForBin(i);
        }
    }

    public void AddPowerUp()
    {
        //int randomIndex = UnityEngine.Random.Range(0, availablePowerUps.Count);
        //System.Type selectedType = availablePowerUps[randomIndex];

        //// Use reflection to create an instance of the selected powerup
        //PowerUp newPower = (PowerUp)Activator.CreateInstance(selectedType);
        //for (int i = 0; i < numBins; i++)
        //{
        //    if (powerUpBins[i] == null)
        //    {
        //        powerUpBins[i] = newPower;
        //        break;
        //    }

        //}
        PowerUp newPower;
        bool isUnique;
        int randomIndex;
        System.Type selectedType;

        do
        {
            randomIndex = UnityEngine.Random.Range(0, availablePowerUps.Count);
            selectedType = availablePowerUps[randomIndex];

            newPower = (PowerUp)Activator.CreateInstance(selectedType);

            isUnique = !IsPowerUpTypeInBins(newPower.GetType());
        }
        while (!isUnique);

        for(int i = 0; i < numBins; i++)
        {
            if (powerUpBins[i] == null)
            {
                powerUpBins[i] = newPower;
                break;
            }
        }
    }
    public void AddSplitBallPowerUp()
    {
        splitBallCount++;
    }

    public void UseSplitBallPowerUp()
    {
        splitBallCount = 0; // Reset the count after using the power-up
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.material.color = Color.white;
        }
    }

    public void SizeIncrease()
    {
        numSizeIncreases++;
    }

    public void ActivatePowerUpFromBin(int binIndex)
    {
        if (binIndex < 0 || binIndex >= numBins || powerUpBins[binIndex] == null)
        {
            Debug.Log("Invalid bin index or empty bin.");
            return;
        }

        PowerUp powerUpToActivate = powerUpBins[binIndex];
        powerUpToActivate.Activate(this);
        if (powerUpToActivate.duration > 0)
        {
            StartCoroutine(DeactivatePowerUpAfterDuration(powerUpToActivate));
        }

        powerUpBins[binIndex] = null;

        UpdatePowerUpUI();
    }

    private void UpdatePowerUpUIForBin(int binIndex)
    {
        if (powerUpBins[binIndex] != null)
        {
            string powerName = powerUpBins[binIndex].powerName;
            Debug.Log(powerName);
            powerUpNames[binIndex].text = powerName;
            // powerUpImages[binIndex].color = Color.green;
            // Set the image sprite based on powerUp type if you have different sprites
        }
        else
        {
            powerUpNames[binIndex].text = "";
            // powerUpImages[binIndex].color = Color.grey;
        }
    }
    public void SizeDecrease()
    {
        numSizeDecreases++;
    }

    public void PlaySoundEffect(string powerName)
    {
      if(powerName == "Slow Paddle"){
        soundEffectSnail.Play();
      }
    }

    private bool IsPowerUpTypeInBins(System.Type powerUpType)
    {
        foreach (var bin in powerUpBins)
        {
            if (bin != null && bin.GetType() == powerUpType)
            {
                return true; // Found an existing instance of the same type
            }
        }
        return false; // No existing instance found
    }

}
