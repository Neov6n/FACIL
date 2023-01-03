using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFancy : MonoBehaviour
{
    public float timeToDestroy;
    private float ticker;
    public GameObject deathParticle;
    public GameObject doll;
    void Start()
    {
        ticker = timeToDestroy;
    }
    void Update()
    {
        if(ticker >= 0)
        {
            ticker -= Time.deltaTime;
        } else
        {
            Die();
        }
    }
    void Die()
    {
        GameObject dp = Instantiate(deathParticle, new Vector3(doll.transform.position.x, doll.transform.position.y, doll.transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
