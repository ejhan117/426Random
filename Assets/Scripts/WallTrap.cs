using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WallTrap : Paddle
{
    int hitsReceived = 0;

    SpriteRenderer sR;

    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hitsReceived++;
            if (hitsReceived == 1)
            {
                sR.color = Color.red;
            }
            if (hitsReceived == 2)
            {
                Destroy(gameObject);
            }
        }
    }
}

