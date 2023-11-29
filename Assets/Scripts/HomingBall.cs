using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBall : PowerUp
{
    public HomingBall(): base("Magnet", 5f)
    {

    }
    override public void Activate(Player player)
    {
        player.isMagnetActive = true;
        List<GameObject> balls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
        foreach (GameObject b in balls)
        {
            b.GetComponent<Ball>().startHoming(player);
        }
    }

    override public void Deactivate(Player player)
    {
        player.isMagnetActive = false;
        List<GameObject> balls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));

        foreach (GameObject b in balls)
        {
            b.GetComponent<Ball>().stopHoming();
        }
    }
}
