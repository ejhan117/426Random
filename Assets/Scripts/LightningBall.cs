using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : PowerUp
{
    public float speedMultiplier = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        powerName = "Lightning Ball";
        duration = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate(Player player)
    {
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Apply the speed power-up to each ball
        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.speed *= speedMultiplier;
            }
            ballScript.UpdateSpeed();
        }
        //Ball ballInstance = GameObject.FindObjectOfType<Ball>();  // Find the Ball instance in the scene
        //if (ballInstance != null)
        //{
        //    ballInstance.speed *= speedMultiplier;
        //}
    }

    public override void Deactivate(Player player)
    {
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Apply the speed power-up to each ball
        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.speed = 9.0f;
            }
            ballScript.UpdateSpeed();
        }
    }
}
