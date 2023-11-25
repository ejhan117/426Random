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

    public Image fadeImage; // UI Image to overlay
    public float fadeSpeed = 0.8f; // Speed of the fade
    // Start is called before the first frame update
    void Start()
    {
        gameOver.enabled = false;
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
        if (playerNo == 0)
        {
            wall = Instantiate(horizontalWallPrefab, new Vector3(7, 0, 0), Quaternion.identity);
            wall.name = "HorizontalWallP1";
        }
        else
        {
            wall = Instantiate(horizontalWallPrefab, new Vector3(-7, 0, 0), Quaternion.identity);
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
}
