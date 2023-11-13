using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPaddle : PowerUp
{
    // Start is called before the first frame update

    public SlowPaddle() : base("Slow Paddle", 5.0f)
    {

    }
    public override void Activate(Player player)
    {
        GameObject snail = player.otherPlayer.scorePanelIcon;
        player.PlaySoundEffect(powerName);
        snail.SetActive(true);
        player.otherPlayer.speed /= 2f;

    }

    public override void Deactivate(Player player)
    {
        GameObject snail = player.otherPlayer.scorePanelIcon;
        snail.SetActive(false);
        player.otherPlayer.speed *= 2f;
    }
}
