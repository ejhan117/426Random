using UnityEngine;

public class MagnetPaddle : PowerUp
{
    public MagnetPaddle() : base("Magnet Paddle", 7.5f)
    {

    }

    public override void Activate(Player player)
    {
        player.isMagnetActive = true;
    }

    public override void Deactivate(Player player)
    {
        player.isMagnetActive = false;
    }
}
