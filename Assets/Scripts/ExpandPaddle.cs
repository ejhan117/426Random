using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandPaddle : PowerUp
{
    private float sizeIncrease = 2.0f; // or whatever value you want

    public override void Activate(Player player)
    {
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y + sizeIncrease, player.transform.localScale.z);
    }

    public override void Deactivate(Player player)
    {
        player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y - sizeIncrease, player.transform.localScale.z);
    }
}
