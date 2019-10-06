using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    ItemType itemType;

    public Vector2Int gridCoordinates;
    public Vector3 worldCoordinates;
    public Vector3 gridToWorldOffset;

    public int AffectSanityLevel;
    public int AffectHungerLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Mushroom")
        {
            GameManager.instance.AddToSanityLevel(AffectSanityLevel);
            GameManager.instance.AddToHungerLevel(AffectHungerLevel);
        }
    }
}
