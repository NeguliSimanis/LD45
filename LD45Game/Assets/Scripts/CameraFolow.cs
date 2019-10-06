using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject playerFrontEye;
    private Vector3 offset;
    //private bool followPlayer = false;
    private bool isFollowingPlayer = false;

    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

  /*  // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (followPlayer)
            transform.position = player.transform.position + offset;
    }*/

   /* private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerEye" && !is)
        {
            followPlayer = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "PlayerEye")
        {
            followPlayer = true;
        }
    }*/

    public IEnumerator MoveCameraToTargetPos(Vector3 startPosition, Vector3 endPosition, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            isFollowingPlayer = true;
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            Vector3 currentPosition = new Vector3
                (Mathf.Lerp(startPosition.x, endPosition.x, percentageComplete),
                Mathf.Lerp(startPosition.y, endPosition.y, percentageComplete),
                Mathf.Lerp(startPosition.z, endPosition.z, percentageComplete));
            

            transform.position = currentPosition;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
        isFollowingPlayer = false;
    }
}
