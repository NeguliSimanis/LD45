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
    private bool isFollowingPlayer = false;

    public int activePlayerEyes = 4;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "PlayerEye")
        {
            Debug.Log(collision.gameObject.name);
            StartCoroutine(MoveCameraToTargetPos(transform.position, player.transform.position + offset, 1f));
        }
    }*/

    private void Update()
    {
        if (!isFollowingPlayer)
        {
           
            if (activePlayerEyes < 4)
            {
                isFollowingPlayer = true;
                StartCameraFollow();
            }
        }
    }

    public void StartCameraFollow()
    {
        StartCoroutine(MoveCameraToTargetPos(transform.position, player.transform.position + offset, 1f));
    }

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
                transform.position.z);
            

            transform.position = currentPosition;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
        isFollowingPlayer = false;
    }
}
