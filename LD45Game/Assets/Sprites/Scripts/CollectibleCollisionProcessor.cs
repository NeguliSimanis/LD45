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

    PlayerController playerController;


    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "LevelEnd")
        {
            Debug.Log("END LEVEL");
            GameManager.instance.EnterNextLevel();
        }

        if (collision.gameObject.tag == "Shipwreck")
        {
            if (GameManager.instance.isGamePaused)
                return;
            soundCollectingShipwrecks.Play();
            Destroy(collision.gameObject);

            switch(collision.gameObject.GetComponent<Item>().type)
            {
                case ItemType.rudder:
                    mast.SetActive(true);
                    GameManager.instance.rudderFound = true;
                    rudderImage.sprite = rudderFound;
                    break;

                case ItemType.compass:
                    front.SetActive(true);
                    GameManager.instance.compassFound = true;
                    compassImage.sprite = compassFound;
                    break;

                case ItemType.anchor:
                    anchor.SetActive(true);  
                    GameManager.instance.anchorFound = true;
                    anchorImage.sprite = anchorFound;
                    break;
            }
        }

    }

    public void PickupMushroom(Item mushroom)
    {
        if (mushroom.isSelectedByPlayer)// && playerController.hasClickedOnShroom)
        {
            soundEating.Play();
            mushroom.GetPickedUpByPlayer();
        }
    }
}
