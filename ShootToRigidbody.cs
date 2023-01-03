using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToRigidbody : MonoBehaviour
{
    private Rigidbody myRigidbody;
    //private DamagePlayer damager;
    public Collider hurtbox;
    public AudioSource moveSound;
    public AudioSource landSound;
    private AmbientSound ambSoundScript;
    private bool activated;
    private bool landed;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        ambSoundScript = GetComponent<AmbientSound>();
        //damager = GetComponent<DamagePlayer>();
        activated = false;
        landed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flop()
    {
        //myRigidbody.SetActive(true);
        if(!activated)
        {
            Fall();
        }
        //play sound?
    }
    private void Fall()
    {
        myRigidbody.constraints = RigidbodyConstraints.None;
        if (hurtbox != null)
        {
            hurtbox.enabled = true;
        }
        if (moveSound != null)
        {
            moveSound.Play();
        }
        if (ambSoundScript != null)
        {
            ambSoundScript.enabled = false;
        }
        //if(damager != null)
        //{
           // damager.canDamage = true;
        //}
        activated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (landed == false)
        {
            if (other.tag == "Chunk")
            {
                if (landSound != null)
                {
                    landSound.Play();
                }
                if (hurtbox != null)
                {
                    hurtbox.enabled = false;
                }
                //if (damager != null)
                //{/
                    //damager.canDamage = false;
                //}
                landed = true;
            }
        }
    }
}
