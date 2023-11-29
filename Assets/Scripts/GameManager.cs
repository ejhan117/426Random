using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player pOne;
    public Player pTwo;
    public TMP_Text gameOver;
    public int maxScore = 10;

    public GameObject horizontalWallPrefab;

    public GameObject shieldPrefab;

    public Image fadeImage; // UI Image to overlay
    public float fadeSpeed = 0.8f; // Speed of the fade

    private float safeDistance = 1.5f;
    float gameAreaHeight;
    // Start is called before the first frame update
    void Start()
    {
        gameOver.enabled = false;
        gameAreaHeight = 2f * Camera.main.orthographicSize;
        Debug.Log(gameAreaHeight);
    }

    // Update is called once per frame
    void Update()
    {
        // Restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reloads the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Exit the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                 Application.Quit();
            #endif
        }

        //if(pOne.score >= maxScore)
        //{
        //    GameOver(1);
        //}
        //else if (pTwo.score >= maxScore)
        //{
        //    GameOver(2);
        //}
        //TODO: Make a better end condition than just score since it gets reached so easily?
    }

    public void spawnHorizontalWall(int playerNo)
    {
        GameObject wall;
        Vector3 spawnPos = CalculateSpawnPosition(playerNo);
        if (playerNo == 0)
        {
            wall = Instantiate(horizontalWallPrefab, spawnPos, Quaternion.identity);
            wall.name = "HorizontalWallP1";
        }
        else
        {
            wall = Instantiate(horizontalWallPrefab, spawnPos, Quaternion.identity);
            wall.name = "HorizontalWallP2";
        }
    }

    public void deleteHorizontalWall(int playerNo)
    {
        if (playerNo == 0)
        {
            Destroy(GameObject.Find("HorizontalWallP1"));
        }
        else
        {
            Destroy(GameObject.Find("HorizontalWallP2"));
        }
    }

    public void spawnShield(int playerNo)
    {
        GameObject shield;
        if (playerNo == 0)
        {
            shield = Instantiate(shieldPrefab, new Vector3(-9, 0, -1), Quaternion.identity);
            shield.name = "Shield1";
        }
        else
        {
            shield = Instantiate(shieldPrefab, new Vector3(9, 0, -1), Quaternion.identity);
            shield.name = "Shield2";
        }
    }

    public void deleteShield(int playerNo)
    {
        if (playerNo == 0)
        {
            Destroy(GameObject.Find("Shield1"));
        }
        else
        {
            Destroy(GameObject.Find("Shield2"));
        }
    }


    public void GameOver(int winnerNum)
    {
        //Show Game Over Text, w/ automatic restart after 3 seconds
        gameOver.text = "Game Over! Player " + winnerNum + " wins!\n";
        gameOver.enabled = true;
        Time.timeScale = 0;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(ReloadSceneAfterDelay(5));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        // Wait for 'delay' seconds
        yield return new WaitForSecondsRealtime(delay);

        // Reload the current scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeAndRestart()
    {
        // Fade to black
        for (float i = 0; i <= 1; i += Time.deltaTime * fadeSpeed)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    Vector3 CalculateSpawnPosition(int playerNo)
    {
        Player other;
        if(playerNo == 0)
        {
            other = pTwo;
        }
        else
        {
            other = pOne;
        }
        float playerSize = other.GetComponent<Renderer>().bounds.size.y; // Get the player's size
        Vector3 playerPosition = other.transform.position;

        // Define the upper and lower bounds of the game area
        float lowerBound = -gameAreaHeight / 2;
        float upperBound = gameAreaHeight / 2;

        // Calculate safe zones above and below the player
        float safeZoneLower = playerPosition.y - playerSize / 2 - safeDistance;
        float safeZoneUpper = playerPosition.y + playerSize / 2 + safeDistance;

        // Ensure the safe zones do not exceed the game area bounds
        safeZoneLower = Mathf.Max(safeZoneLower, lowerBound);
        safeZoneUpper = Mathf.Min(safeZoneUpper, upperBound);

        // Check if there is enough space to spawn the wall
        if (safeZoneUpper - safeZoneLower <= 0)
        {
            // Not enough space to spawn the wall safely
            return Vector3.zero;
        }

        // Randomize spawn position outside the player's area
        float spawnY;
        if (playerPosition.y >= 0f) // Randomly choose above or below the player
        {
            spawnY = safeZoneLower;
        }
        else
        {
            spawnY = safeZoneUpper;
        }

        return new Vector3(playerPosition.x, spawnY, playerPosition.z); // Assuming a 2D game on the XY plane
    }
}

