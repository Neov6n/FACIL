using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissappearOnContact : MonoBehaviour
{
    public Collider contac;
    public SpriteRenderer rend;
    public Color lerpedColor;
    public bool fade = false;
    public float deathTimer = 5f;
    private float deathTicker = 12f;
    private LightingManager theLightingManager;

    // Start is called before the first frame update
    void Start()
    {
        theLightingManager = FindObjectOfType<LightingManager>();
        contac = GetComponent<Collider>();
        rend = GetComponent<SpriteRenderer>();

        if (theLightingManager.TimeOfDay <= 6f && theLightingManager.TimeOfDay >= 18f)
        {
            Destroy(gameObject);//nighttime
        }
    }

    // Update is called once per frame
    void Update()
    {
        rend.color = lerpedColor;
        if (fade)
        {
            float fadeAmount = lerpedColor.a - Time.deltaTime;
            lerpedColor = new Color(lerpedColor.r, lerpedColor.g, lerpedColor.b, fadeAmount);
        }
        if(deathTicker > 0 && deathTicker < 11)
        {
            deathTicker -= Time.deltaTime;
        } else if(deathTicker <= 0f)
        {
            Destroy(gameObject);
        }
        if (theLightingManager.TimeOfDay <= 6f && theLightingManager.TimeOfDay >= 18f)
        {
            fade = true;
            deathTicker = deathTimer;//nighttime
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            fade = true;
            deathTicker = deathTimer;
        }
    }
}
