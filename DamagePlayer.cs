using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private levelManager theLevelManager;
    public float delayTime;
    private float hurtticker;
    public float damageAmount;
    public bool canDamage = true;
    public bool isAttacking;
    private float attackTicker;

    public Animator Coyote;
    public NightWolf nW;
    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTicker >= 0)
        {
            isAttacking = true;
            if(nW != null) nW.shouldChase = false;
            attackTicker -= Time.deltaTime;
        } else
        {
            isAttacking = false;
            if (nW != null) nW.shouldChase = true;
        }
        if(Coyote != null) Coyote.SetBool("isAttacking", isAttacking);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "player")
        {
            if (canDamage)
            {
                if (hurtticker > 0)
                {
                    hurtticker = hurtticker - Time.deltaTime;
                }
                else
                {
                    theLevelManager.PlayerTakeDamage(damageAmount);
                    hurtticker = delayTime;
                }
            }
            attackTicker = 2f;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            if (canDamage)
            {
                hurtticker = 0;
            }
        }
    }
}
