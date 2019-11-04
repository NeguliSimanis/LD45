using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    bool listenToAnyKeyForUnpause = false;
    bool gameUnpausedAfterIntro = false;
    [SerializeField]
    GameObject gameStoryIntro;

    void Start()
    {
        GameManager.instance.isGamePaused = true;
    }

    public void StartListeningToGameAnyKey()
    {
        listenToAnyKeyForUnpause = true;
    }

    private void Update()
    {
        if (!listenToAnyKeyForUnpause)
            return;
        if (Input.anyKey && !gameUnpausedAfterIntro)
        {
            UnPauseGameAfterIntro();
        }
    }

    private void UnPauseGameAfterIntro()
    {
        gameUnpausedAfterIntro = true;
        gameStoryIntro.SetActive(false);
        GameManager.instance.UnPauseGame();
        StartCoroutine(GameManager.instance.DisablePlayerMovementForXSeconds(0.3f));
    }
}
