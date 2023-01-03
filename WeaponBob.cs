using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("MovingIntensities")]
    public float xAmplitudeMoving = 0.05f;
    public float yAmplitudeMoving = 0.2f;
    public float MovingFrequency = 8f;
    public float snappinessMoving = 2f;

    [Header("ShiftingIntensities")]
    public float xAmplitudeShift = 0.05f;
    public float yAmplitudeShift = 0.2f;
    public float ShiftFrequency = 4f;
    public float snappinessShift = 2f;

    [Header("CrouchingIntensities")]
    public float xAmplitudeCrouch = 0.05f;
    public float yAmplitudeCrouch = 0.2f;
    public float CrouchFrequency = 4f;
    public float snappinessCrouch = 2f;

    [Header("IdleIntensities")]
    public float xAmplitudeIdle = 0.05f;
    public float yAmplitudeIdle = 0.05f;
    public float IdleFrequency = 1f;
    public float snappinessIdle = 2f;

    [Header("Jumping")]
    public float jumpMultiplier = 5f;
    public float jumpLowerLimit = -1f;
    public float jumpUpperLimit = 0f;
    public float snappinessJump = 2f;

    //other stuff
    private float swayCounter;

    private Vector3 originalPosition;

    [HideInInspector] public bool playerIsMoving=false;
    [HideInInspector] public bool playerIsShifting = false;
    [HideInInspector] public bool playerInAir = false;
    [HideInInspector] public bool playerIsCrouching = false;
    [HideInInspector] public Transform ADSTransform;
    private Vector3 hipFirePosition;
    [HideInInspector] public bool aimingDownSights = false;

    private newPlayerMovementScript thePlayer;
    private levelManager theLevelManager;

    [Header("Turn On Bob")]
    public bool bobQueen = true;

    // Start is called before the first frame update
    void Start()
    {
        hipFirePosition = gameObject.transform.localPosition;
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerVariables();
        if (!theLevelManager.isPaused)
        {
            if (!playerInAir)
            {
                if (bobQueen)
                {
                    if (playerIsMoving)
                    {
                        if (playerIsCrouching)
                        {
                            bob(swayCounter, xAmplitudeCrouch, yAmplitudeCrouch, snappinessCrouch);
                            swayCounter += Time.deltaTime * CrouchFrequency;
                        } else if (playerIsShifting)
                        {
                            bob(swayCounter, xAmplitudeShift, yAmplitudeShift, snappinessShift);
                            swayCounter += Time.deltaTime * ShiftFrequency;
                        }
                        else
                        {
                            bob(swayCounter, xAmplitudeMoving, yAmplitudeMoving, snappinessMoving);
                            swayCounter += Time.deltaTime * MovingFrequency;
                        }
                    }
                    else
                    {
                        //idle
                        bob(swayCounter, xAmplitudeIdle, yAmplitudeIdle, snappinessIdle);
                        swayCounter += Time.deltaTime * IdleFrequency;
                    }
                }
            }
            else
            {
                inAir(jumpMultiplier, snappinessJump, thePlayer.velocity.y, jumpLowerLimit, jumpUpperLimit);
            }


            if (!aimingDownSights)
            {
                originalPosition = hipFirePosition;
            }
            else
            {
                originalPosition = ADSTransform.localPosition;
            }

        }
    }

    void bob(float zValue, float xValueAmplitude, float yValueAmplitude, float snappiness)
    {
        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, originalPosition + new Vector3(Mathf.Cos(zValue) * xValueAmplitude, Mathf.Sin(zValue * 2) * yValueAmplitude, 0f), snappiness * Time.deltaTime);
    }

    void inAir(float multiplier, float snappiness, float yVelocity, float lowerLimit, float upperLimit)
    {
        Vector3 targetPosition = Vector3.Lerp(gameObject.transform.localPosition, originalPosition + new Vector3(0f, -yVelocity * multiplier,0f), snappiness * Time.deltaTime);
        targetPosition = new Vector3(targetPosition.x, Mathf.Clamp(targetPosition.y, lowerLimit, upperLimit), targetPosition.z);
        //gameObject.transform.localPosition = Vector3.Slerp(gameObject.transform.localPosition,targetPosition, 4f * Time.deltaTime);
        gameObject.transform.localPosition = targetPosition;
    }

    void SetPlayerVariables()
    {
        playerIsMoving = thePlayer.amWalking;
        playerIsShifting = thePlayer.IsSprinting;
        playerInAir = thePlayer.amInAir;
        playerIsCrouching = thePlayer.IsCrouching;
    }
}
