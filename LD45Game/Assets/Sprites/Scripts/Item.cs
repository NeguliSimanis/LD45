using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    [HideInInspector]
    public ItemStatus itemStatus;
    AudioSource audioSource;

    Color defaultColor = new Color(1, 1, 1, 1);
    Color hiddenColor = new Color(1, 1, 1, 0);
    SpriteRenderer spriteRenderer;

    #region COORDINATES
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
        if (Random.Range(0f,1f) > chanceToPlayMushroomSFX)
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
}
