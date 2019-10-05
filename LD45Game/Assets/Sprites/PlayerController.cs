using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridLayout grid;
    private List<Vector3Int> currentPath;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
        }
    }
}
