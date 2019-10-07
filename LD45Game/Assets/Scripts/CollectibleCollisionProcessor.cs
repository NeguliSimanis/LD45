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

    [SerializeField]
    Sprite anchorFound;
    [SerializeField]
    Sprite compassFound;
    [SerializeField]
    Sprite rudderFound;

    [SerializeField]
    Image anchorImage;
    [SerializeField]
    Image compassImage;
    [SerializeField]
    Image rudderImage;

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

        if (collision.gameObject.tag == "LevelEnd")
        {
            Debug.Log("END LEVEL");
            GameManager.instance.EnterNextLevel();
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
                    rudderImage.sprite = rudderFound;
                    break;

                case ItemType.compass:
                    front.SetActive(true);
                    Debug.Log("picked up 1");
                    GameManager.instance.compassFound = true;
                    compassImage.sprite = compassFound;
                    break;

                case ItemType.anchor:
                    anchor.SetActive(true);
                    Debug.Log("picked up 1");   
                    GameManager.instance.anchorFound = true;
                    anchorImage.sprite = anchorFound;
                    break;
            }
        }

    }
}
