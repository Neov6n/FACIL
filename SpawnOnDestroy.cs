using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public GameObject protect;
    public GameObject wolfPrefab;
    private float ticker;

    // Update is called once per frame
    void Update()
    {
        if (protect == null)
        {
            if (ticker > 0f)
            {
                ticker -= Time.deltaTime;
            }
            else
            {
                Instantiate(wolfPrefab, gameObject.transform.position, Quaternion.identity);
                PlayerPrefs.SetInt("hardMode", 1);
                ticker = 2f;
            }
        }
    }
}
