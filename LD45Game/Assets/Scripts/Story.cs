using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    [SerializeField]
    GameObject storyContainer;
    int currentStoryID = -1;
    int nextStoryID = 1;
    [SerializeField]
    Text storyUI;

    [SerializeField]
    private AudioClip[] voiceOver;
    [SerializeField]
    private AudioSource audioSource;

    private bool storyStarted = false;
    private float nextStoryTime;
    private float delayBetweenStoryItems = 0.8f;

    private float storyHideTime;
    private bool waitingToHideStory = false;
    private bool storyHidden = false;

    private string[] story =
    {
        "",
        "This is 24th day of May, in the year of our Lord 1709",
        "My ship and crew were lost in a storm",
        "I have nothing left",
        "Will I find my way back to civilization?",
        "Will I find my way back to civilization?"//,
        //"",
    };
    
    public void StartDisplayingStory()
    {
        storyStarted = true;
        nextStoryTime = Time.time;

    }

    private void Update ()
    {
        if (waitingToHideStory && !storyHidden && Time.time > storyHideTime)
        {
            HideStory();
        }

        if (!storyStarted)
            return;
        if (Time.time > nextStoryTime)
        {
            GoToNextStoryStep();
        }
    }

    void HideStory()
    {
        storyHidden = true;
        storyContainer.SetActive(false);
    }

    void Start()
    {
        GoToNextStoryStep(false);
    }

    public void GoToNextStoryStep(bool playAudio = true)
    {
        if (currentStoryID < story.Length - 1)
        {
            if (currentStoryID == 0)
                storyContainer.SetActive(true);
            if (currentStoryID == story.Length - 3)
            {
                storyHideTime = Time.time + 4.5f;
                waitingToHideStory = true;
            }
            if (currentStoryID < story.Length - 2 && playAudio)
            {
                audioSource.PlayOneShot(voiceOver[currentStoryID]);
            }
            currentStoryID++;
            nextStoryID++;

            if (currentStoryID == 2 || currentStoryID == 3)
            {

                nextStoryTime = Time.time + voiceOver[currentStoryID].length + delayBetweenStoryItems;
            }
            else
            {
                nextStoryTime = Time.time +5f; 
            }


            storyUI.text = story[currentStoryID];
        }
    }
}
