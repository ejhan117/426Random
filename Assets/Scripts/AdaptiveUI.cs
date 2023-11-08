using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdaptiveUI : MonoBehaviour
{
    public GameObject uiPanelOne;
    public GameObject uiPanelTwo;
    public GameObject ball;
    public float proximityThreshold = 20f;  // Set this to a value that makes sense for your game

    void Update()
    {
        CanvasGroup canvasGroupOne = uiPanelOne.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroupTwo = uiPanelTwo.GetComponent<CanvasGroup>();

        if (IsBallNearUIOne())
        {
            Debug.Log("Near UI One");
            canvasGroupOne.alpha = 0.5f;
        }
        else
        {
            canvasGroupOne.alpha = 1.0f;
        }

        if (IsBallNearUITwo())
        {
            Debug.Log("Near UI Two");
            canvasGroupTwo.alpha = 0.5f;
        }
        else
        {
            canvasGroupTwo.alpha = 1.0f;
        }
    }

    bool IsBallNearUIOne()
    {
        Vector3 ballScreenPos;
        Vector3 panelScreenPos;
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");  // Assumes that all balls have the tag "Ball"

        foreach (GameObject ball in balls)
        {
            ballScreenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
            panelScreenPos = Camera.main.WorldToScreenPoint(uiPanelOne.transform.position);

            float distance = Vector3.Distance(ballScreenPos, panelScreenPos);
            Debug.Log(distance);

            if (distance < proximityThreshold)
            {
                return true;
            }
        }

        return false;
        //Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
        //Vector3 panelScreenPos = Camera.main.WorldToScreenPoint(uiPanelOne.transform.position);

        //float distance = Vector3.Distance(ballScreenPos, panelScreenPos);

        //return distance < proximityThreshold;
    }

    bool IsBallNearUITwo()
    {
        Vector3 ballScreenPos;
        Vector3 panelScreenPos;
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");  // Assumes that all balls have the tag "Ball"

        foreach (GameObject ball in balls)
        {
            ballScreenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
            panelScreenPos = Camera.main.WorldToScreenPoint(uiPanelTwo.transform.position);

            float distance = Vector3.Distance(ballScreenPos, panelScreenPos);

            if (distance < proximityThreshold)
            {
                return true;
            }
        }

        return false;
        //Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
        //Vector3 panelScreenPos = Camera.main.WorldToScreenPoint(uiPanelOne.transform.position);

        //float distance = Vector3.Distance(ballScreenPos, panelScreenPos);

        //return distance < proximityThreshold;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

}
