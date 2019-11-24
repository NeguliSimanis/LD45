using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    [HideInInspector]
    public ItemStatus itemStatus;
    AudioSource audioSource;
    PlayerController playerController;

    Color defaultColor = new Color(1, 1, 1, 1);
    Color hiddenColor = new Color(1, 1, 1, 0);
    SpriteRenderer spriteRenderer;
    [SerializeField]
    SpriteRenderer highlightSprite;
    
    #region COORDINATES
    [HideInInspector]
    public bool isSelectedByPlayer = false;
    [HideInInspector]
    public Vector2Int gridCoordinates;
    [HideInInspector]
    public Vector3 worldCoordinates;
    public Vector3 gridToWorldOffset;
    #endregion

    #region Interactable items
    public int AffectSanityLevel;
    public int AffectHungerLevel;
    float chanceToPlayMushroomSFX = 0.4f;
    #endregion

    private void Start()
    {
        GameManager.instance.itemsOnMap.Add(this);

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeSprite(ItemStatus.hidden);
    }

    public void RevealOnMap()
    {
        itemStatus = ItemStatus.visible;
        ChangeSprite(itemStatus);
    }

    private void ChangeSprite(ItemStatus visibility)
    {
        if (visibility == ItemStatus.hidden)
        {
            spriteRenderer.color = hiddenColor;
        }
        else if (visibility == ItemStatus.visible)
        {
            spriteRenderer.color = defaultColor;
        }
    }

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

    private void OnDestroy()
{
    GameManager.instance.itemsOnMap.Remove(this);
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
        if (playerController != null)
        {
            ProcessCollisionWithPlayer(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController != null)
        {
            ProcessCollisionWithPlayer(collision);
        }
    }

    void ProcessCollisionWithPlayer(Collider2D collision)
    {
        if (gameObject.tag == "Mushroom" 
                && collision.gameObject.tag == "Player"
                && playerController.hasClickedOnShroom)
            {
                CollectibleCollisionProcessor colProcessor = collision.gameObject.GetComponent<CollectibleCollisionProcessor>();
                colProcessor.PickupMushroom(this);
            }
    }

}
