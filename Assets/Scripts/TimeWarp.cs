using UnityEngine;

public class TimeWarp : PowerUp
{
    private float originalTimeScale = 1.0f;
    private float slowTimeScale = 0.5f; // Half the normal speed
    private float originalPlayerSpeed;
    private float increasedPlayerSpeedFactor = 3.0f; // Factor to increase the player's speed

    public TimeWarp() : base("Time Warp", 3.0f) // 10 seconds duration
    {

    }

    public override void Activate(Player player)
    {
        // Slow down time
        originalTimeScale = Time.timeScale;
        Time.timeScale = slowTimeScale;

        // Increase the player's paddle speed to compensate
        originalPlayerSpeed = player.speed;
        player.speed *= increasedPlayerSpeedFactor;
    }

    public override void Deactivate(Player player)
    {
        // Revert time scale and player's speed
        Time.timeScale = originalTimeScale;
        player.speed = originalPlayerSpeed;
    }
}
