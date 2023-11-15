using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWallTrap : PowerUp
{
    GameObject horizontalWallPrefab;
    GameManager gm;
    // Start is called before the first frame update
    public HorizontalWallTrap() : base("Wall Trap", 10.0f)
    {

    }
    
    public override void Activate(Player player)
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.spawnHorizontalWall((int) player.playerNum);

    }

    public override void Deactivate(Player player)
    {
        gm.deleteHorizontalWall((int) player.playerNum);
    }
}