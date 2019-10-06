using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEye : MonoBehaviour
{
    private float cameraDelay;
    [SerializeField]
    CameraFolow cameraFollow;

    bool outsideCameraBounds = true;
    bool coroutineStarted = false;

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            outsideCameraBounds = false;
            cameraFollow.activePlayerEyes++;
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            outsideCameraBounds = true;
            cameraFollow.activePlayerEyes--;
            Debug.Log("WHAAAT");
           // if (!coroutineStarted)
               // StartCoroutine(StartMoveCameraAfterDelay());
        }
    }

    public IEnumerator StartMoveCameraAfterDelay()
    {
        Debug.Log("startedthis");
        coroutineStarted = true;

        yield return new WaitForSeconds(cameraDelay);
        cameraFollow.StartCameraFollow();
        coroutineStarted = false;
    }
}

