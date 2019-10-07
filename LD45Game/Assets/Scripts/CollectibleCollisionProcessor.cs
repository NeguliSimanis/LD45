using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleCollisionProcessor : MonoBehaviour
{
    public AudioSource soundEating;
    public AudioSource soundCollectingShipwrecks;
    public GameObject mast;
    public GameObject front;
    public GameObject anchor;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Mushroom")
        {
            soundEating.Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Shipwreck")
        {
            soundCollectingShipwrecks.Play();
            Destroy(collision.gameObject);

            switch(collision.gameObject.name)
            {
                case "Mast":
                    mast.SetActive(true);
                    GameManager.instance.rudderFound = true;
                    break;

                case "Front":
                    front.SetActive(true);
                    GameManager.instance.compassFound = true;
                    break;

                case "Anchor":
                    anchor.SetActive(true);
                    GameManager.instance.anchorFound = true;
                    break;
            }
        }

    }
}
