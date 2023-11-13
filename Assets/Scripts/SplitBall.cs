using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBall : PowerUp
{
    // Start is called before the first frame update
    public SplitBall() : base("Split Ball", 10.0f)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(Player player)
    {
        player.readySplit = true;
        Renderer r = player.GetComponent<Renderer>();
        if(r != null) 
        {
            r.material.color = Color.red;
        }
    }

    public override void Deactivate(Player player)
    {
        player.readySplit = false;
    }
}
