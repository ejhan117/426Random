using UnityEngine;

public class TimeWarp : PowerUp
{
    // Keep track of the number of active time warps
    private static int activeTimeWarps = 0;
    private float slowTimeScale = 0.5f; // Half the normal speed

    public TimeWarp() : base("Time Warp", 3.0f) // 10 seconds duration
    {

    }

    public override void Activate(Player player)
    {
        activeTimeWarps++;
        if (activeTimeWarps == 1) // Only slow down time once
        {
            // Slow down everything
            Time.timeScale = slowTimeScale;
        }
    }

    public override void Deactivate(Player player)
    {
        activeTimeWarps--;
        if (activeTimeWarps == 0) // Only reset time when the last warp ends
        {
            // Revert time scale
            Time.timeScale = 1.0f;
        }
    }
}
