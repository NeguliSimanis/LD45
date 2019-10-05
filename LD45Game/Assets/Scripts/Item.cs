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
}
