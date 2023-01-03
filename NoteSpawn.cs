using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawn : MonoBehaviour
{
    public GameObject[] NoteItems;
    public Transform Structure;
    private int before;
    private float ticker;
    public GameObject newWeapon;
    public bool playerHasBeenHere = false;
    private Collider sphere;

    // Start is called before the first frame update
    void OnEnable()
    {
        sphere = GetComponent<Collider>();
        int kan = PlayerPrefs.GetInt("NoteNumber");
        newWeapon = Instantiate(NoteItems[kan], transform.position, Quaternion.identity, gameObject.transform);
        newWeapon.transform.localScale = new Vector3(newWeapon.transform.localScale.x / Structure.localScale.x, newWeapon.transform.localScale.y / Structure.localScale.y, newWeapon.transform.localScale.z / Structure.localScale.z);
        //PlayerPrefs.SetInt("NoteNumber", kan + 1);
    }
    void Update()
    {
        if (!playerHasBeenHere)
        {
            if (ticker <= 0)
            {
                if (PlayerPrefs.GetInt("NoteNumber") != before)
                {
                    Destroy(newWeapon);
                    newWeapon = Instantiate(NoteItems[PlayerPrefs.GetInt("NoteNumber")], transform.position, Quaternion.identity, gameObject.transform);
                    newWeapon.transform.localScale = new Vector3(newWeapon.transform.localScale.x / Structure.localScale.x, newWeapon.transform.localScale.y / Structure.localScale.y, newWeapon.transform.localScale.z / Structure.localScale.z);
                }
                before = PlayerPrefs.GetInt("NoteNumber");
                ticker = 4;
            }
            else
            {
                ticker -= Time.deltaTime;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            playerHasBeenHere = true;
        }
    }
}
