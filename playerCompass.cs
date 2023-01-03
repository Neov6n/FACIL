using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCompass : MonoBehaviour
{

    public RawImage CompassImage;
    public newPlayerMovementScript thePlayer;
    private float targetX;
    public float compassSmoothing;
    //private float tolerance;

    // Update is called once per frame

    void Start()
    {
        thePlayer = FindObjectOfType<newPlayerMovementScript>();
        CompassImage = GetComponentInChildren<RawImage>();

        //tolerance = .0001f;
    }
    void Update()
    {
        //have it lerp to position and overadjust.
        
        targetX = (thePlayer.transform.localEulerAngles.y / 360);
        /*
        if(CompassImage.uvRect.x < 0f + tolerance)
        {
            Debug.Log("spwan");
            CompassImage.uvRect = new Rect(1, 0, 1, 1);
        }
        if(CompassImage.uvRect.x > 1f - tolerance)
        {
            CompassImage.uvRect = new Rect(0, 0, 1, 1);
        }
        CompassImage.uvRect = new Rect(Mathf.Lerp(CompassImage.uvRect.x, targetX, compassSmoothing), 0, 1, 1);
        */
        CompassImage.uvRect = new Rect(targetX + .223f, 0, 1, 1);

    }
}
