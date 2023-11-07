using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallTrapPowerUp : PowerUp
{


    public override void Activate(Player player)
    {
        GameObject wallTrapPrefab = Resources.Load<GameObject>("Prefabs/WallTrap");
        GameObject wallTrap = GameObject.Instantiate(wallTrapPrefab);
        if (player.playerNum == Player.PlayerNum.Player1) {
            wallTrap.transform.position = new Vector3(1, 0, 0);
        }
        else
        {
            wallTrap.transform.position = new Vector3(-1, 0, 0);
        }
    }


    public override void Deactivate(Player player)
    {
        
    }
  
}
