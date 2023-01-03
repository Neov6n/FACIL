using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPlayerMovementScript : MonoBehaviour
{

    private InventoryManager theInventoryManager;
    private levelManager theLevelManager;

    public bool canMove { get; set; } = true;
    public bool canLook = true;
    public bool IsSprinting = false;
    public bool IsGrounded => characterController.isGrounded;
    public bool isMovingForward;
    public bool IsAiming => canSprint && aimingDownSights;
    public bool ShouldJump => canHoldJump ? theLevelManager.jumpPressed && characterController.isGrounded : theLevelManager.jumpTimeHeld == 1 && characterController.isGrounded; //getkey for holding down jump, otherwise getkeydown
    private bool ShouldCrouch => theLevelManager.crouchTimeHeld == 1 && !duringCrouchAnimation && characterController.isGrounded;
    private bool ShouldProne => theLevelManager.crouchTimeHeld > (theLevelManager.usingXboxController ? 30 : 50 )/* this changes how long it takes for prone to activate*/ && !duringProneAnimation && characterController.isGrounded && theLevelManager.crouchPressed && !duringCrouchAnimation && IsCrouching && !justProned; // if crouch is held for a certain amount of time?
    public bool amWalking;
    public bool amInAir;
    public bool aimingDownSights;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canHoldJump = false;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canDuckLand = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool canProne = true;
 
    [Header("Controls")]
    //[SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    //[SerializeField] private KeyCode jumpKey = KeyCode.Space;
    //[SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 12.0f;//12 is a good speed, 10
    [SerializeField] private float sprintSpeed = 24f; //15
    [SerializeField] private float crouchSpeed = 6.0f; //3
    [SerializeField] private float aimSpeed = 6.0f; //5
    [SerializeField] private float proneSpeed = 1.0f;
    private float actualProneSpeed;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSensX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSensY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 90.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 90.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForceY = 18.0f;
    [SerializeField] private float gravity = 45.0f;
    [SerializeField] private float timeToCrouchJump = 0.15f;
    private float downwardConstant = -2.0f;
    private float jumpForceH; //jumpForce Horizontal;
    //[SerializeField] private float landingImpactAmount;
    //[SerializeField] private float landingImpactSnappiness;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 3.75f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    public bool IsCrouching;
    private bool duringCrouchAnimation;

    [Header("Sprint Parameters")]
    [SerializeField] private float sprintDuration = 2;
    [SerializeField] private float sprintCooldownLength = 1;
    [SerializeField] private float sprintRechargeSpeed = 1;
    [SerializeField] private bool wantsToSprint;
    private bool charged = true;
    private float sprintTicker;
    private float sprintCooldownTicker;
    public bool unstarted = true;

    [Header("Prone Parameters")]
    [SerializeField] private float proneHeight = .25f;
    [SerializeField] private float timeToProne = 0.25f;
    [SerializeField] private Vector3 proneCenter = new Vector3(0, 1f, 0);
    public bool IsProning;
    private bool duringProneAnimation;
    public bool justProned = false;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 8f;
    [SerializeField] private float sprintBobAmount = 0.025f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    [SerializeField] private float aimBobSpeed = 8f;
    [SerializeField] private float aimBobAmount = 0.025f;
    private float defaultYPos = 0f;
    private float timerH;

    [Header("DuckOnLand")]
    [SerializeField] private float duckAmount = 0.5f;
    [SerializeField] private float duckSpeed = 20f;
    [SerializeField] private float duckTolerance = 0.01f;
    private bool shouldDuck =false;
    private bool shouldDuckUp = false;
    private float duckTimer = 0f;
    private int playlandSound = 1;

    private Camera playerCamera; // the main camera used for looking, currently the nothing camera
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    [HideInInspector] public float rotationX = 0;
    [HideInInspector] public float rotationY = 0;

    //velocity
    private Vector3 newPosition;
    private Vector3 oldPosition;
    public Vector3 velocity; //read only

    //jumplanding
    private Vector3 originalPositionCamera;
    private bool justJumped;

    [Header("Interact")]
    public LayerMask ItemLayer;
    //[SerializeField] private KeyCode InteractKey = KeyCode.E;
    public bool shouldPickupItems = true;

    [Header("FootstepSounds")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.5f;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] sandClips;
    [SerializeField] private AudioClip[] woodClips;
    [SerializeField] private AudioClip[] steelClips;
    [SerializeField] private AudioClip[] concreteClips;
    [SerializeField] private LayerMask floorLayer;
    private float footstepTimer;
    private float GetCurrentOffset => IsCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    //private float oldsinevalue = 1;
    //private float timerF;
    private float GetCurrentVolume => IsCrouching ? 0.5f : IsSprinting ? 1f : 1f;

    [Header("Sensitivity Altering")]
    public float ADSMultiplier =1f;
    public int shouldConsiderADSMultiplier = 0; //0, 1

    void Awake()
    {
        actualProneSpeed = proneSpeed;
        //sprint
        sprintTicker = sprintDuration;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        theInventoryManager = FindObjectOfType<InventoryManager>();
        theLevelManager = FindObjectOfType<levelManager>();
        //inventory = new InventoryManager();
        //inventory.SetInventory(inventory);

        oldPosition = gameObject.transform.localPosition;
        originalPositionCamera = playerCamera.transform.localPosition;

        //set playerpref sensitivity
        lookSensX = (float)PlayerPrefs.GetInt("XSens");
        lookSensY = (float)PlayerPrefs.GetInt("YSens");
        shouldConsiderADSMultiplier = PlayerPrefs.GetInt("AlterSensBool");
    }

    void Update()
    {
        if (canLook)
        {
            HandleMouseLook();
        }

        if (canMove)
        {
            HandleMovementInput();


            if (canJump)
            {
                HandleJump();
            }
            if (canCrouch)
            {
                HandleCrouch();
            }
            if (canSprint)
            {
                HandleSprint();
            }
            if (canUseHeadbob)
            {
                HandleHeadbob();
            }
            if (canDuckLand)
            {
                HandleDuckLand();
            }
            if (useFootsteps)
            {
                HandleFootsteps();
            }
            if (canProne)
            {
                HandleProne();
            }

            ApplyFinalMovements();
        }

        //velocity
        newPosition = gameObject.transform.localPosition;
        velocity = newPosition - oldPosition;
        oldPosition = gameObject.transform.localPosition;

        if(amInAir && justJumped)
        {
            duckTimer = 0;
            justJumped = false;
        }
        if (shouldPickupItems)
        {
            LookForItem();
        }

        //for prone
        if (!theLevelManager.crouchPressed)
        {
            justProned = false;
        }
        if (IsAiming)
        {
            proneSpeed = actualProneSpeed /4f;

        }else
        {
            proneSpeed = actualProneSpeed;

        }
    }

    private void HandleMovementInput()
    {
        if (characterController.isGrounded)
        {
            currentInput = new Vector2((IsProning ? proneSpeed : IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : IsAiming ? aimSpeed : walkSpeed) * theLevelManager.movementInput.y/*Input.GetAxis("Vertical")*/, (IsProning ? proneSpeed : IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : IsAiming ? aimSpeed : walkSpeed) * theLevelManager.movementInput.x/*Input.GetAxis("Horizontal")*/); //now this is poggers
            if (Mathf.Abs(velocity.x) > 0 || Mathf.Abs(velocity.z) > 0)
            {
                amWalking = true;
            }
            else
            {
                amWalking = false;
            }
            amInAir = false;
        } else
        {
            currentInput = new Vector2(jumpForceH * theLevelManager.movementInput.y/*Input.GetAxis("Vertical")*/, jumpForceH * theLevelManager.movementInput.x/*Input.GetAxis("Horizontal")*/);
            amWalking = false;
            amInAir = true;
        }

        if(theLevelManager.movementInput.y > 0)
        {
            isMovingForward = true;
        } else
        {
            isMovingForward = false;
        }

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.forward * currentInput.x) + (transform.right * currentInput.y);
        //moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y); //overcomplicated comp3 way
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        //float mouseX = Input.GetAxis("Mouse X") * lookSensX; //* Time.deltaTime?
        //float mouseY = Input.GetAxis("Mouse Y") * lookSensY; // no
        float mouseX;
        float mouseY;
        if (shouldConsiderADSMultiplier == 1)
        {
            mouseX = theLevelManager.lookInput.x * lookSensX / 10 * ADSMultiplier;
            mouseY = theLevelManager.lookInput.y * lookSensY / 10 * ADSMultiplier;
        } else
        {
            mouseX = theLevelManager.lookInput.x * lookSensX / 10;
            mouseY = theLevelManager.lookInput.y * lookSensY / 10;
        }

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        rotationY = mouseX;
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump) //&& !Physics.Raycast(playerCamera.transform.position, Vector3.up, 2f))
        {
            if (!IsProning)
            {
                moveDirection.y = jumpForceY;
                if (IsCrouching)
                {
                    StartCoroutine(CrouchJump());
                }
                jumpForceH = (IsProning ? proneSpeed : IsCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : IsAiming ? aimSpeed : walkSpeed);
                justJumped = true;
            } else
            {
                StartCoroutine(ProneCrouch());
            }
        }
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            if (!IsProning)
            {
                StartCoroutine(CrouchStand());
            } else
            {
                StartCoroutine(ProneCrouch());
            }
        }
    }

    private void HandleProne()
    {
        if (ShouldProne)
        {
            //StartCoroutine(ProneCrouch()); // should this  go directly to prone stand or just between crouch and prone//have a seperate function like crouch jump for prone stand
            StopAllCoroutines();
            StartCoroutine(ProneCrouch());
        }
    }

    private void HandleSprint()
    {
        //IsSprinting => canSprint && theLevelManager.sprintPressed;

        if (IsAiming)
        {
           
            IsSprinting = false;
            wantsToSprint = false;
            return;

        }
        if(IsCrouching && wantsToSprint && unstarted && !IsProning && !duringProneAnimation && !duringCrouchAnimation) //fix this
        {
            StartCoroutine(CrouchStand());
            unstarted = false;
        }
        if (!IsCrouching)
        {
            unstarted = true;
        }

        if (theLevelManager.sprintTimeHeld == 1)
        {
            //toggle sprint
            wantsToSprint = !wantsToSprint;

            if(wantsToSprint == false)
            {
                IsSprinting = false;
                sprintCooldownTicker = sprintCooldownLength;
            }
        }

        if(sprintTicker > sprintDuration)
        {
            sprintTicker = sprintDuration; //reverts to next argument
        } else
        if(sprintTicker > 0 && sprintTicker <= sprintDuration)
        {
            if (charged)
            {
                if (wantsToSprint)
                {
                    if (isMovingForward)
                    {
                        if (!IsCrouching)
                        {
                            IsSprinting = true;
                            if (amWalking || amInAir)
                            {
                                sprintTicker -= Time.deltaTime;
                            }
                        }
                    }
                }
                else
                {
                    sprintTicker += sprintRechargeSpeed * Time.deltaTime;
                }
            }
        } else
        if(sprintTicker <= 0)
        {
            charged = false;
            IsSprinting = false;
            wantsToSprint = false;
            //if cooldown is over
            sprintTicker += Time.deltaTime; // push to above loop
            sprintCooldownTicker = sprintCooldownLength;
        }

        if(sprintCooldownTicker > 0)
        {
            sprintCooldownTicker -= Time.deltaTime;
        } else
        {
            charged = true;
        }
        if (!charged)
        {
            wantsToSprint = false;
        }

        if (!amWalking && !amInAir)
        {
            IsSprinting = false;
            wantsToSprint = false;
        }
        if (!isMovingForward)
        {
            IsSprinting = false;
            wantsToSprint = false;
        }

    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (shouldDuck || shouldDuckUp) return;

        if (amWalking)
        {
            timerH += Time.deltaTime * (IsCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : IsAiming ? aimBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timerH) * (IsCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : IsAiming ? aimBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleDuckLand()
    {
        if (!characterController.isGrounded)
        {
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos, playerCamera.transform.localPosition.z);
            return;
        }

        CalculateDuck();
        if (playlandSound == 0)
        {
            LandingSound();
            footstepTimer = GetCurrentOffset; //reset footstep timer 
            playlandSound += 1;
        }

        if (shouldDuck)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition,
                new Vector3(playerCamera.transform.localPosition.x, defaultYPos - duckAmount, playerCamera.transform.localPosition.z),
                duckSpeed * Time.deltaTime);
        }

        if (shouldDuckUp)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition,
                new Vector3(playerCamera.transform.localPosition.x, defaultYPos, playerCamera.transform.localPosition.z),
                duckSpeed * Time.deltaTime);
        }
    }

    private void CalculateDuck()
    {
        if (characterController.isGrounded)
        {
            if(duckTimer == 0f)
            {
                shouldDuck = true;
                playlandSound = 0;
            }
            if(playerCamera.transform.localPosition.y <= defaultYPos - duckAmount +duckTolerance && playerCamera.transform.localPosition.y >= defaultYPos - duckAmount - duckTolerance)
            {
                shouldDuck = false;
                shouldDuckUp = true;
            }
            if(playerCamera.transform.localPosition.y <= defaultYPos +duckTolerance && playerCamera.transform.localPosition.y >= defaultYPos - duckTolerance)
            {
                shouldDuckUp = false;
            }

            duckTimer += 1f;
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        } else if(characterController.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = downwardConstant; //constant downward force when grounded
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;

        //if (currentInput == Vector2.zero) return;
        if (theLevelManager.movementInput == Vector2.zero) return;

        if (velocity.x > -.01 && velocity.x < .01) //tests if velocity is greater than zero or not(less exact than above) 
        {
            if (velocity.z > -.01 && velocity.z < .01)
            {
                if (velocity.y > -.01 && velocity.y < .01)
                {
                    //all velocity is zero
                    return;
                } else
                {
                    //only considers x and z velocity is zero
                    footstepTimer = 0;
                    return;
                }
            }
        }
        //instead of timer play sound on headbob
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5, floorLayer))
            {
                //Debug.Log(hit.collider.tag);
                switch (hit.collider.tag)
                {
                    case "Chunk":
                        footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)], GetCurrentVolume);
                        break;
                    case "Rock":
                        footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)], GetCurrentVolume);
                        break;
                    case "Cacti":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)], GetCurrentVolume);
                        break;
                    case "Wood":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)], GetCurrentVolume);
                        break;
                    case "Metal":
                        footstepAudioSource.PlayOneShot(steelClips[Random.Range(0, steelClips.Length - 1)], GetCurrentVolume);
                        break;
                    default:
                        //footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)]);
                        break;
                }
            }
            footstepTimer = GetCurrentOffset;
        }
        /*
        timerF += Time.deltaTime * (1/GetCurrentOffset) * 5;
        Debug.Log((Mathf.Sin(timerF) * (IsCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : IsAiming ? aimBobAmount : walkBobAmount)*1000) * (oldsinevalue));
        if((Mathf.Sin(timerF) * 1000) *oldsinevalue < 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5, floorLayer))
            {
                //Debug.Log(hit.collider.tag);
                switch (hit.collider.tag)
                {
                    case "Chunk":
                        footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)]);
                        break;
                    case "Rock":
                        footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                        break;
                    case "Cacti":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    default:
                        //footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)]);
                        break;
                }
            }

            oldsinevalue = Mathf.Sin(timerF)* 1000;
        }
        */
    }

    private void LandingSound()
    {
        if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5, floorLayer))
        {
            switch (hit.collider.tag)
            {
                case "Chunk":
                    footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)]);
                    break;
                case "Rock":
                    footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                    break;
                case "Cacti":
                    footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                    break;
                case "Wood":
                    footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)], GetCurrentVolume);
                    break;
                case "Metal":
                    footstepAudioSource.PlayOneShot(steelClips[Random.Range(0, steelClips.Length - 1)], GetCurrentVolume);
                    break;
                default:
                    //footstepAudioSource.PlayOneShot(sandClips[Random.Range(0, sandClips.Length - 1)]);
                    break;
            }
        }
    }

    private IEnumerator CrouchStand()
    {
        if(unstarted == true)
        {
            wantsToSprint = false; //change this later
        }

        if(IsCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 2f))
        {
            yield break;
        }

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = IsCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = IsCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while(timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        IsCrouching = !IsCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator CrouchJump()
    {

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float currentHeight = characterController.height;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouchJump)
        {
            characterController.height = Mathf.Lerp(currentHeight, standingHeight, timeElapsed / timeToCrouchJump);
            characterController.center = Vector3.Lerp(currentCenter, standingCenter, timeElapsed / timeToCrouchJump);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = standingHeight;
        characterController.center = standingCenter; // to make sure these remain the same

        IsCrouching = !IsCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ProneCrouch()
    {
        if (IsProning && Physics.Raycast(playerCamera.transform.position, Vector3.up, 2f))
        {
            yield break;
        }

        duringProneAnimation = true;


        float timeElapsed = 0;
        float targetHeight = IsProning ? crouchHeight : proneHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = IsProning ? crouchingCenter : proneCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToProne)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToProne);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToProne);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        IsProning = !IsProning;

        duringProneAnimation = false;
        justProned = true;
    }

    private void OnTriggerEnter(Collider collider) //do a headbox here to cancel upward jump veloctiy
    {
        /*
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {

            theInventoryManager.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
            if (theLevelManager.inInventory)
            {
                theInventoryManager.RefreshInventoryItems();
            }
        }*/

        //maybe for prtals here
    }

    private void LookForItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f, ItemLayer, QueryTriggerInteraction.Collide))
        {
            ItemWorld itemWorld = hit.transform.GetComponent<ItemWorld>();


            if (itemWorld != null)
            {
                theLevelManager.UpdateInteractText("Press E to collect " + itemWorld.item.displayName);
                if (/*Input.GetKeyDown(InteractKey)*/ theLevelManager.intTimeHeld == 1)
                {
                    theInventoryManager.AddItem(itemWorld.GetItem());
                    itemWorld.DestroySelf();
                    if (theLevelManager.inInventory)
                    {
                        theInventoryManager.RefreshInventoryItems();
                    }
                    theLevelManager.UpdateInteractText("");
                }
            } else
            {
                if (hit.transform.gameObject.layer != 11)
                {
                    theLevelManager.UpdateInteractText("");
                }
            }
        }
        else
        {
            theLevelManager.UpdateInteractText("");
        }
    }
}
