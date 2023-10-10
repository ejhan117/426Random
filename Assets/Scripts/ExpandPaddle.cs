using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandPaddle : PowerUp
{
    private float originalSize;
    private float increasedSize = 3.0f; // or whatever value you want

    public override void Activate(Player player)
    {
        originalSize = player.transform.localScale.y;
        player.transform.localScale = new Vector3(player.transform.localScale.x, increasedSize, player.transform.localScale.z);
    }

    public override void Deactivate(Player player)
    {
        player.transform.localScale = new Vector3(player.transform.localScale.x, originalSize, player.transform.localScale.z);
    }
}
