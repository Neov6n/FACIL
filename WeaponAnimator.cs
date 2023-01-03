using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{

    public Animator myAnimator;
    private newPlayerMovementScript thePlayer;
    private levelManager theLevelManager;
    private bool sprintChange;
    private bool crouchChange;
    private bool groundChange;
    private bool walkChange;
    private bool pauseChange;
    public bool isFiring;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(thePlayer.IsSprinting != sprintChange)
        {
            //update animator
            myAnimator.SetBool("IsSprinting", thePlayer.IsSprinting);
        }
        sprintChange = thePlayer.IsSprinting;

        if (thePlayer.IsCrouching != crouchChange)
        {
            //update animator
            myAnimator.SetBool("IsCrouching", thePlayer.IsCrouching);
        }
        crouchChange = thePlayer.IsCrouching;

        if(thePlayer.IsGrounded != groundChange)
        {
            myAnimator.SetBool("IsGrounded", thePlayer.IsGrounded);
        }
        groundChange = thePlayer.IsGrounded;

        if(thePlayer.amWalking != walkChange)
        {
            myAnimator.SetBool("IsWalking", thePlayer.amWalking);
        }
        walkChange = thePlayer.amWalking;

        if(theLevelManager.isPaused != pauseChange)
        {
            myAnimator.SetBool("IsPaused", theLevelManager.isPaused);
        }
        pauseChange = theLevelManager.isPaused;

        if (theLevelManager.fire1TimeHeld == 1 && !isFiring && !theLevelManager.isPaused)
        {
            myAnimator.SetTrigger("Fire");
            //isFiring = true;
        }

        if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            isFiring = false;//will this set it false when its not supposed to 
        }
        else
        {
            isFiring = true; //theoretically this doesn't need to be here
        }
    }
}
