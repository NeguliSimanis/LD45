using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    AudioSource audioSource;

    [HideInInspector]
    public Vector2Int gridCoordinates;
    [HideInInspector]
    public Vector3 worldCoordinates;
    public Vector3 gridToWorldOffset;

    public int AffectSanityLevel;
    public int AffectHungerLevel;

   /* private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Mushroom")
        {
            Debug.Log("COLLIDE");
            GameManager.instance.AddToSanityLevel(AffectSanityLevel);
            GameManager.instance.AddToHungerLevel(AffectHungerLevel);
        }
    }*/

    /// <summary>
    /// eating sfx is played in the calling method
    /// </summary>
    public void GetPickedUpByPlayer()
    {
        if (GameManager.instance.isGamePaused)
            return;
        GameManager.instance.AddToSanityLevel(AffectSanityLevel);
        GameManager.instance.AddToHungerLevel(AffectHungerLevel);

       GameManager.instance.occupiedTiles.Remove(gridCoordinates);
        if (type == ItemType.mushroomLegendary)
        {
            audioSource = GameManager.instance.gameObject.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
            audioSource.PlayOneShot(GameManager.instance.legendaryReaction);
        }
        Destroy(gameObject);
    }

    /*private void ClearTile()
    {
        for (int i = 0; i < GameManager.instance.occupiedTiles.Count; i++)
        {
            if (GameManager.instance.occupiedTiles[i].x == gridCoordinates.x && GameManager.instance.occupiedTiles[i].y == gridCoordinates.y)
            {
                GameManager.instance.occupiedTiles.Remove(i);
                return;
            }
        }
    }*/
}
