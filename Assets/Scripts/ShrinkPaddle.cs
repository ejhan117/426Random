using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPaddle : PowerUp
{
    private float sizeDecrease = 0.5f; 
    private bool dontShrink = false;

    public ShrinkPaddle() : base("Shrink", 5.0f)
    {

    }

  public override void Activate(Player player)
  {
        if(player.otherPlayer.transform.localScale.y <= 0.5f)
        {
            dontShrink = true;
            return;
        }
        player.otherPlayer.transform.localScale = new Vector3(player.otherPlayer.transform.localScale.x, player.otherPlayer.transform.localScale.y - sizeDecrease, player.otherPlayer.transform.localScale.z);
        //player.otherPlayer.SizeDecrease();
  }

  public override void Deactivate(Player player)
  {
        if (dontShrink)
        {
            return;
        }
        player.otherPlayer.transform.localScale = new Vector3(player.otherPlayer.transform.localScale.x, player.otherPlayer.transform.localScale.y + sizeDecrease, player.otherPlayer.transform.localScale.z);
  }
}
