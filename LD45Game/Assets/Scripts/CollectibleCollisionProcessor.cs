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
            if (GameManager.instance.isGamePaused)
                return;
            soundEating.Play();
            Item mushroom = collision.gameObject.GetComponent<Item>();
            mushroom.GetPickedUpByPlayer();
        }

        if (collision.gameObject.tag == "Shipwreck")
        {
            if (GameManager.instance.isGamePaused)
                return;
            Debug.Log("he up 1");
            soundCollectingShipwrecks.Play();
            Destroy(collision.gameObject);

            switch(collision.gameObject.GetComponent<Item>().type)
            {
                case ItemType.rudder:
                    Debug.Log("picked up 1");
                    mast.SetActive(true);
                    GameManager.instance.rudderFound = true;
                    break;

                case ItemType.compass:
                    front.SetActive(true);
                    Debug.Log("picked up 1");
                    GameManager.instance.compassFound = true;
                    break;

                case ItemType.anchor:
                    anchor.SetActive(true);
                    Debug.Log("picked up 1");   
                    GameManager.instance.anchorFound = true;
                    break;
            }
        }

    }
}
