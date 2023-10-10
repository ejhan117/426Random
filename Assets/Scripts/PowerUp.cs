using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp
{
    public string powerName;
    public float duration = 5.0f;
    public abstract void Activate(Player player);
    public abstract void Deactivate(Player player);
}
