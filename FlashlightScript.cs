using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    public AudioSource flashlightOnSound;
    public AudioSource flashlightOffSound;
    private bool flashlightIsOff;
    public Light theFlashLight;
    private levelManager theLevelManager;
    private newPlayerMovementScript thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
        thePlayer = FindObjectOfType<newPlayerMovementScript>();

        if (theFlashLight.gameObject.activeSelf)
        {
            flashlightIsOff = false;
        }else
        {
            flashlightIsOff = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        thePlayer.aimingDownSights = false; //as long as everything is operating normally, while this flashlight is active player should never be aiming
        if (!theLevelManager.inMenu)
        {
            if (theLevelManager.fire1TimeHeld == 1)  //getbuttondown for semiautomatic
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (flashlightIsOff)
        {
            flashlightOnSound.Play();
            theFlashLight.gameObject.SetActive(true);
            flashlightIsOff = false;
        }
        else if (!flashlightIsOff) //on, turn off
        {
            flashlightOffSound.Play();
            theFlashLight.gameObject.SetActive(false);
            flashlightIsOff = true;
        }
    }
}
