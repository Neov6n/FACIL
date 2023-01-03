using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Light MoonLight;
    [SerializeField] private LightingPreset Preset;

    [SerializeField, Range(0, 24)] public float TimeOfDay; //13.15 eclipse //ecplise moon v2 12.365      //12.3
    [SerializeField] private float timeSpeed = 0.04166667f;

    private Skybox theSky;

    private float moonTimer1 = 12;
    [SerializeField, Range(0, 12)] private float timeTillTick1; //3.72 eclipse //3.26   //3.22

    private float moonMultiplierX = -29.5f;
    private float moonMultiplierY = 0;
    private float moonMultiplierZ = 27.3f; //a small amount less than x

    public float moonSpeed = .001111111f;//.04167

    public AnimationCurve starAlpha;
    public MeshRenderer starRenderer; //cubemaptransparent shader

    private float starTimer = 12;
    [SerializeField, Range(0, 12)] private float timeTillStar;
    public float starSpeed = 0.02083334f; // same speed as sun
    private float starMultiplier = 30;

    private levelManager theLevelManager;

    public bool gameModePlay = true;
    private float timer = 10f;
    private float timertick = 1f;

    private void Start()
    {
        theLevelManager = FindObjectOfType<levelManager>();
        if (gameModePlay)
        {
            if(PlayerPrefs.GetFloat("TimeOfDay") == 0f)
            {
                TimeOfDay = 7f;
            } else
            {
                TimeOfDay = PlayerPrefs.GetFloat("TimeOfDay") + .01f;
            }
        }
    }

    private void Update()
    {
        if(timeTillTick1 < moonTimer1)
        {
            timeTillTick1 += 1 * Time.deltaTime * moonSpeed;
        } else
        {
            timeTillTick1 = 0f;
        }
        if (timeTillStar < starTimer)
        {
            timeTillStar += 1 * Time.deltaTime * starSpeed;
        }
        else
        {
            timeTillStar = 0f;
        }
        /*
        if (switchBox)
        {
            if (TimeOfDay > dawnTime && TimeOfDay < 24-dawnTime)
            {
                theSky.material = daySky;
            }
            else
            {
                theSky.material = nightSky;
            }
        }*/

        if (Preset == null)
            return;
        if (Application.isPlaying && !theLevelManager.isPaused)
        {
            TimeOfDay += timeSpeed * Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }

        if (gameModePlay)
        {

            if(timertick <= 0)
            {
                UpdatePrefTime();
                timertick = timer;
            } else
            {
                timertick -= 1 * Time.deltaTime;
                //Debug.Log(timertick);
            }

        }
    }

    public void UpdatePrefTime()
    {
        PlayerPrefs.SetFloat("TimeOfDay", TimeOfDay);
        //Debug.Log(PlayerPrefs.GetFloat("TimeOfDay"));
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170, 0));
        }
        if (MoonLight != null)
        {
            //MoonLight.color = MoonPreset.DirectionalColor.Evaluate(timePercent);
            MoonLight.transform.localRotation = Quaternion.Euler(new Vector3(timeTillTick1 * moonMultiplierX, timeTillTick1* moonMultiplierY, (timeTillTick1 * moonMultiplierZ))); //x is around same as z
            if(MoonLight.transform.localRotation.y >= 360f)
            {
                MoonLight.transform.localRotation = Quaternion.Euler(new Vector3(MoonLight.transform.localRotation.x, 0f, MoonLight.transform.localRotation.z));
            }
        }
        if (starRenderer != null)
        {
            starRenderer.sharedMaterial.SetFloat("_Rotation", timeTillStar * starMultiplier);
            if (starRenderer.sharedMaterial.GetFloat("_Rotation") == 360f)
            {
                starRenderer.sharedMaterial.SetFloat("_Rotation", 0);
            }
            
            starRenderer.sharedMaterial.SetFloat("_Alpha", starAlpha.Evaluate(timePercent));
            //Debug.Log(starMaterial.GetFloat("_Alpha"));
        }

    }

    private void OnValidate()
    {

        if (DirectionalLight != null)
            return;

        if(RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light i in lights)
            {
                if(i.type == LightType.Directional)
                {
                    //DirectionalLight = i;
                    return;
                }
            }
        }
    }
}
