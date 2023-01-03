using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerMovement : MonoBehaviour
{ 

    public CharacterController controller;

    public float speed = 12f;
    public float gravitationalConstant = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity; //only for jump
    bool isGrounded;
    //Vector3 bypass;
    //Vector3 bypass2;
    //public float stopConstant = 3f;
    //public float thresholdStop = 1f;

    public float jumpHeight = 5f;

    public float slideDecay = 3f;
    public float pushPower = 5;
    private Vector3 normal;

    public bool collidingWithWalls;
    public float normalForce = 15; //30

    public float touchDistance = 1;
    public GameObject feet;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //only goes to 1 on each axis 
        /*
        if(x > -thresholdStop && x < thresholdStop)
        {
            bypass = new Vector3(controller.velocity.x, 0, 0);
            controller.Move(-bypass * stopConstant * Time.deltaTime);
            Debug.Log("porque");
        }
        if (z > -thresholdStop && z < thresholdStop)
        {
            bypass2 = new Vector3(controller.velocity.z, 0, 0);
            controller.Move(-bypass2 * stopConstant * Time.deltaTime);
        }*/
        if (Input.GetButtonDown("Crouch")) 
        {
            //Vector3 slidezub = new Vector3(-slideDecay, -slideDecay, 0f);

            //Vector3 slide = move + slidezub;
            //velocity = new Vector3(slide.x, velocity.y, slide.z);
        }

        RaycastHit wall;
        Ray pumb = new Ray(feet.transform.position, transform.forward);
        Debug.DrawRay(feet.transform.position, transform.forward * touchDistance);
        if (Physics.Raycast(pumb, out wall, touchDistance))
        {
            //normal = wall.normal;
        }

        controller.Move(AbsReturn(move)); //moving //(move / Mathf.Abs(move))gives negative 1 or positive 1 based on earlier sign AbsReturn(move)

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravitationalConstant);
        }

        velocity.y += gravitationalConstant * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); //updates every frame
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody other = hit.collider.attachedRigidbody;

        if(other == null || other.isKinematic)
        {
            //normal = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        } else

        if (other != null) // || means and/ or
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            other.velocity = pushDir * pushPower;
        }
    }

    public Vector3 AbsReturn(Vector3 stupid)
    {
        //stupid = new Vector3(Mathf.Abs(stupid.x), Mathf.Abs(stupid.y), Mathf.Abs(stupid.z));
        float xcum = (Mathf.Abs(stupid.x) - normal.x); //normal should never > stupid
        float zcum = (Mathf.Abs(stupid.z) - normal.z);

        float x = xcum * speed * Time.deltaTime * (stupid.x / Mathf.Abs(stupid.x));
        float z = zcum * speed * Time.deltaTime * (stupid.z / Mathf.Abs(stupid.z));
        //float x = (Mathf.Abs(stupid.x) - normal.x) * speed * Time.deltaTime * (stupid.x / Mathf.Abs(stupid.x));
        //float z = (Mathf.Abs(stupid.z) - normal.z) * speed * Time.deltaTime * (stupid.z / Mathf.Abs(stupid.z));
        //float x = (stupid.x * speed * Time.deltaTime)-(normal.x/normalForce);
        //float z = (stupid.z * speed * Time.deltaTime)-(normal.z/normalForce);
        Vector3 smart = new Vector3(x, 0, z);
        return smart;
    }
}
