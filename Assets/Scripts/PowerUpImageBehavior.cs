using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpImageBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    Image sprite;
    Color originalColor;
    
    void Start()
    {
        sprite = GetComponent<Image>();
        originalColor = sprite.color;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Here");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var tempColor = originalColor;
        tempColor.a = .2f;
        sprite.color = tempColor;
        Debug.Log("Triggered");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        sprite.color = originalColor;
    }
}
