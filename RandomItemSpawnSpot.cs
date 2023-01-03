using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawnSpot : MonoBehaviour
{
    public GameObject[] possibleItemSpawns;
    public Transform Structure;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject newWeapon = Instantiate(possibleItemSpawns[Random.Range(0, possibleItemSpawns.Length)], transform.position, Quaternion.identity, gameObject.transform);
        newWeapon.transform.localScale = new Vector3(newWeapon.transform.localScale.x / Structure.localScale.x, newWeapon.transform.localScale.y / Structure.localScale.y, newWeapon.transform.localScale.z / Structure.localScale.z);
    }

}
