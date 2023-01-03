using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{ 

    public Rigidbody playerbody;
    private Vector3 PlayerInput;
    public float speed = 12f;
    public float gravitationalConstant = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    //Vector3 bypass;
    //Vector3 bypass2;
    //public float stopConstant = 3f;
    //public float thresholdStop = 1f;

    public float jumpHeight = 3f;

    public float stoppingVar;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        PlayerInput = new Vector3(Input.GetAxis("Horizontal") * stoppingVar, 0f, Input.GetAxis("Vertical") * stoppingVar);

        MovePlayer();


        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            Debug.Log("cum");
            //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravitationalConstant);
            playerbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("Crouch")) 
        {
       
        }

        velocity.y += gravitationalConstant * Time.deltaTime;
    }

    void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerInput) * speed;
        playerbody.velocity = new Vector3(MoveVector.x, playerbody.velocity.y, MoveVector.z);
    }
}
