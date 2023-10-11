using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player pOne;
    public Player pTwo;
    public TMP_Text gameOver;
    public int maxScore = 10;
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

        if(pOne.score >= maxScore)
        {
            GameOver(1);
        }
        else if (pTwo.score >= maxScore)
        {
            GameOver(2);
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
}
