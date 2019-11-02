using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoordinates : MonoBehaviour
{

    public int x;
    public int y;

    public bool IsOutsideMap()
    {
        if (x < -GameManager.instance.mapSizeX || x > GameManager.instance.mapSizeX)
            return true;
        else if (y < -GameManager.instance.mapSizeY || y > GameManager.instance.mapSizeY)
            return true;
        return false;
    }
}
