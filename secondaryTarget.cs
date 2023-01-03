using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondaryTarget : MonoBehaviour
{
    public float health = 100f;
    private float currentHealth;
    public GameObject deathParticle;
    private TargetScript parent;

    void Start()
    {
        currentHealth = health;
        parent = GetComponentInParent<TargetScript>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

        //Debug.Log(currentHealth);
    }

    void Die()
    {
        GameObject dp = Instantiate(deathParticle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        Destroy(parent.gameObject);
    }
}
