using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleCollisionProcessor : MonoBehaviour
{

    public int AffectSanityLevel;
    public int AffectHungerLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(gameObject.tag == "Mushroom")
        {
            GameManager.instance.AddToSanityLevel(AffectSanityLevel);
            GameManager.instance.AddToHungerLevel(AffectHungerLevel);
            Destroy(gameObject);
        }

        else if (gameObject.tag == "Road")
        {
           Debug.Log("Hit the Road jack");
        }
    }
}
