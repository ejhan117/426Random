using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpeed : PowerUp
{
    public float speedMultiplier = 3.5f;

    public SuperSpeed() : base("Super Speed", 10.0f)
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
            ballScript.GetComponent<SpriteRenderer>().material.color = Color.blue;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );

            // Apply the gradient to the trail renderer
            ballScript.GetComponentInChildren<TrailRenderer>().colorGradient = gradient;
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
                if (ballScript.speed <= 0.0f)
                {
                    ballScript.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                    Gradient grad = new Gradient();
                    grad.SetKeys(
                        new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                        new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                    );

                    // Apply the gradient to the trail renderer
                    ballScript.GetComponentInChildren<TrailRenderer>().colorGradient = grad; 
                    return;
                }
                ballScript.speed /= speedMultiplier;
                if (ballScript.speed < 9.0f)
                {
                    ballScript.speed = 9.0f;
                }
            }
            ballScript.GetComponent<SpriteRenderer>().material.color = Color.yellow;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );

            // Apply the gradient to the trail renderer
            ballScript.GetComponentInChildren<TrailRenderer>().colorGradient = gradient; 
            ballScript.UpdateSpeed();
        }
    }
}
