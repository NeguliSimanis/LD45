using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridLayout grid;
    private List<Vector3Int> currentPath;

    public bool isDead = false;
    public bool moveCommandReceived = false;

    Vector3 targetPosition;
    bool isMoving = false;

    private AudioSource soundWalking;

    [SerializeField]
    GameObject moveUpPlayerSprite;
    [SerializeField]
    GameObject moveDownPlayerSprite;
    [SerializeField]
    Animator[] playerAnimators;


    private Story story;
    int currentStoryID = 0;
    private float storyStartDelay = 3f;
    private float storyStartTime;
    private bool storyStarted = false;


    public void InitializePlayer()
    {
        story = GameManager.instance.gameObject.GetComponent<Story>();
        storyStartTime = Time.time + storyStartDelay;
    }

    private void Start()
    {
        soundWalking = GameManager.instance.gameObject.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
    }

    /*  void FindPositionAtStartOfLevel()
      {

      }*/

    void Update()
    {
        if (!isDead && !GameManager.instance.isGamePaused)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                moveCommandReceived = true;
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = transform.position.z;
            }
            if (moveCommandReceived && (transform.position.x != targetPosition.x || transform.position.y != targetPosition.y))
            {
                if(!soundWalking.isPlaying)
                {
                    soundWalking.Play();
                    SwitchWalkAnimation(true);
                }

                MovePlayer();
            }
            else
            {
                soundWalking.Stop();
                SwitchWalkAnimation(false);
            }
            if (!storyStarted && Time.time > storyStartTime)
            {
                storyStarted = true;
                StartStory();
            }

        }
    }

    void StartStory()
    {
        story.StartDisplayingStory();
        /*if (GameManager.instance.currentLevelID != 0)
            return;
        if (currentStoryID > storyLength)
            return;
        story.GoToNextStoryStep();  
        currentStoryID++;*/
    }
    
    void SwitchWalkAnimation(bool isWalking)
    {
        for (int i = 0; i < playerAnimators.Length; i++)
        {
            playerAnimators[i].SetBool("isWalking", isWalking);
        }

    }

    void MovePlayer()
    {
        if (GameManager.instance.isGamePaused)
            return;
        float step = Time.deltaTime * GameManager.instance.playerCurrentMoveSpeed;
        bool facingUp = true;
        if (targetPosition.y > transform.position.y)
        {
            playerAnimators[0].SetBool("facingUp", true);
            facingUp = true;
        }
        else
        {
            playerAnimators[0].SetBool("facingUp", false);
            facingUp = false;
            if (targetPosition.x >= transform.position.x)
            {
                moveUpPlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                moveUpPlayerSprite.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (targetPosition.x >= transform.position.x)
        {
            if (facingUp)
                moveUpPlayerSprite.transform.localScale = new Vector3(1, 1, 1);
            else
                moveUpPlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            if (facingUp)
                moveUpPlayerSprite.transform.localScale = new Vector3(-1, 1, 1);
            else
                moveUpPlayerSprite.transform.localScale = new Vector3(1, 1, 1);
        }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }


}
