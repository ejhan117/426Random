using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiBall : PowerUp
{
    List<GameObject> balls = new List<GameObject>();

    Coroutine runningTimer;

    public InvisiBall() : base("InvisiBall", 5f)
    { 

    }
    public override void Activate(Player player)
    {
        // get all balls 
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in ballObjects)
        {
            ball.GetComponent<Ball>().setIsInvisiBall(true);
        }

    }

    public override void Deactivate(Player player)
    {
        // get all balls
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<Ball>().setIsInvisiBall(false);
        }
    }


}


