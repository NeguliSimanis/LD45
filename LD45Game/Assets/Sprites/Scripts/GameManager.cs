using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int currentLevelID = 0;
    [HideInInspector]
    public bool isGamePaused = false;
    [HideInInspector]
    public bool isStoryOver = false;
    public static GameManager instance = null;

    private ItemSpawner itemSpawner;
    private TileSpawner tileSpawner;

    #region ITEMS
    [HideInInspector]
    public List<Item> itemsOnMap;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField]
    AudioSource birdSounds;
    public AudioClip legendaryReaction;
    public AudioClip[] badMushroomReaction;
    #endregion

    #region MUSHROOMS
    [Header("MUSHROOM RULES")]
    // good
    public int minGoodShroomsInLevel;
    public int maxGoodShroomsInLevel;
    // bad
    public int minBadShroomsInLevel;
    public int maxBadShroomsInLevel;
    // legendary
    public int minLegendaryShroomsInLevel;
    public int maxLegendaryShroomsInLevel;

    [HideInInspector]
    public List<GameObject> goodShrooms;
    #endregion

    #region MAP
    [Header("MAP")]
    public int mapSizeX = 100;
    public int mapSizeY = 100;
    public int fogOfWarRadius = 5;
    [HideInInspector]
    /// <summary>
    /// tiles that contain interactable objects
    /// </summary>
    public List<Vector2Int> occupiedTiles;
    #endregion

    #region PLAYER STATS
    [Header("PLAYER")]
    public PlayerController playerController;

    [Header("MOVEMENT")]
    public float defaultPlayerMoveSpeed = 1f;
    public float playerRoadMoveSpeed = 1.7f;
    [HideInInspector]
    public float playerCurrentMoveSpeed;
    [HideInInspector]
    public bool movementAllowed = false;

    [Header("STATS")]
    public Slider hungerMeter;
    public Slider sanityProgressBar;
    [HideInInspector]
    public bool startReducingPlayerStats = false;
    private float sateLevel = 100;
    private float sanityLevel = 100;
    [SerializeField]
    private float hungerIncreaseSpeed = 0.025f;

    [HideInInspector]
    public bool playerEyesWork = false;
    #endregion

    #region VICTORY
    [Header("VICTORY")]
    public bool rudderFound = false;
    public bool compassFound = false;
    public bool anchorFound = false;

    [SerializeField]
    GameObject gameWinUI;
    #endregion

    #region DEFEAT
    [Header("DEFEAT")]
    [SerializeField]
    GameObject gameLoseUI;
    [SerializeField]
    Text gameLoseText;
    string hungerDefeat = "Ravaged by hunger, you perished in a land far from home.";
    string sanityDefeat = "Having lost everything you held dear in a merciless storm, you sought solace in the noxious mushrooms of the unknown land.\nIt was a mistake.";
    string timeDefeat = "You wasted away too much time wandering around the forest and the ship parts were carried away by animals and winds. Now you may never find the way back home.";
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Simanis")
        {
            SetupGameplayScene();
        }
    }

    private void SetupGameplayScene()
    {
        if (itemsOnMap == null)
        {
            itemsOnMap = new List<Item>();
        }

        occupiedTiles.Clear();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;

        sanityProgressBar.value = sanityLevel / 100;
        hungerMeter.value = sateLevel / 100;

    }

    public void EnterNextLevel()
    {
        Debug.Log("entering next level");
        DestroyOldItems();
        isGamePaused = true;
        playerEyesWork = false;
        StartCoroutine(DisablePlayerMovementForXSeconds(0.2f));

        occupiedTiles.Clear();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;

        isGamePaused = false;
        playerEyesWork = true;
    }

    private void DestroyOldItems()
    {
        Item[] oldItems = FindObjectsOfType<Item>();
        for (int i = 0; i < oldItems.Length; i++)
        {
            Destroy(oldItems[i].gameObject);
        }
    }

    private void ExecuteTestingMethods()
    {
        if (Input.GetKey(KeyCode.K))
        {
            GameManager.instance.UnPauseGame();
            StartCoroutine(GameManager.instance.DisablePlayerMovementForXSeconds(0.01f));
            /*for (int i = 0; i < itemsOnMap.Count; i++)
            {
                Debug.Log(itemsOnMap[i].gameObject.name);
            }*/
        }
    }

    private void Update()
    {
        ExecuteTestingMethods();
        if (isGamePaused)
            return;

        if (startReducingPlayerStats)
        {
            sateLevel -= hungerIncreaseSpeed;
            hungerMeter.value = sateLevel / 100;

            if (sateLevel <= 0)
            {
                LoseGame(DefeatType.hunger);
            }
        }

        if (anchorFound && compassFound && rudderFound)
        {
            Debug.Log("VICTORY");
            gameWinUI.SetActive(true);
            isGamePaused = true;

        }
    }

    public void AddToSateLevel(int amount)
    {
        sateLevel += amount;
        if (sateLevel <= 0)
        {
            //gameOver
            LoseGame(DefeatType.hunger);
        }
        hungerMeter.value = sateLevel / 100;
    }

    public void AddToSanityLevel(int amount)
    {
        sanityLevel += amount;
        //Debug.Log("adding sanity " + amount);
        if (sanityLevel <= 0)
        {
            //gameOver
            LoseGame(DefeatType.sanity);
            return;
        }

        else if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }

        sanityProgressBar.value = sanityLevel / 100;
    }

    public void UnPauseGame()
    {
        playerController.InitializePlayer();
        isGamePaused = false;
        birdSounds.enabled = true;
        playerController.RevealFogAroundPosition();
    }

    void LoseGame(DefeatType defeatType)
    {
        gameLoseUI.SetActive(true);
        isGamePaused = true;
        if (defeatType == DefeatType.hunger)
        {
            gameLoseText.text = hungerDefeat;
        }
        if (defeatType == DefeatType.sanity)
        {
            gameLoseText.text = sanityDefeat;
        }
        if (defeatType == DefeatType.time)
        {
            gameLoseText.text = timeDefeat;
        }
    }


    public IEnumerator DisablePlayerMovementForXSeconds(float xSeconds)
    {
        movementAllowed = false;
        yield return new WaitForSeconds(xSeconds);
        movementAllowed = true;
    }

}
