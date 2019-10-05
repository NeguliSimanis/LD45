using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridLayout grid;
    private List<Vector3Int> currentPath;

    public bool isDead = false;
    private bool moveCommandReceived = false;

    Vector3 targetPosition;
    bool isMoving = false;

    [SerializeField]
    float moveSpeed = 2f;
    [SerializeField]
    GameObject moveUpPlayerSprite;
    [SerializeField]
    GameObject moveDownPlayerSprite;

    private Story story;
    int storyLength;

    void Update()
    {
        if (!isDead && !GameManager.instance.isGamePaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                moveCommandReceived = true;
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = transform.position.z;
            }

            if (moveCommandReceived && (transform.position.x != targetPosition.x || transform.position.y != targetPosition.y))
            {
                MovePlayer();
            }

        }
    }
    
    void MovePlayer()
    {
        float step = Time.deltaTime * moveSpeed;
        if (targetPosition.y > transform.position.y)
        {
            moveUpPlayerSprite.SetActive(true);
            moveDownPlayerSprite.SetActive(false);
        }
        else
        {
            moveUpPlayerSprite.SetActive(false);
            moveDownPlayerSprite.SetActive(true);
        }
        if (targetPosition.x >= transform.position.x)
        {
            moveDownPlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
            moveUpPlayerSprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            moveDownPlayerSprite.transform.localScale = new Vector3(1, 1, 1);
            moveUpPlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }


}
