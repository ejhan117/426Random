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
        Ball ballInstance = GameObject.FindObjectOfType<Ball>();  // Find the Ball instance in the scene
        if (ballInstance != null)
        {
            ballInstance.speed *= speedMultiplier;
        }
    }

    public override void Deactivate(Player player)
    {
        Ball ballInstance = GameObject.FindObjectOfType<Ball>();  // Find the Ball instance in the scene
        if (ballInstance != null)
        {
            ballInstance.speed /= speedMultiplier;
        }
    }
}
