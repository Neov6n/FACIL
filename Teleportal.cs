using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportal : MonoBehaviour
{

    //[SerializeField] private KeyCode InteractKey = KeyCode.E;

    [SerializeField] private bool playerInVicinity = false;

    private levelManager theLevelManager;

    public string endScene;

    [TextArea] public string pressText;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theLevelManager.intTimeHeld ==1 && playerInVicinity)
        {
            SceneManager.LoadScene(endScene);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "player")
        {
            playerInVicinity = true;
            theLevelManager.UpdateInteractText(pressText);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            playerInVicinity = false;
            theLevelManager.UpdateInteractText("");
        }
    }
}
