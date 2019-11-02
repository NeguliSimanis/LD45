using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceCollisionProcessor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Road")
        {
            GameManager.instance.playerCurrentMoveSpeed = GameManager.instance.playerRoadMoveSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Road")
        {
            GameManager.instance.playerCurrentMoveSpeed = GameManager.instance.defaultPlayerMoveSpeed;
        }
    }
}
