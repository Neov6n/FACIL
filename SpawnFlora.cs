using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlora : MonoBehaviour
{
    public levelManager theLevelManager;

    public float chunkSideLength = 128;

    public GameObject[] veggies;

    private int randomIndex;
    private Vector3 randomSpawnPosition;

    public int amountOfPlantsToSpawn=1;

    public bool structureSpawn;

    private int vegcounter;

    private Quaternion randomRotation;

    [Header("grass")]

    public GameObject grass;
    private int vegcounterG;
    private Vector3 randomSpawnPositionG;
    //private Quaternion randomRotationG;
    public int amountOfGrassToSpawn = 1;

    //also spawns more than just flora

    //[Header("Other")]

    //public int[] randomnessForOther;
    //public GameObject[] otherObject;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType < levelManager>();

        vegcounter = amountOfPlantsToSpawn;
        vegcounterG = amountOfGrassToSpawn;

    }

    void Update()
    {
        if (theLevelManager.infiniteDesert)
        {
            if (vegcounter != 0)
            {
                randomIndex = Random.Range(0, veggies.Length);
                randomSpawnPosition = new Vector3(Random.Range(gameObject.transform.position.x, gameObject.transform.position.x + chunkSideLength), 1, Random.Range(gameObject.transform.position.z - chunkSideLength, gameObject.transform.position.z));
                randomRotation = Quaternion.Euler(veggies[randomIndex].transform.rotation.x, Random.Range(0, 361), veggies[randomIndex].transform.rotation.y);
                Instantiate(veggies[randomIndex], randomSpawnPosition, randomRotation, gameObject.transform);
                vegcounter -= 1;
            }
        }
        if (theLevelManager.dynamicDesert)
        {
            if (vegcounter != 0)
            {
                randomIndex = Random.Range(0, veggies.Length);
                randomSpawnPosition = new Vector3(Random.Range(gameObject.transform.position.x- chunkSideLength/2, gameObject.transform.position.x + chunkSideLength/2), 1, Random.Range(gameObject.transform.position.z - chunkSideLength/2, gameObject.transform.position.z+chunkSideLength/2));
                randomRotation = Quaternion.Euler(veggies[randomIndex].transform.rotation.x, Random.Range(0, 361), veggies[randomIndex].transform.rotation.y);
                Instantiate(veggies[randomIndex], randomSpawnPosition, randomRotation, gameObject.transform);
                vegcounter -= 1;
            }
            if (vegcounterG != 0)
            {
                randomSpawnPositionG = new Vector3(Random.Range(gameObject.transform.position.x- chunkSideLength/2, gameObject.transform.position.x + chunkSideLength/2), 1, Random.Range(gameObject.transform.position.z - chunkSideLength/2, gameObject.transform.position.z+chunkSideLength/2));
                //randomRotationG = Quaternion.Euler(grass.transform.rotation.x, Random.Range(0, 361), grass.transform.rotation.z);
                Instantiate(grass, randomSpawnPositionG, Quaternion.identity, gameObject.transform);
                vegcounterG -= 1;
            }
        }

        if (structureSpawn)
        {
            StartCoroutine(SpawnStructures());
            structureSpawn = false;
        }
    }

    IEnumerator SpawnStructures()
    {
        int x = 0;
        foreach (GameObject poo in theLevelManager.StructureObjects)
        {
            //Debug.Log(PlayerPrefs.GetInt("finalKey"));
            //if (!theLevelManager.finalKey && x == 1)
            if (PlayerPrefs.GetInt("finalKey") == 0 && x == 1) //this asks if there is no final key for the object and if so continues to the next loop without spawning object
            {
                //Debug.Log("skip");
                x++;
                continue; //maybe break later
            }
            if (theLevelManager.prevSpawned[x] == 0) //not yet spawned
            {
                int blool = Random.Range(0, theLevelManager.randomnessForStructs[x]* (int)theLevelManager.randomMultiplier);
                if (blool == 0)
                {
                    SpawnOther(poo);
                    theLevelManager.prevSpawned[x] = 1;
                    //Debug.Log("Spawned"+poo.name);
                    break;
                }
            }
            else if (theLevelManager.prevSpawned[x] == 2) // can be spawned anytime
            {
                int blool = Random.Range(0, (int)theLevelManager.randomnessForStructs[x] * (int)theLevelManager.randomMultiplier);
                if (blool == 0)
                {
                    SpawnOther(poo);
                    break;
                }
            }
            x++;
        }

        yield return new WaitForEndOfFrame();
    }

    void SpawnOther(GameObject otherObject1)
    {
        randomSpawnPosition = new Vector3(Random.Range(gameObject.transform.position.x, gameObject.transform.position.x + chunkSideLength), 1, Random.Range(gameObject.transform.position.z - chunkSideLength, gameObject.transform.position.z));
        Instantiate(otherObject1, randomSpawnPosition, Quaternion.identity, gameObject.transform); //make sure to make this a child so that is deleted once more land is created
    }
}
