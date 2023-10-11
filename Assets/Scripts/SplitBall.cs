using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBall : PowerUp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(Player player)
    {
        player.readySplit = true;
    }

    public override void Deactivate(Player player)
    {
        player.readySplit = false;
    }
}
