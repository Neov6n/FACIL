using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    //to be used in tandem with weaponAnimator
    private WeaponAnimator theWeaponAnimator;
    private levelManager theLevelManager;
    private newPlayerMovementScript thePlayer;

    public float damage;
    public float range;
    public float impactForce;
    public LayerMask ThingsToShoot;

    public AudioSource shootSound;
    [SerializeField] private AudioClip[] shootClips;

    private bool fireChange;

    [HideInInspector] public Transform cameraObject;
    private Transform cameraHolder;
    private Transform NothingCamera;

    // Start is called before the first frame update
    void Start()
    {
        theWeaponAnimator = GetComponent<WeaponAnimator>();
        theLevelManager = FindObjectOfType<levelManager>();
        thePlayer = FindObjectOfType<newPlayerMovementScript>();

        cameraHolder = thePlayer.transform.Find("CameraHolder");
        NothingCamera = cameraHolder.transform.Find("NothingCamera");
        cameraObject = NothingCamera.transform.Find("CameraObject");
    }

    // Update is called once per frame
    void Update()
    {
        //see if weapon is firing
        //if so 
        if (theWeaponAnimator.isFiring)
        {
            if (fireChange)
            {
                Shoot(); //activate once
                fireChange = false;
            }
        } else
        {
            fireChange = true;
        }
    }

    void Shoot()
    {
        //play knife sound
        if (shootSound != null)
        {
            shootSound.PlayOneShot(shootClips[Random.Range(0, shootClips.Length - 1)]);
        }

        RaycastHit hit;
        if (Physics.Raycast(cameraObject.position, cameraObject.forward, out hit, range, ThingsToShoot, QueryTriggerInteraction.Ignore)) //layermask
        {
            //Debug.Log(hit.transform.name);

            TargetScript target = hit.transform.GetComponent<TargetScript>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            secondaryTarget turget = hit.transform.GetComponent<secondaryTarget>();
            if (turget != null)
            {
                turget.TakeDamage(damage);
            }
            ShootToRigidbody tonget = hit.transform.GetComponent<ShootToRigidbody>();
            if (tonget != null)
            {
                //Debug.Log("pi"); remember to assign shoottorigidbody script to object
                tonget.Flop();
            }


            if (hit.rigidbody != null)
            {
                //Debug.Log("bruh");
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            theLevelManager.ImpactEffects(hit);

        }
    }
}
