using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] private AudioSource AmbientSoundSource;
    [SerializeField] private AudioClip[] ambClips;
    public float timer;
    public float deviation;
    private float ticker;
    private int lastClip;
    // Start is called before the first frame update
    void Start()
    {
        ticker = timer;
        lastClip = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ticker <= 0)
        {
            int clipToPlay = Random.Range(0, ambClips.Length - 1);
            if(clipToPlay == lastClip)
            {
                clipToPlay += 1;
            }
            if(clipToPlay > 24)
            {
                clipToPlay = 1;
            }
            //AmbientSoundSource.PlayOneShot(ambClips[clipToPlay]);
            if (!AmbientSoundSource.isPlaying)
            {
                AmbientSoundSource.clip = ambClips[clipToPlay];
                AmbientSoundSource.Play();
            }
            lastClip = clipToPlay;
            ticker = Random.Range(timer - deviation, timer + deviation);
        } else
        {
            ticker -= Time.deltaTime;
        }
    }

    void OnDisable()
    {
        AmbientSoundSource.Stop();
    }
}
