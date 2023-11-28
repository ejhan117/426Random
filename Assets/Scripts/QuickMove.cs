using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMove : PowerUp
{
    private float speedIncrease = 7.0f;
    public QuickMove() : base("Quick Move", 10.0f)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate(Player player)
    {
        player.speed += speedIncrease;
    }

    public override void Deactivate(Player player)
    {
        player.speed -= speedIncrease;
    }
}
