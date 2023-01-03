using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    public levelManager theLevelManager;
    //public float distanceBetweenGrass;

    public int density;
    public GameObject grassObject;

    public Vector3 minScale;
    public Vector3 maxScale;

    public bool gra;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
        //chunkGrass = FindObjectsOfType<chunkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theLevelManager.infiniteDesert)
        {
            foreach (chunkScript i in theLevelManager.chunkDrawer)
            {
                if (!i.grassed)
                {
                    StartCoroutine(MakeGrass(i.gameObject.transform.position, theLevelManager.chunkSideLength, i.gameObject));
                    i.grassed = true;
                }
            }
        }
    }

    IEnumerator MakeGrass(Vector3 chunkPosition, float chunkSideLength, GameObject Chunk)
    {
        for(int i = 0; i < density; i++)
        {
            float sampleZ = Random.Range(chunkPosition.z - chunkSideLength, chunkPosition.z);
            float sampleX = Random.Range(chunkPosition.x, chunkPosition.x + chunkSideLength);
            Vector3 raystart = new Vector3(sampleX,50f,sampleZ);

            if (!Physics.Raycast(raystart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;
            if (hit.point.y < -50)
                continue;
            if (hit.transform.tag != "Chunk")
                continue;

            GameObject InstantiatedObject = Instantiate(grassObject, Chunk.transform);
            InstantiatedObject.transform.position = hit.point;
            //InstantiatedObject.transform.Rotate(Vector3.up, Random.Range(360, 360), Space.self);
            InstantiatedObject.transform.localScale = new Vector3(
                Random.Range(minScale.x,maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
                );
            yield return gra;
        }
    }
}
