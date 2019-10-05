using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    int currentStoryID = -1;
    [SerializeField]
    Text storyUI;
    public string[] story =
    {
        "",
        "Today is 24th of May, year of our lord 1321",
        "My ship and crew were lost in a storm",
        "I have nothing left",
        "Will I find my way back to civilization?",
        "",
    };

    void Start()
    {
        GoToNextStoryStep();
    }

    public void GoToNextStoryStep()
    {
        if (currentStoryID < story.Length-1)
            currentStoryID++;
        storyUI.text = story[currentStoryID];
    }
}
