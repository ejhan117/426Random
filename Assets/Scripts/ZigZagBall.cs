using UnityEngine;

public class ZigZagBall : PowerUp
{
    public ZigZagBall() : base("ZigZag Ball", 7.5f) // Adjust duration as needed
    {

    }

    public override void Activate(Player player)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.StartZigZagEffect();
            }
        }
    }

    public override void Deactivate(Player player)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.StopZigZagEffect();
            }
        }
    }
}
