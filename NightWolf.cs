using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightWolf : MonoBehaviour
{
    private Rigidbody theRigidbody;
    private newPlayerMovementScript thePlayer;
    private levelManager theLevelManager;
    public float moveSpeed;
    public float targetRotation;

    public bool shouldChase;
    // Start is called before the first frame update
    void Start()
    {
        theRigidbody = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!theLevelManager.isPaused)
        {
            UpdateWhileUnpaused();
        }
    }

    void UpdateWhileUnpaused()
    {
        if (thePlayer != null)
        {
            LookAtPlayer();
        }
        ApplyFinalMovements();
    }

    private void ApplyFinalMovements()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotation, transform.eulerAngles.z);
        if (shouldChase)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }

    public void LookAtPlayer() //literally pure math 
    {
        float angleFromZAxis;
        float lengthOfVerticalSide = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.z - thePlayer.transform.position.z, 2));
        float lengthOfHypotenuse = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - thePlayer.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - thePlayer.transform.position.z, 2));
        angleFromZAxis = Mathf.Acos(lengthOfVerticalSide / lengthOfHypotenuse);
        angleFromZAxis = ((angleFromZAxis * 360f) / (2f * Mathf.PI));

        Vector2 relativePositionOfPlayer = new Vector2(thePlayer.transform.position.x - gameObject.transform.position.x, thePlayer.transform.position.z - gameObject.transform.position.z);

        if (relativePositionOfPlayer.x > 0 && relativePositionOfPlayer.y > 0)
        {
            targetRotation = angleFromZAxis;
        } else if(relativePositionOfPlayer.x > 0 && relativePositionOfPlayer.y == 0)//this one
        {
            Debug.Log("Fuckoff");
        } else if(relativePositionOfPlayer.x > 0 && relativePositionOfPlayer.y < 0)
        {
            targetRotation = (180 - angleFromZAxis);
        } else if(relativePositionOfPlayer.x == 0 && relativePositionOfPlayer.y < 0)//are these necessary
        {
            Debug.Log("Fuckoff");
        } else if(relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y < 0)
        {
            targetRotation = 180 + angleFromZAxis;
        } else if(relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y == 0) //this
        {
            Debug.Log("Fuckoff");
        } else if(relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y > 0)
        {
            targetRotation = 360 - angleFromZAxis;
        } else if(relativePositionOfPlayer.x == 0 && relativePositionOfPlayer.y > 0)//last one
        {
            Debug.Log("Fuckoff");
        } //let it be known FACIL is made by a mathmagician 
    }

    //"Coyote Decoy 3D Scanned" (https://skfb.ly/ouJJn) by Catgirl is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
}
