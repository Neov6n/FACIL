using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private newPlayerMovementScript thePlayer;
    public float targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (thePlayer != null)
        {
            LookAtPlayer();
        }
        ApplyFinalMovements();
    }

    public void ApplyFinalMovements()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotation, transform.eulerAngles.z);
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
        }
        else if (relativePositionOfPlayer.x > 0 && relativePositionOfPlayer.y == 0)//this one
        {
            //Debug.Log("Fuckoff");
        }
        else if (relativePositionOfPlayer.x > 0 && relativePositionOfPlayer.y < 0)
        {
            targetRotation = (180 - angleFromZAxis);
        }
        else if (relativePositionOfPlayer.x == 0 && relativePositionOfPlayer.y < 0)//are these necessary
        {
            //Debug.Log("Fuckoff");
        }
        else if (relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y < 0)
        {
            targetRotation = 180 + angleFromZAxis;
        }
        else if (relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y == 0) //this
        {
            //Debug.Log("Fuckoff");
        }
        else if (relativePositionOfPlayer.x < 0 && relativePositionOfPlayer.y > 0)
        {
            targetRotation = 360 - angleFromZAxis;
        }
        else if (relativePositionOfPlayer.x == 0 && relativePositionOfPlayer.y > 0)//last one
        {
            //Debug.Log("Fuckoff");
        } //let it be known FACIL is made by a mathmagician 
    }

}
