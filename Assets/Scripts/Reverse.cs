using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverse : PowerUp
{
    public Reverse() : base("Reverse", 0.0f)
    {

    }
    public override void Activate(Player player)
    {
        // get all balls 
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in ballObjects)
        {
            ball.GetComponent<Ball>().ReverseDirection();
        }

    }

    public override void Deactivate(Player player)
    {

    }
}
