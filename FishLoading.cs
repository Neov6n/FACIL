using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishLoading : MonoBehaviour
{

    public Slider fami;
    //private Animator fish;
    private Image fishimage;
    private float fam = 100f;
    private float tick;
    public Sprite[] images;
    private int imageNumber;
    // Start is called before the first frame update
    void Start()
    {
        //fish = GetComponent<Animator>();
        fishimage = GetComponent<Image>();
        tick = fam;
        fishimage.sprite = images[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (tick <= 0)
        {
            //fish.SetFloat("much", fami.value);
            tick = fam;
            imageNumber += 1;
            if(imageNumber > images.Length - 1)
            {
                imageNumber = 0;
            }
            fishimage.sprite = images[imageNumber];

        }
        else
        {
            tick -= 1f;
        }
    }
}
