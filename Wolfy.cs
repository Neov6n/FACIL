using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolfy : MonoBehaviour
{
    private Rigidbody theRigidbody;

    public int variable = 1;

    public bool isGrounded;

    public float radiusGroundCheck;
    public Transform groundCheckLocation;
    //public LayerMask Ground; //everything is ground

    public Vector3 moveDirection;

    //public float gravityFactor = 9.81f;
    //public float downwardConstant = 0f;

    public float jumpForce;

    public float feetLength = 1f;

    public float randomness = 10;

    private levelManager theLevelManager;
    private newPlayerMovementScript thePlayer;

    [SerializeField] private bool panicMode = false;

    public float moveSpeed =1;
    private bool reset = false;

    [SerializeField] private int currentState;

    private float floater;
    public float maxFloater = 1000;

    public float angle;

    private Vector3 targetRotation;

    public float turnRate;

    public int seed = 42;

    public bool lookAtPlayer;
    //public GameObject playerWatcher;

    // Start is called before the first frame update
    void Start()
    {
        theRigidbody = GetComponent<Rigidbody>();
        theLevelManager = FindObjectOfType<levelManager>();
        currentState = 0;
        PhaseChange();
        floater = maxFloater;

        thePlayer = FindObjectOfType<newPlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerWatcher.transform.LookAt(thePlayer.gameObject.transform);

        RaycastHit hit;
        isGrounded = Physics.SphereCast(groundCheckLocation.position, radiusGroundCheck, -transform.up, out hit, feetLength); //, Ground, QueryTriggerInteraction.Ignore
        //isGrounded = Physics.OverlapSphere(groundCheckLocation.position, radiusGroundCheck);
        if (!panicMode) {
            if ((variable / randomness) < 0.25)
            {
                currentState = 1;

            }
            else if ((variable / randomness) >= 0.25 && (variable / randomness) < 0.5)
            {
                currentState = 1;

            }
            else if ((variable / randomness) >= 0.5 && (variable / randomness) < .75)
            {
                currentState = 2;
                //towards player
                //goto Final;

            }
            else if ((variable / randomness) >= 0.75)
            {

                currentState = 3;
                //goto Final;

            }
        } else
        {
            currentState = 2;
        }
        //Final:

        if (reset)
        {
            if (currentState == 1)
            {
                RandomDirection();
            }
            else if (currentState == 2)
            {
                //FaceDirection(); for testing
                FacePlayerDirection();
            }
            else if (currentState == 3)
            {
                moveDirection = new Vector3(0, moveDirection.y, 0);
            }
            reset = false;
        }
        if(currentState == 2)
        {
            FacePlayerDirection();
        }

        ApplyFinalMovements();
        AdjustRotation();
        PoopFunction();

    }

    private void PhaseChange()
    {
        variable = Random.Range(0, (int)randomness);
        reset = true;
    }

    private void FaceDirection() //dont use this
    {
        CalculateAngle();
        if (moveDirection.x > 0 && moveDirection.z > 0)
        {
            angle += 0f;
        }
        else
        if (moveDirection.x > 0 && moveDirection.z < 0)
        {
            angle += 90f;
        }
        else
        if (moveDirection.x < 0 && moveDirection.z < 0)
        {
            angle += 180f;
        }
        else if (moveDirection.x < 0 && moveDirection.z > 0)
        {
            angle += 270f;
        }
        targetRotation = new Vector3(0f, angle, 0f);
    }

    private void FacePlayerDirection()
    {
        //moveDirection = new Vector3(gameObject.transform.position.x - thePlayer.transform.position.x, moveDirection.y, gameObject.transform.position.z - thePlayer.transform.position.z);
        //angle = Mathf.Asin(Mathf.Sqrt(Mathf.Pow((gameObject.transform.position.x - thePlayer.transform.position.x), 2) + Mathf.Pow((gameObject.transform.position.z - 0), 2)) /
        //Mathf.Sqrt(Mathf.Pow((gameObject.transform.position.x - thePlayer.transform.position.z), 2) + Mathf.Pow((gameObject.transform.position.z - thePlayer.transform.position.z), 2)));

        //angle = (angle * 360f) / (2f * Mathf.PI);

        lookAtPlayer = true;

        //targetRotation = new Vector3(0f, angle, 0f);
        //Debug.Log("p");
        //targetRotation = playerWatcher.transform.eulerAngles;
        //targetRotation = new Vector3(0f, playerWatcher.transform.rotation.y, 0f);
    }

    private void RandomDirection()
    {
        lookAtPlayer = false;

        float xSpeed = Random.Range(0, moveSpeed);
        float zSpeed = moveSpeed - xSpeed;

    Roll:
        //Random.InitState(seed);
        int glass = Random.Range(-1, 2); //gives either 1 or -1;
        if (glass == 0)
        {
            goto Roll;
        }

    Roll2:
        int horse = Random.Range(-1, 2);
        if (horse == 0)
        {
            goto Roll2;
        }

        moveDirection = new Vector3(xSpeed * (float)glass, moveDirection.y, zSpeed * (float)horse);

        if (moveDirection.x > 0 && moveDirection.z > 0){
            CalculateAngle();
            angle += 0f;
        } else 
        if (moveDirection.x > 0 && moveDirection.z < 0)
        {
            CalculateAngleSin();
            angle -= 540f;
            angle = Mathf.Abs(angle);
        }else
        if(moveDirection.x < 0 && moveDirection.z < 0)
        {
            CalculateAngle();
            angle += 180f;
        } else if(moveDirection.x < 0 && moveDirection.z > 0)
        {
            CalculateAngleSin();
            angle -= 360f;
            angle = Mathf.Abs(angle);
        }

        //gameObject.transform.eulerAngles = new Vector3(0f, angle * 100f, 0f);
        targetRotation = new Vector3(0f, angle, 0f);
    }

    public void CalculateAngle()
    {
        //mathf .pow
        //angle = Mathf.Acos(Mathf.Sqrt((0 - 0) * (0 - 0) + (0 - moveDirection.z) * (0 - moveDirection.z)) /
                //Mathf.Sqrt((0 - moveDirection.x) * (0 - moveDirection.x) + (0 - moveDirection.z) * (0 - moveDirection.z))); //distance formula Mathf.Sqrt((x-x)*(x-x) + (y-y)(y-y))

        angle = Mathf.Acos(Mathf.Sqrt(Mathf.Pow(0 - 0, 2) + Mathf.Pow(0 - moveDirection.z, 2)) / 
            Mathf.Sqrt(Mathf.Pow(0 - moveDirection.x, 2) + Mathf.Pow(0 - moveDirection.z, 2)));
        //angle = Mathf.Atan(Mathf.Sqrt(Mathf.Pow(0 - moveDirection.x, 2) + Mathf.Pow(0 - 0, 2)) /
            //Mathf.Sqrt(Mathf.Pow(0 - 0, 2) + Mathf.Pow(0 - moveDirection.z, 2)));

        //360 degrees = 2pi radians

        angle = (angle*360f) / (2f*Mathf.PI);
    }

    public void CalculateAngleSin()
    {
        angle = Mathf.Asin(Mathf.Sqrt(Mathf.Pow(0 - moveDirection.x, 2) + Mathf.Pow(0 - 0, 2)) /
            Mathf.Sqrt(Mathf.Pow(0 - moveDirection.x, 2) + Mathf.Pow(0 - moveDirection.z, 2)));

        angle = (angle * 360f) / (2f * Mathf.PI);
    }

    public void AdjustRotation()
    {
        //gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, targetRotation, turnRate * Time.deltaTime);
        if (lookAtPlayer)
        {
            transform.LookAt(thePlayer.gameObject.transform);
        } else
        {
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, targetRotation, turnRate * Time.deltaTime);
        }
    }

    private void PoopFunction() // determines when phase chnge happens
    {

        int poop = Random.Range(0, 5);
        if (poop == 4 && floater <= 0)
        {
            PhaseChange();
            floater = maxFloater;
        }
        else
        {
            floater -= 1;
        }
    }


    private void Jump() // when you call this function he will try to jump
    {
        if (isGrounded)
        {
            theRigidbody.velocity = new Vector3(theRigidbody.velocity.x, jumpForce, theRigidbody.velocity.z);
        }
    }

    private void ApplyFinalMovements()
    {
        /*
        if (!isGrounded)
        {
            if (moveDirection.y > 0f)
            {
                moveDirection.y -= gravityFactor * Time.deltaTime;
            }
        }*/
        theRigidbody.velocity = new Vector3(moveDirection.x, theRigidbody.velocity.y, moveDirection.z);
    }
}
