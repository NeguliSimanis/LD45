using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    AudioSource audioSource;

    [HideInInspector]
    public bool isSelectedByPlayer = false;
    [HideInInspector]
    public Vector2Int gridCoordinates;
    [HideInInspector]
    public Vector3 worldCoordinates;
    public Vector3 gridToWorldOffset;

    public int AffectSanityLevel;
    public int AffectHungerLevel;

    float chanceToPlayMushroomSFX = 0.4f;
    [SerializeField]
    SpriteRenderer highlightSprite;

    PlayerController playerController;

    /// <summary>
    /// eating sfx is played in the calling method
    /// </summary>
    public void GetPickedUpByPlayer()
    {
        if (GameManager.instance.isGamePaused)
            return;
        GameManager.instance.AddToSanityLevel(AffectSanityLevel);
        GameManager.instance.AddToSateLevel(AffectHungerLevel);

        GameManager.instance.occupiedTiles.Remove(gridCoordinates);
        PlayMushroomReaction();
        Destroy(gameObject);
    }

    private void PlayMushroomReaction()
    {
        if (Random.Range(0f, 1f) > chanceToPlayMushroomSFX)
            return;
        audioSource = GameManager.instance.gameObject.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        if (type == ItemType.mushroomLegendary)
        {
            audioSource.PlayOneShot(GameManager.instance.legendaryReaction);
        }
        else if (type == ItemType.mushroomBad)
        {
            int i = Random.Range((int)0, (int)2);
            audioSource.PlayOneShot(GameManager.instance.badMushroomReaction[i]);
        }
    }

    private void OnMouseEnter()
    {
        if (type == ItemType.mushroomBad || type == ItemType.mushroomGood || type == ItemType.mushroomLegendary)
        {
            highlightSprite.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        if (type == ItemType.mushroomBad || type == ItemType.mushroomGood || type == ItemType.mushroomLegendary)
        {
            highlightSprite.enabled = true;
            isSelectedByPlayer = true;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.hasClickedOnShroom = true;
        }
    }

    private void OnMouseExit()
    {
        if (type == ItemType.mushroomBad || type == ItemType.mushroomGood || type == ItemType.mushroomLegendary)
        {
            highlightSprite.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.tag == "Mushroom" && collision.gameObject.tag == "Player")
        {
            CollectibleCollisionProcessor colProcessor = collision.gameObject.GetComponent<CollectibleCollisionProcessor>();
            colProcessor.PickupMushroom(this);
        }
    }
}
