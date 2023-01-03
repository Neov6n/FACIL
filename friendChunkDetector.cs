using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class friendChunkDetector : MonoBehaviour
{
    public chunkScript dad;
    public float radius = 128;//227 for full encapsulation
    // Start is called before the first frame update
    void Start()
    {
        dad = gameObject.GetComponentInParent<chunkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dad.iAmCurrentChunk) //only activates if i am child of current
        {
            Collider[] surroundingObjects = Physics.OverlapSphere(gameObject.transform.position + new Vector3(64, 0 ,-64), radius);
            foreach (var hitCollider in surroundingObjects) //foreach thing in sphere
            {

                chunkScript collideChunk = hitCollider.GetComponent<chunkScript>(); 
                if (collideChunk != null && gameObject.GetComponentInParent<chunkScript>() != collideChunk) //if that thing has a chunkscript and is not my parent
                {
                    //Debug.Log("huh");
                    collideChunk.iAmAFriendOfCurrent = true;
                    collideChunk.friendshipCounter = collideChunk.friendshipCounter + 1 * Time.deltaTime; //2this is an elastic switch nubmer2
                    //add to surrounding chunk array // no longer using an array, surrounding is identified by friendchunk variable in chunkscript
                } 
            }
        }
    }

    /*void OnTriggerStay(Collider other)
    {

        if (dad.iAmCurrentChunk) // and if other is not parent)
        {

            chunkScript collideChunk = other.GetComponent<chunkScript>();
            if (collideChunk != null)
            {
                Debug.Log("huh");
                collideChunk.iAmAFriendOfCurrent = true;
                //add to surrounding chunk array
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Chunk")
        {
            if (dad.iAmCurrentChunk)
            {
                chunkScript collideChunk = other.GetComponent<chunkScript>();
                collideChunk.iAmAFriendOfCurrent = false;
                //remove their status as surrounding chunk
            }
        }
    }*/
}
