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
        if (!GameManager.instance.playerEyesWork)
            return;
        if (collision.gameObject.tag == "MainCamera")
        {
            outsideCameraBounds = true;
            cameraFollow.activePlayerEyes--;
           // if (!coroutineStarted)
               // StartCoroutine(StartMoveCameraAfterDelay());
        }
    }

    public IEnumerator StartMoveCameraAfterDelay()
    {
        coroutineStarted = true;

        yield return new WaitForSeconds(cameraDelay);
        if (GameManager.instance.playerEyesWork)
        {
            cameraFollow.StartCameraFollow();
            coroutineStarted = false;
        }
        else
        {
            coroutineStarted = false;
        }
    }
}

