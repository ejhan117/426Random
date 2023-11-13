using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPaddle : PowerUp
{
  private float sizeDecrease = 2.0f; // or whatever value you want

    public ShrinkPaddle() : base("Shrink Paddle", 5.0f)
    {

    }

  public override void Activate(Player player)
  {
      player.otherPlayer.transform.localScale = new Vector3(player.otherPlayer.transform.localScale.x, player.otherPlayer.transform.localScale.y / sizeDecrease, player.otherPlayer.transform.localScale.z);
      player.otherPlayer.SizeDecrease();
  }

  public override void Deactivate(Player player)
  {
    player.otherPlayer.transform.localScale = new Vector3(player.otherPlayer.transform.localScale.x, player.otherPlayer.transform.localScale.y * sizeDecrease, player.otherPlayer.transform.localScale.z);
  }
}
