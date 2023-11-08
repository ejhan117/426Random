using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : PowerUp
{
    public float speedMultiplier = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        powerName = "Wormhole";
        duration = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate(Player player)
    {
        player.wormholeMode = true;
    }

    public override void Deactivate(Player player)
    {
        player.wormholeMode = false;
    }
}
