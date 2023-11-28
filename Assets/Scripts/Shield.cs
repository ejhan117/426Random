using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PowerUp
{
    GameObject horizontalWallPrefab;
    GameManager gm;
    // Start is called before the first frame update
    public Shield() : base("Shield", 10.0f)
    {

    }

    public override void Activate(Player player)
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.spawnShield((int)player.playerNum);

    }

    public override void Deactivate(Player player)
    {
        gm.deleteShield((int)player.playerNum);
    }
}
