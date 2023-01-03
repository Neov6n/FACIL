using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class levelManager : MonoBehaviour
{
    public bool menuScene = false;
    public bool abletoPause = true;
    public bool abletoInv = true;

    public class HotbarKeyComparer : IComparer<ItemDragUsableSlot>
    {
        public int Compare(ItemDragUsableSlot box1, ItemDragUsableSlot box2)
        {
            return box1.hotbarKey.CompareTo(box2.hotbarKey);
        }
    }

    public bool isPaused;

    //[SerializeField] private KeyCode InvKey = KeyCode.Tab;
    public bool inInventory => theInventoryManager.InventoryScreen.activeSelf;

    public GameObject PauseMenu;
    public bool inMenu => inInventory || isPaused;

    //public GameObject Chunk;
    public float chunkSideLength = 128;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Transform playerLocation;
    private newPlayerMovementScript PlayerScript;
    private InventoryManager theInventoryManager;

    public chunkScript[] chunkDrawer;
    //public GameObject[] surroundingChunks;

    [HideInInspector] public bool landNeedsToBeSpawned;
    [HideInInspector] public int preland;

    public GameObject chunkywunky; //chunk prefab
    private float lastCheckedValue = -1;
    [HideInInspector] public Vector3 oldCurrentChunk;

    private bool landSpawnSwitch = true;
    private float landSpawnBuffer;
    [HideInInspector] public float theLandSpawnBufferTime = 2; //just needs to be more than 0 really

    [HideInInspector] public int howManyPlayerHaves;
    [HideInInspector] public int howManyCurrents; // should be one;

    [HideInInspector] public bool start0currentFunction = false; //not sure if this even needs to exist but whatever

    public bool infiniteDesert = true;
    public bool dynamicDesert = false;

    [Header("ImpactEffects")]
    //public GameObject impactEffectDefault;
    public GameObject impactEffectCactus;
    public GameObject[] impactEffectsRocks;
    public GameObject[] impactEffectDirt;
    public GameObject[] impactEffectsMetal;
    public GameObject[] impactEffectsWood;
    public GameObject[] impactEffectsBlood;

    [Header("Audio")]
    public AudioMixer theAudioMixer;

    private AudioMixerSnapshot inMenuLevels;
    private AudioMixerSnapshot normalLevels;
    public float transitionTime;

    private Coroutine changeMusicVol;

    public AudioSource Select1;
    public AudioSource Select2;

    [Header("InventoryControls")]
    public List<ItemDragUsableSlot> OrderedHotbar;
    public bool orderHotbarTrue = true;

    public Transform hotbarPhysical; //where all the weapons and stuff are stored
    public GameObject[] HotbarPrefabs;

    public int currentHotbarKey;
    public bool dohotbar = true;

    public Button InventoryBackground;

    [Header("HudControls")]

    public GameObject[] HudObject; //to make things dissappear when you open menus

    public TMP_Text InteractText;
    private Animator InteractTextAnimator;
    public string oldnews;

    private bool watashi = true;
    public  bool flickfirst = true; //toggle this false in place like maain menu etc where there is no need for inventory

    [Header("Playervalues")]

    public float playerMaxHealth;
    private float playerCurrentHealth;

    public float timeBeforeHeal;
    private float healTicker;

    public GameObject NothingCameraObject;
    public GameObject deathPrefab;

    public GameObject DeathScreen;
    public TMP_Text DeathMessage;

    private Volume GlobalVolume;

    //public bool finalKey = false;

    private SceneManagerPlus theSceneManagerPlus;

    [Header("StructuresAndSuch")]

    public int[] randomnessForStructs;
    public GameObject[] StructureObjects;
    public int[] prevSpawned; //0 is false, 1 is true, 2 is doesnt matter

    public float defaultRandomMultiplier;
    public float randomMultiplier;
    public float randomTicker;

    private float resetTicker;
    
    [Header("Support")]
    public GameObject XboxCursor;
    public GameObject XboxCursorController;
    public bool usingXboxController;

    FACILCONTROLS controls;
    //pressed means held in this context
    [Header("Input")]
    public Vector2 movementInput;
    public Vector2 lookInput;
    public Vector2 scrollInput;

    public bool fire2Pressed;

    public bool intPressed;
    public int intTimeHeld;
    public bool crouchPressed;
    public int crouchTimeHeld;
    public bool jumpPressed;
    public int jumpTimeHeld;
    public bool fire1Pressed;
    public int fire1TimeHeld;
    public bool sprintPressed;
    public int sprintTimeHeld;

    public bool pausePressed;
    public bool invPressed;

    public int nextTimeHeld;
    public int prevTimeHeld;

    public int oneTimeHeld;
    public int twoTimeHeld;
    public int threeTimeHeld;
    public int fourTimeHeld;
    public int fiveTimeHeld;
    public int sixTimeHeld;
    public int sevenTimeHeld;
    public int eightTimeHeld;
    public int nineTimeHeld;

    //public bool reloadPressed;

    //all number keys

    void OnDisable()
    {
        controls.Default.Disable();
        controls.UI.Disable();
    }

    void Awake()
    {
        randomMultiplier = defaultRandomMultiplier;
        //for structures
        //prevSpawned = new bool[StructureObjects.Length]; 

        //new input system
        controls = new FACILCONTROLS();

        controls.Default.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Default.Move.canceled += ctx => movementInput = Vector2.zero;
        controls.Default.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Default.Look.canceled += ctx => lookInput = Vector2.zero;
        controls.Default.MouseScrollwheel.performed += ctx => scrollInput = ctx.ReadValue<Vector2>();
        controls.Default.MouseScrollwheel.canceled += ctx => scrollInput = Vector2.zero;
        controls.Default.Fire2.performed += ctx => fire2Pressed = true;
        controls.Default.Fire2.canceled += ctx => fire2Pressed = false;

        controls.Default.Crouch.performed += ctx => CrouchTime();
        controls.Default.Crouch.canceled += ctx => crouchPressed = false;
        controls.Default.Sprint.performed += ctx => SprintTime();
        controls.Default.Sprint.canceled += ctx => sprintPressed = false;
        controls.Default.Interact.performed += ctx => InteractTime();
        controls.Default.Interact.canceled += ctx => intPressed = false;
        controls.Default.Jump.performed += ctx => JumpTime();
        controls.Default.Jump.canceled += ctx => jumpPressed = false;
        controls.Default.Fire1.performed += ctx => Fire1Time();
        controls.Default.Fire1.canceled += ctx => fire1Pressed = false;

        controls.Default.Pause.performed += ctx => pausePressed = !pausePressed;
        controls.Default.Inventory.performed += ctx => invPressed = !invPressed;

        controls.Default.NextWeapon.performed += ctx => nextTimeHeld = 0;
        controls.Default.PreviousWeapon.performed += ctx => prevTimeHeld = 0;
        //numbers
        controls.Default.One.performed += ctx => oneTimeHeld = 0;
        controls.Default.Two.performed += ctx => twoTimeHeld = 0;
        controls.Default.Three.performed += ctx => threeTimeHeld = 0;
        controls.Default.Four.performed += ctx => fourTimeHeld = 0;
        controls.Default.Five.performed += ctx => fiveTimeHeld = 0;
        controls.Default.Six.performed += ctx => sixTimeHeld = 0;
        controls.Default.Seven.performed += ctx => sevenTimeHeld = 0;
        controls.Default.Eight.performed += ctx => eightTimeHeld = 0;
        controls.Default.Nine.performed += ctx => nineTimeHeld = 0;
    }

    void JumpTime()
    {
        jumpPressed = true;
        jumpTimeHeld = 0;
    }

    void Fire1Time()
    {
        fire1Pressed = true;
        fire1TimeHeld = 0;
    }
    void CrouchTime()
    {
        crouchPressed = true;
        crouchTimeHeld = 0;
    }
    void SprintTime()
    {
        sprintPressed = true;
        sprintTimeHeld = 0;
    }
    void InteractTime()
    {
        intPressed = true;
        intTimeHeld = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        theSceneManagerPlus = FindObjectOfType<SceneManagerPlus>();
        abletoPause = true;

        isPaused = false;
        PlayerScript = FindObjectOfType<newPlayerMovementScript>();
        if (PlayerScript != null)
        {
            Player = PlayerScript.gameObject;
            playerLocation = Player.transform;
        }
        preland = 0;
        landSpawnBuffer = -1;

        theInventoryManager = FindObjectOfType<InventoryManager>();

        //setup audio
        inMenuLevels = theAudioMixer.FindSnapshot("InMenu");
        normalLevels = theAudioMixer.FindSnapshot("Normal");

        if (flickfirst) //flickers inventory menu to update values
        {
            GoInInventory();
        }

        playerCurrentHealth = playerMaxHealth;
        if (DeathScreen != null)
        {
            DeathScreen.SetActive(false);

        }

        GlobalVolume = FindObjectOfType<Volume>();
    }

    void OnEnable()
    {
        controls.Default.Enable();
        controls.UI.Enable();

        if (infiniteDesert)
        {
            //Instantiate center chunk at levelmanager position//meaning that player spawn must always be in 0,0 square//no, just where the level manager is, which happens to be 0,0
            Instantiate(chunkywunky, gameObject.transform);
            //set this chunk as center / friend
            //instantiate surrounding chunks in right locations
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(chunkSideLength, 0, 0), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(-chunkSideLength, 0, 0), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(0, 0, chunkSideLength), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(0, 0, -chunkSideLength), Quaternion.identity, gameObject.transform); //4 compass cardinals
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(chunkSideLength, 0, chunkSideLength), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(-chunkSideLength, 0, chunkSideLength), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(-chunkSideLength, 0, -chunkSideLength), Quaternion.identity, gameObject.transform);
            Instantiate(chunkywunky, gameObject.transform.position - new Vector3(chunkSideLength, 0, -chunkSideLength), Quaternion.identity, gameObject.transform); //the inbetweens
                                                                                                                                                                    //set these chunks as friend
        }

        oldCurrentChunk = gameObject.transform.position;
        landNeedsToBeSpawned = false; //spawn original 3 by 3 in the start loop
        chunkDrawer = FindObjectsOfType<chunkScript>();

        if (InteractText != null)
        {
            InteractText.SetText("");
            InteractTextAnimator = InteractText.gameObject.GetComponent<Animator>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (dynamicDesert)
        {
            if (resetTicker > 0f)
            {
                resetTicker -= Time.deltaTime;
            } else
            {
                ResetSpawns();
                resetTicker = 1f;
            }
        }

        //for fish
        if(randomTicker > 0)
        {
            randomMultiplier = defaultRandomMultiplier/2;
            randomTicker -= Time.deltaTime;
        } else
        {
            randomMultiplier = defaultRandomMultiplier;
        }


        if (flickfirst && !orderHotbarTrue) // this causes hotbar values to be evaluated immediately at the start
        {
            LeaveInventory(); // put this somewhere?
            flickfirst = false;
        }

        if (landSpawnBuffer == theLandSpawnBufferTime)
        {
            landSpawnSwitch = false;
            landSpawnBuffer = landSpawnBuffer - 1f;
        }
        else if (landSpawnBuffer > 0f)
        {
            landSpawnBuffer = landSpawnBuffer - 1f;
        } else if (landSpawnBuffer == 0f)
        {
            landNeedsToBeSpawned = true;
            landSpawnBuffer = landSpawnBuffer - 1f;
        }

        List<chunkScript> deservesToLive = new List<chunkScript>(chunkDrawer); // to remove undefined values from chunkDrawer
        deservesToLive.RemoveAll(delegate (chunkScript o) { return o == null; });
        chunkDrawer = deservesToLive.ToArray();

        if (howManyPlayerHaves > 1)
        {
            start0currentFunction = true;
        }
        if (howManyCurrents == 0 && start0currentFunction)
        {
            foreach (chunkScript i in chunkDrawer)
            {
                if (i.iHavePlayer)
                {
                    if (i.gameObject.transform.position.x != oldCurrentChunk.x && i.gameObject.transform.position.z != oldCurrentChunk.z)
                    {
                        continue;
                    }
                    else {
                        i.exception = true;
                        if (landSpawnSwitch)
                        {
                            landSpawnBuffer = theLandSpawnBufferTime;
                        }
                        preland = 0;

                        break;
                    }
                }
            }
        }

        howManyPlayerHaves = 0;
        foreach (chunkScript i in chunkDrawer)
        {
            if (i.iHavePlayer)
            {
                howManyPlayerHaves += 1;
                if (i.gameObject.transform.position != oldCurrentChunk)
                {
                    preland = preland + 1;
                }
                if (i.gameObject.transform.position == oldCurrentChunk)
                {
                    preland = preland + 2;
                    //Debug.Log("yeah");
                }
            }
        }
        howManyCurrents = 0;
        foreach (chunkScript i in chunkDrawer)
        {
            if (i.iAmCurrentChunk)
            {
                howManyCurrents += 1;

            }
        }
        if (preland == 1) //to make sure there are not multiple current chunks
        {
            if (landSpawnSwitch)
            {
                landSpawnBuffer = theLandSpawnBufferTime;
            }
            preland = 0;
        } else
        {
            preland = 0;
        }
        //check if currentchunk changes

        if (landNeedsToBeSpawned) //spawn land
        {
            //Debug.Log("cuum");you have no idea how long this took to fix lol this is scuffed
            foreach (chunkScript i in chunkDrawer)
            {
                if (i.iAmCurrentChunk)
                {
                    //take this chunk out of surrounding chunk array
                    //clear surrounding chunk array of chunks that are not within friendchunk radius
                    //spawn required number of more chunks around the currentchunk, into the surrounding chunk array
                    //place previous chunk into surrounding chunk array
                    if (!isPaused)
                    {
                        SpawnLand(i.gameObject.transform.position);
                    }
                }
            }
        }

        foreach (chunkScript i in chunkDrawer) //this is an elastic switch for the var iAmCurrentChunk// optomise this somehow
        {
            if (i.chunkScriptCounter != lastCheckedValue)
            {
                i.iAmCurrentChunk = false;
                //Debug.Log("haha false");
            }
            lastCheckedValue = i.chunkScriptCounter;
        }

        if (abletoPause)
        { 
            if (pausePressed)
            {
                if (inInventory == false)
                {
                    if (isPaused == false)
                    {
                        Pause();
                        pausePressed = false;
                    }
                    else if (isPaused == true)
                    {
                        Unpause();
                        pausePressed = false;
                        //Debug.Log("thsi hsould only happen once");
                    }
                } else
                {
                    LeaveInventory();
                    pausePressed = false;
                }
            }

            if (abletoInv)
            {
                if (invPressed && inInventory == false && inMenu == false && isPaused == false)
                {
                    GoInInventory();
                    invPressed = false;
                }
                else
                if (invPressed && inInventory == true && isPaused == false)
                {
                    LeaveInventory();
                    invPressed = false;
                }
            }
            if (dohotbar)
            {
                if (oneTimeHeld == 1)
                {
                    currentHotbarKey = 1;
                }
                else if (twoTimeHeld == 1)
                {
                    currentHotbarKey = 2;
                }
                else if (threeTimeHeld == 1)
                {
                    currentHotbarKey = 3;
                }
                else if (fourTimeHeld == 1)
                {
                    currentHotbarKey = 4;
                }
                else if (fiveTimeHeld == 1)
                {
                    currentHotbarKey = 5;
                }
                else if (sixTimeHeld == 1)
                {
                    currentHotbarKey = 6;
                }
                else if (sevenTimeHeld == 1)
                {
                    currentHotbarKey = 7;
                }
                else if (eightTimeHeld == 1)
                {
                    currentHotbarKey = 8;
                }
                else if (nineTimeHeld == 1)
                {
                    currentHotbarKey = 9;
                }

                if (/*Input.GetAxis("Mouse ScrollWheel")*/scrollInput.y > 0 || nextTimeHeld == 1)
                {
                    //NextWeapon();
                    currentHotbarKey += 1;
                    if (currentHotbarKey > 9)
                    {
                        currentHotbarKey = 1;
                    }
                }
                if (/*Input.GetAxis("Mouse ScrollWheel")*/scrollInput.y < 0 || prevTimeHeld ==1)
                {
                    //PreviousWeapon();
                    currentHotbarKey -= 1;
                    if (currentHotbarKey < 1)
                    {
                        currentHotbarKey = 9;
                    }
                }

                SwitchItem();
            }
        }
        PlayerHealOverTime(); //heals play over time
        if (GlobalVolume != null)
        {
            BloodVignette();
        }
        if (InventoryBackground != null)
        {
            if (usingXboxController)
            {
                InventoryBackground.enabled = false;
            }
            else
            {
                InventoryBackground.enabled = true;
            }
        }

        //for input timers
        jumpTimeHeld++;
        fire1TimeHeld++;
        crouchTimeHeld++;
        sprintTimeHeld++;
        intTimeHeld++;
        nextTimeHeld++;
        prevTimeHeld++;
        oneTimeHeld++;
        twoTimeHeld++;
        threeTimeHeld++;
        fourTimeHeld++;
        fiveTimeHeld++;
        sixTimeHeld++;
        sevenTimeHeld++;
        eightTimeHeld++;
        nineTimeHeld++;
    }

    public void SwitchHud()
    {
        if (inMenu)
        {
            foreach (GameObject grub in HudObject)
            {
                grub.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject grub in HudObject)
            {
                grub.SetActive(true);
            }
        }
    }

    public void Pause()
    {
        Select1.Play();
        //Time.timeScale = 0.000001f; //you could make this value just really small and not 0 
        //dont use timescale for pause;
        isPaused = true;
        PauseMenu.SetActive(true);

        PlayerScript.canLook = false;
        PlayerScript.canMove = false;
        //StartCoroutine(SnapshotTransition(inMenuLevels, transitionTime)); // not sure why this jumps to a value and doesn't transition

        //check if coroutnie is running
        StopAllCoroutines();
        changeMusicVol = StartCoroutine(theSceneManagerPlus.LerpMusicVolume(PlayerPrefs.GetInt("UMusicVolume")-20));
        //Debug.Log("loco");

        ItemDraggable[] cocaine = FindObjectsOfType<ItemDraggable>();
        foreach(ItemDraggable smelly in cocaine)
        {
            smelly.levelPaused = true;
        }
        //SwitchHud();
        if (usingXboxController)
        {
            XboxCursor.SetActive(true);
            XboxCursorController.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            controls.Default.Jump.Disable();
        } else
        {
            Cursor.lockState = CursorLockMode.None; //can also be confined
            Cursor.visible = true;
        }

        controls.Default.Inventory.Disable();
    }

    public void Unpause()
    {
        Select2.Play();
        //Time.timeScale = 1;
        isPaused = false;
        PauseMenu.SetActive(false);
        PlayerScript.canLook = true;
        PlayerScript.canMove = true;
        if (!inMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            XboxCursor.SetActive(false);
            //XboxCursorController.SetActive(false);
            controls.Default.Jump.Enable();
        }
        //StartCoroutine(SnapshotTransition(normalLevels, transitionTime));

        StopAllCoroutines();
        StartCoroutine(theSceneManagerPlus.LerpMusicVolume(PlayerPrefs.GetInt("UMusicVolume")));

        ItemDraggable[] cocaine = FindObjectsOfType<ItemDraggable>();
        foreach (ItemDraggable smelly in cocaine)
        {
            smelly.levelPaused = false;
        }
        //SwitchHud();
        controls.Default.Inventory.Enable();
        
    }

    public IEnumerator SnapshotTransition(AudioMixerSnapshot bud, float transitTime) //look to scene manager plus for how to do this properly
    {
        bud.TransitionTo(transitTime);
        yield break;
    }

    public void GoInInventory()
    {
        //Select2.Play();
        theInventoryManager.InventoryScreen.SetActive(true);

        //PlayerScript.canLook = false;
        //make the player unable to move?

        theInventoryManager.RefreshInventoryItems(); //7:20
        //Debug.Log(theInventoryManager.GetItemList().Count);
        if (orderHotbarTrue)
        {
            OrderHotbar();
            orderHotbarTrue = false;
        }
        if (usingXboxController)
        {
            XboxCursor.SetActive(true);
            XboxCursorController.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            controls.Default.Jump.Disable();
            controls.Default.Look.Disable();
            controls.Default.Fire1.Disable();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; //can also be confined
        }
        SwitchHud();
    }

    public void LeaveInventory()
    {
        theInventoryManager.InventoryScreen.SetActive(false);
        if (!inMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            PlayerScript.canLook = true;
            Cursor.visible = false;
            XboxCursor.SetActive(false);
            //XboxCursorController.SetActive(false);
            controls.Default.Jump.Enable();
        }
        controls.Default.Look.Enable();
        controls.Default.Fire1.Enable();
        SwitchHud();
    }

    public void GoToMainMenu()
    {
        theSceneManagerPlus.ContainSceneLoadingInfo("MainMenu");
        SceneManager.LoadScene(1);
    }

    public void ResetSpawns()//for dynamic
    {
        //for strucutres
        bool scum = false;
        foreach (int pee in prevSpawned)
        {
            if (pee == 0)
            {
                scum = true;
            }
        }
        if (!scum)
        {
            //there are no new structures left to spawn so repeat
            int x = 0;
            foreach (int pee in prevSpawned)
            {
                if (pee == 1)
                {
                    prevSpawned[x] = 0;
                }
                x++;
            }
        }
    }

    public void SpawnLand(Vector3 newCurrentChunk)
    {
        //for strucutres
        bool scum = false;
        foreach (int pee in prevSpawned)
        {
            if (pee == 0)
            {
                scum = true;
            }
        }
        if (!scum)
        {
            //there are no new structures left to spawn so repeat
            int x = 0;
            foreach(int pee in prevSpawned)
            {
                if(pee == 1)
                {
                    prevSpawned[x] = 0;
                }
                x++;
            }
        }

        //Debug.Log("multiple");
        List<Vector3> positionsOfDeletedChunks = new List<Vector3>();
        //Debug.Log(positionsOfDeletedChunks.Count);
        //clear chunks not within friend radius
        foreach (chunkScript i in chunkDrawer) //gets stuck here
        {
            if (!i.iAmAFriendOfCurrent)
            {
                positionsOfDeletedChunks.Add(i.gameObject.transform.position); //this messes up
                Destroy(i.gameObject);
            }
        }

        if(positionsOfDeletedChunks.Count != 3) { Debug.Log("land spawn skipped"); landNeedsToBeSpawned = false; return; }
        //Debug.Log(positionsOfDeletedChunks[0] + positionsOfDeletedChunks[1] + positionsOfDeletedChunks[2]); //calling these the second time after a turn causes error
        //Debug.Log(positionsOfDeletedChunks.Count);
        //find the difference between old current chunk and new current chunk, and add that to each position (times 3) of deleted chunk for each new chunk
        Vector3 differenceBetweenChunks = newCurrentChunk - oldCurrentChunk;//new minus original
        //spawn more in the right places (how to figure out right places//any movment, three more will need to be spawned
        Instantiate(chunkywunky, positionsOfDeletedChunks[0] + (differenceBetweenChunks * 3), Quaternion.identity, gameObject.transform);
        Instantiate(chunkywunky, positionsOfDeletedChunks[1] + (differenceBetweenChunks * 3), Quaternion.identity, gameObject.transform);
        Instantiate(chunkywunky, positionsOfDeletedChunks[2] + (differenceBetweenChunks * 3), Quaternion.identity, gameObject.transform);

        oldCurrentChunk = newCurrentChunk;
        landSpawnSwitch = true;
        landNeedsToBeSpawned = false;
        positionsOfDeletedChunks.Clear();

        chunkDrawer = FindObjectsOfType<chunkScript>(); //restock chunk drawer with new inventory of chunks, always should be full of 9
    }

    public void ImpactEffects(RaycastHit hit)
    {

        if (hit.transform.tag == "Cacti")
        {
            GameObject impactGo = Instantiate(impactEffectCactus, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        else if (hit.transform.tag == "Rock")
        {
            GameObject impactGo = Instantiate(impactEffectsRocks[Random.Range(0, impactEffectsRocks.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        else if (hit.transform.tag == "Metal")
        {
            GameObject impactGo = Instantiate(impactEffectsMetal[Random.Range(0, impactEffectsMetal.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        else if (hit.transform.tag == "Wood")
        {
            GameObject impactGo = Instantiate(impactEffectsWood[Random.Range(0, impactEffectsWood.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        else if (hit.transform.tag == "Blood")
        {
            GameObject impactGo = Instantiate(impactEffectsBlood[Random.Range(0, impactEffectsBlood.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
        else
        {
            GameObject impactGo = Instantiate(impactEffectDirt[Random.Range(0, impactEffectDirt.Length)], hit.point, Quaternion.LookRotation(hit.normal));
            //GameObject impactGo = Instantiate(impactEffectDefault, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
    }

    public void OrderHotbar()
    {
        ItemDragUsableSlot[] arrayy = FindObjectsOfType<ItemDragUsableSlot>();
        OrderedHotbar = new List<ItemDragUsableSlot>(arrayy);
        HotbarKeyComparer hkc = new HotbarKeyComparer();
        OrderedHotbar.Sort(hkc);
        theInventoryManager.hotbarArray = new InventoryItem[OrderedHotbar.Count]; //maybe change this later, because might break if player starts with items in their hotbar

        HotbarPrefabs = new GameObject[OrderedHotbar.Count];

        if (watashi)
        {
            theInventoryManager.RedistributeItemsToHotbarUponStartGame();
            watashi = false;
            theInventoryManager.RefreshHotbar(); //or this RefreshInventoryItems()
        }
    }

    public void OrderHotbarPrefabs()
    {
        //delete all objects in hotbarprefabs
        //int itemKeep =-1;
        for (var q = 0; q < HotbarPrefabs.Length; q++)
        {
            if (HotbarPrefabs[q] != null)
            {
                Destroy(HotbarPrefabs[q]);
                /*
                if (HotbarPrefabs[q].activeSelf == false)
                {
                    Destroy(HotbarPrefabs[q]);
                }
                else
                {
                    Debug.Log("man");
                    itemKeep = q;
                }*/
            }
        }
        HotbarPrefabs = new GameObject[HotbarPrefabs.Length]; 
        /*
        if(itemKeep > -1)
        {
            HotbarPrefabs[itemKeep] = hotbarPhysical.transform.GetChild(0).gameObject;
        }*/
        //spawn new items in the hotbarPhysical parent
        for (var b = 0; b < theInventoryManager.hotbarArray.Length; b++)
        {
            if (theInventoryManager.hotbarArray[b] != null)
            {
                //GameObject meso = Instantiate(theInventoryManager.hotbarArray[b].prefab, theInventoryManager.hotbarArray[b].prefabSpawnPosition, Quaternion.identity, hotbarPhysical);
                
                //HotbarPrefabs.Add(meso); //i like this too because it orders them for me //also ordered them wrong
                /*
                if (HotbarPrefabs[b] != null)
                {
                    if (HotbarPrefabs[b].name != theInventoryManager.hotbarArray[b].prefab.name +"(Clone)")
                    {
                        Debug.Log("catears");
                        goto SpawnToolPrefab;
                    }
                }
                else
                {
                    goto SpawnToolPrefab;
                }*/

                //SpawnToolPrefab:
                GameObject meso = Instantiate(theInventoryManager.hotbarArray[b].prefab, hotbarPhysical);
                HotbarPrefabs[b] = meso;
                meso.transform.localPosition = theInventoryManager.hotbarArray[b].prefabSpawnPosition;
                meso.transform.rotation = Quaternion.identity;

                //disactive them done in switchitem  constantly
            }
        }
    }

    public void SwitchItem()
    {
        for (var p = 0; p < HotbarPrefabs.Length; p++)
        {
            if(p == (currentHotbarKey -1))
            {
                if (HotbarPrefabs[p] != null)
                {
                    HotbarPrefabs[p].SetActive(true);//in the  future, this will spawn an instance of the object needed, and an animation for taking out the item will play ( no need for order hotbar prefab loop)
                }
                continue;
            }
            if (HotbarPrefabs[p] != null)
            {
                HotbarPrefabs[p].SetActive(false);
            }
        }
    }

    public void UpdateInteractText(string news)
    {
        InteractText.SetText(news);
        //animate text

        if (oldnews != news)
        {
            InteractTextAnimator.SetTrigger("Bounceyes");
        }

        oldnews = news;
    }

    public void PlayerHealOverTime()
    {
        if (playerCurrentHealth > 0 && !isPaused)
        {
            if (playerCurrentHealth < playerMaxHealth)
            {
                healTicker -= Time.deltaTime;
                if (healTicker <= 0)
                {
                    playerCurrentHealth += 10f * Time.deltaTime;
                }
            }
            else if (playerCurrentHealth > playerMaxHealth)
            {
                playerCurrentHealth = playerMaxHealth;
            }
        }
    }

    public void PlayerTakeDamage(float amount)
    {
        if (!isPaused)
        {
            playerCurrentHealth -= amount;
            if (playerCurrentHealth <= 0)
            {
                PlayerDie("You Suck");
            }
            healTicker = timeBeforeHeal;
        }
    }
    public void PlayerDie(string deathMessage)
    {
        LeaveInventory();
        controls.Default.Inventory.Disable();
        //PlayerScript.canMove = false;
        //PlayerScript.canLook = false;
        abletoInv = false;
        Vector3 rotation = new Vector3(NothingCameraObject.transform.localEulerAngles.x, Player.transform.localEulerAngles.y, 0);
        Quaternion rotat = Quaternion.Euler(rotation);
        //Debug.Log(rotation);

        GameObject poopObejct = Instantiate(deathPrefab,Player.transform.position, rotat);
        //poopObejct.transform.eulerAngles = rotation;
        poopObejct.GetComponent<Rigidbody>().velocity = PlayerScript.velocity;
        Player.SetActive(false);

        DeathMessage.SetText(deathMessage);
        foreach (GameObject grub in HudObject)
        {
            grub.SetActive(false);
        }
        StartCoroutine(ToRespawnScreen());
    }

    public void BloodVignette()
    {
        float intensity = (-0.5f / 100f) * (playerCurrentHealth - 100f);
        GlobalVolume.profile.TryGet<Vignette>(out var bloodEffect);
        bloodEffect.intensity.value = intensity;
    }

    public IEnumerator ToRespawnScreen()
    {
        abletoPause = false;
        yield return new WaitForSeconds(6);
        GlobalVolume.profile.TryGet<LiftGammaGain>(out var lgg);
        //Debug.Log(lgg);
        //lgg.gamma.value.w = Mathf.Lerp(lgg.gamma.value.w, 0, Time.deltaTime);
        float valp = 0;
        while (valp > -1f + .1f)
        {
            //Debug.Log(valp);
            valp = Mathf.Lerp(valp, -1f, 1f*Time.deltaTime);
            lgg.gamma.Override(new Vector4(1f, 1f, 1f, valp));
            yield return new WaitForEndOfFrame();
        }
        DeathScreen.SetActive(true);
        //flash inventory screen here//////////////////////////////////////////////////////////////////////////////
        //i have instead opted to keep it open and have the death screen cover it

        GoInInventory();

        //Debug.Log(lgg.gamma.value.w);
        yield return new WaitForSeconds(2);
        //theSceneManagerPlus.ContainSceneLoadingInfo("Desert");
        if (PlayerPrefs.GetInt("hardMode") == 1)
        {
            //true
            //noloadsave
            PlayerPrefs.SetInt("LoadSaveExists", 0);
            //Roll credits
            // return to main menu
            theSceneManagerPlus.ContainSceneLoadingInfo("Credits"); //false
            SceneManager.LoadScene(1);
        }
        else
        {
            theSceneManagerPlus.ContainSceneLoadingInfo("InfiniteDesert3"); //false
            SceneManager.LoadScene(1);
        }
    }

    public IEnumerator SpawnStructures() //no t being used
    {
        int x = 0;
        foreach (GameObject poo in StructureObjects)
        {
            //Debug.Log(PlayerPrefs.GetInt("finalKey"));
            //if (!theLevelManager.finalKey && x == 1)
            if (PlayerPrefs.GetInt("finalKey") == 0 && x == 1) //this asks if there is no final key for the object and if so continues to the next loop without spawning object
            {
                //Debug.Log("skip");
                x++;
                continue; //maybe break later
            }
            if (prevSpawned[x] == 0)
            {
                int blool = Random.Range(0, randomnessForStructs[x]);
                if (blool == 0)
                {
                    SpawnOther(poo);
                    prevSpawned[x] = 1;
                    //Debug.Log("Spawned"+poo.name);
                    break;
                }
            }
            else if (prevSpawned[x] == 2)
            {
                int blool = Random.Range(0, randomnessForStructs[x]);
                if (blool == 0)
                {
                    SpawnOther(poo);
                    break;
                }
            }
            x++;
        }

        yield return new WaitForEndOfFrame();
    }

    void SpawnOther(GameObject otherObject1)
    {
        //randomSpawnPosition = new Vector3(Random.Range(gameObject.transform.position.x, gameObject.transform.position.x + chunkSideLength), 1, Random.Range(gameObject.transform.position.z - chunkSideLength, gameObject.transform.position.z));
        //Instantiate(otherObject1, randomSpawnPosition, Quaternion.identity, gameObject.transform); //make sure to make this a child so that is deleted once more land is created
    }
}
