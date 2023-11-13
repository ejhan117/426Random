using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPaddle : PowerUp
{
    // Start is called before the first frame update
    public GhostPaddle() : base("Ghost Paddle", 10.0f)
    {

    }
    public override void Activate(Player player)
    {
        player.otherPlayer.GetComponent<Renderer>().enabled = false;

    }

    public override void Deactivate(Player player)
    {
        player.otherPlayer.GetComponent<Renderer>().enabled = true;
    }
}
