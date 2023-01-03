using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowLuckFish : MonoBehaviour
{
    public levelManager theLevelManager;
    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        theLevelManager.randomTicker = 1;
    }
}
