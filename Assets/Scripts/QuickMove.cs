using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMove : PowerUp
{
    private float speedIncrease = 7.0f;

    public QuickMove() : base("Quick Move", 10.0f)
    {

    }

    public override void Activate(Player player)
    {
        player.speed += speedIncrease;

        // Get the TrailRenderer component from the player and enable it
        TrailRenderer playerTrail = player.GetComponentInChildren<TrailRenderer>();
        if (playerTrail != null)
        {
            playerTrail.enabled = true;
        }
    }

    public override void Deactivate(Player player)
    {
        player.speed -= speedIncrease;

        // Get the TrailRenderer component from the player and disable it
        TrailRenderer playerTrail = player.GetComponentInChildren<TrailRenderer>();
        if (playerTrail != null)
        {
            playerTrail.enabled = false;
        }
    }
}
