using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    Vector3Int lastKnownCellPosition;
    FogOfWar fogOfWar;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool moveCommandReceived = false;
    Vector3 targetPosition;
    [HideInInspector]
    public bool isMoving = false;

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

    public void MoveToStartOfLevel(Vector3 startCoordinates)
    {
        transform.position = startCoordinates;
        Debug.Log("start pos: " + tilemap.WorldToCell(transform.position));
        lastKnownCellPosition = tilemap.WorldToCell(transform.position);
        fogOfWar.RevealAroundCoordinate(lastKnownCellPosition);
    }

    private void Start()
    {
        fogOfWar = GameManager.instance.gameObject.GetComponent<FogOfWar>();
        soundWalking = GameManager.instance.gameObject.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (GameManager.instance.isGamePaused && soundWalking.isPlaying)
        {
            soundWalking.Stop();
        }
        if (!isDead && !GameManager.instance.isGamePaused && GameManager.instance.movementAllowed)
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
                    SwitchWalkState();
                }
                MovePlayer();
            }
            else
            {
                SwitchWalkState(false);
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
    }

    /// <summary>
    /// switches between walking and idle (and vice versa)
    /// </summary>
    void SwitchWalkState(bool startWalking = true)
    {
        if (startWalking)
        {
           // PlayerController
            soundWalking.Play();
            SwitchWalkAnimation(true);
            if (GameManager.instance.isStoryOver)
                GameManager.instance.startReducingPlayerStats = true;
        }
        else
        {
            soundWalking.Stop();
            SwitchWalkAnimation(false);
            GameManager.instance.startReducingPlayerStats = false;
        }
    }
    
    void SwitchWalkAnimation(bool isWalking)
    {
        for (int i = 0; i < playerAnimators.Length; i++)
        {
            playerAnimators[i].SetBool("isWalking", isWalking);
        }

    }

    void FindPlayerCellPosition()
    {
        Vector3Int currentPos = tilemap.WorldToCell(transform.position);
        if (currentPos != lastKnownCellPosition)
        {
            lastKnownCellPosition = currentPos;
            fogOfWar.RevealAroundCoordinate(lastKnownCellPosition);
            Debug.Log("currPos " + tilemap.WorldToCell(transform.position));
        }
    }

    void MovePlayer()
    {
        if (GameManager.instance.isGamePaused)
            return;
        float step = Time.deltaTime * GameManager.instance.playerCurrentMoveSpeed;
        UpdatePlayerSprite();
        FindPlayerCellPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    void UpdatePlayerSprite()
    {
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
    }
}
