using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject scoreCanvas;

    private void Start()
    {
        Pause();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)){
            if (isPaused){
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    void Resume (){
      pauseMenuUI.SetActive(false);
      scoreCanvas.SetActive(true);
      Time.timeScale = 1f;
      isPaused = false;
    }

    void Pause (){
      scoreCanvas.SetActive(false);
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
      isPaused = true;
    }
}
