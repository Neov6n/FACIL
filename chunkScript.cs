using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkScript : MonoBehaviour
{
    public bool iAmCurrentChunk;
    public bool iAmAFriendOfCurrent;
    public float chunkScriptCounter;
    //public friendChunkDetector detec;

    private float lastCheckedValue;
    public float friendshipCounter;

    public bool grassed = false;
    private levelManager theLevelManager;

    public bool iHavePlayer;
    public bool exception;

    public float sideLength = 128;
    // Start is called before the first frame update
    void Start()
    {
        //detec = gameObject.GetComponentInChildren<friendChunkDetector>();
        chunkScriptCounter = 0;
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] playerDetectionCube = Physics.OverlapBox(gameObject.transform.position + new Vector3(sideLength/2, 0, -sideLength / 2), (sideLength / 2)* transform.localScale); //half of 128
        foreach (var other in playerDetectionCube)
        {
            if (other.tag == "player")
            {
                iHavePlayer = true;
                if (gameObject.transform.position != theLevelManager.oldCurrentChunk && theLevelManager.howManyPlayerHaves > 1 && !exception) {
                    break;
                }
                else {
                iAmCurrentChunk = true;
                iAmAFriendOfCurrent = true;
                //the center chunk should both be a current and friend chunk so that it does not get deleted.
                chunkScriptCounter = chunkScriptCounter + 1* Time.deltaTime; //this is an elastic switch
                                                                             //chunkScriptCounter = Mathf.Sin(chunkScriptCounter);
                    break;
                }
            } else
            {
                iHavePlayer = false;
            }
        }
        if (!iAmCurrentChunk)
        {
            if (friendshipCounter == lastCheckedValue) // 2 elastic swithc number 2for friensship script
            {
                iAmAFriendOfCurrent = false;
            }
            lastCheckedValue = friendshipCounter;
        } else
        {
            exception = false;
        }
    }

    /*void OnTriggerStay(Collider other)
    {
        if(other.tag == "player")
        {
            iAmCurrentChunk = true;
            iAmAFriendOfCurrent = true;
        } 
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "player")
        {
            iAmCurrentChunk = false;
        }
    }*/
}
