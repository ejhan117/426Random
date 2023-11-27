using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPaddle : PowerUp
{
    // Start is called before the first frame update
    public GhostPaddle() : base("Ghost Paddle", 5.0f)
    {

    }
    public override void Activate(Player player)
    {
        player.otherPlayer.GetComponent<Renderer>().enabled = false;
        player.otherPlayer.transform.GetChild(0).gameObject.SetActive(false);

    }

    public override void Deactivate(Player player)
    {
        player.otherPlayer.GetComponent<Renderer>().enabled = true;
        player.otherPlayer.transform.GetChild(0).gameObject.SetActive(false);

    }
}
