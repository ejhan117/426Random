using UnityEngine;

public class CurveBall : PowerUp
{
    public CurveBall() : base("Curve Ball", 7.5f)
    {

    }

    public override void Activate(Player player)
    {
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.StartCurveEffect();
            }
        }
    }

    public override void Deactivate(Player player)
    {
        // Find all balls in the scene
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in balls)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.StopCurveEffect();
            }
        }
    }
}
