using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightWolfSpawner : MonoBehaviour
{
    private newPlayerMovementScript thePlayer;
    private levelManager theLevelManager;
    private LightingManager theLightingManager;
    public float radius;
    public GameObject wolfPrefab;
    public bool shouldSpawn;
    public float delay;
    private float dlayticker;
    //call for lightmanager?
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
        theLevelManager = FindObjectOfType<levelManager>();
        theLightingManager = FindObjectOfType<LightingManager>();
        dlayticker = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!theLevelManager.isPaused)
        {
            if (shouldSpawn)
            {
                if (dlayticker > 0)
                {
                    //dlayticker--;
                    dlayticker = dlayticker - Time.deltaTime;
                }
                else
                {
                    SpawnWolf();
                    dlayticker = delay;
                }
            }
        }

        if(theLightingManager.TimeOfDay >= 6f && theLightingManager.TimeOfDay <= 18f)
        {
            shouldSpawn = false; //daytime
        } else
        {
            shouldSpawn = true; //nighttime
        }
    }

    void SpawnWolf()
    {
        Instantiate(wolfPrefab, RandomPositionInCircle(), Quaternion.identity); //, gameObject.transform
        //Debug.Log(RandomPositionInCircle());
    }

    Vector3 RandomPositionInCircle()
    {
        float xPosition;
        float zPosition;

        xPosition = Random.Range(-radius, radius); //this is inclusive 
        zPosition = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(xPosition, 2));

        int PosOrNeg = Random.Range(0, 2);
        if (PosOrNeg == 0) //neg
        {
            zPosition = -zPosition;
        }

        Vector3 randomSpawnPosition = new Vector3(xPosition, 1, zPosition);
        randomSpawnPosition += thePlayer.transform.position;
        return randomSpawnPosition;
    }
}
