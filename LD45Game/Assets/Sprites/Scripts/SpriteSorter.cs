using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]
    int sortingLayerOffset = 0;

    [SerializeField]
    bool movingObject = true; //  moving object -> check sorting layer in every update

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -(int)(gameObject.transform.position.y * 100) + sortingLayerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingObject)
            spriteRenderer.sortingOrder = -(int)(gameObject.transform.position.y*100) + sortingLayerOffset;
    }
}
