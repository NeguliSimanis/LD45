using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridLayout grid;
    private List<Vector3Int> currentPath;

    public bool isDead = false;
    Vector2 targetPosition;
    bool isMoving = false;


    void Update()
    {
        if (!isDead && !GameManager.instance.isGamePaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                targetPosition = Input.mousePosition;
                targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);

                if (transform.position.x != targetPosition.x && transform.position.y != targetPosition.y)
                {
                    isMoving = true;
                    //_animator.SetBool("isMoving", isMoving);

                }

                if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                {
                    isMoving = false;
                    // _animator.SetBool("isMoving", isMoving);
                }
            }
        }
        if (isMoving)
            MovePlayer();
    }

    void MovePlayer()
    {
        //transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed);
    }
    /*
     * if (isMoving && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed);
            }

     */


}
