using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]
    int sortingLayerOffset = 0;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = -(int)(gameObject.transform.position.y*100) + sortingLayerOffset;
    }
}
