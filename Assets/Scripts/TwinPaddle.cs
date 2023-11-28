using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TwinPaddle : PowerUp
{
    public TwinPaddle() : base("Twin Paddle", 10.0f)
    {

    }
    public override void Activate(Player player)
    {
        Player clone = UnityEngine.Object.Instantiate(player, new Vector2(player.transform.position.x, player.transform.position.y - 5.0f), Quaternion.identity);
        player.clone = clone;
        clone.isClone = true;
    }

    public override void Deactivate(Player player)
    {
        UnityEngine.Object.Destroy(player.clone);
        player.clone = null;
    }

}
