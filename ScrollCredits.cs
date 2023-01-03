using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollCredits : MonoBehaviour
{
    private SceneManagerPlus theSceneManagerPlus;
    private int creditsDone = 0;
    public float creditsLength;
    private float creditTicker;
    public GameObject credits;
    public float creditSpeed;
    // Start is called before the first frame update
    void Start()
    {
        creditTicker = creditsLength;
        theSceneManagerPlus = FindObjectOfType<SceneManagerPlus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(creditsDone == 0)
        {
            credits.transform.Translate(Vector3.up * Time.deltaTime * creditSpeed);
        }
        if(creditTicker > 0)
        {
            creditTicker -= Time.deltaTime;
        } else
        {
            creditsDone = 1;
        }


        if (creditsDone == 1)
        {
            theSceneManagerPlus.ContainSceneLoadingInfo("MainMenu"); //false
            SceneManager.LoadScene(1);
        }
    }
}
