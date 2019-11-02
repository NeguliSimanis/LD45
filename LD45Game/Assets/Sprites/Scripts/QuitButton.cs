using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    void Start()
    {
        #if UNITY_WEBGL
        gameObject.SetActive(false);
        #endif
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
