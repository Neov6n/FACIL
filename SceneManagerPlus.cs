using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

//[ExecuteAlways] best script in the game 10/10
public class SceneManagerPlus : MonoBehaviour
{
    public bool playGameMusic;
    public bool loopGameMusic;
    public AudioSource gameMusic; //ambiance
    [Header("Pitches and Bitches")]
    public float[] pitches; //and bitches

    private bool haa = false;
    [HideInInspector] public int randomness = 100;
    private levelManager theLevelManager;

    //public string scenee;
    public AudioSource menuMusic;

    public string sceneToLoad;

    public Scene oldScene;

    [Header("Audio")]
    public AudioMixer theAudioMixer;

    [Header("BackMusic")]
    public AudioSource backMusic; //should not be looped
    public bool shouldPlayBack;
    public AudioClip[] backClips;
    private int lastClip;
    public int PlayerControlPlayBackMusic = 1;
    public bool LerpMasterVolumeRunning = false;

    // Start is called before the first frame update
    void Awake()
    {
        SceneManagerPlus[] objs = FindObjectsOfType<SceneManagerPlus>();

        if (objs.Length > 1)
        {
            Debug.Log("deleting extra scene manager");
            Destroy(gameObject, 0f);
            return;
        }

        DontDestroyOnLoad(gameObject); // since this is a do not destroy object, getcomponent each component you need before you call it

        if (playGameMusic)
        {
            //weight the randomness-------------------------------------------------------------------------------------------------------
            int die = Random.Range(0, randomness);
            float die2 = (float)die;

            //Debug.Log(99 % 99); the remainder equals 0

            if (die2 % 99 == 0f) //if die2 is divided by 99 and the remainder is 0, then perform following script (1/100 chance)
            {
                gameMusic.pitch = pitches[1];
                gameMusic.Play();
                return;
                //break return and continue are all called jump statements;
                //for foreach while and do are all iteration statements;
            }
            if (die2 > 50) // this code can only be activated if the above if statement is false, so no need to further clarify
            {
                gameMusic.pitch = pitches[0]; //1/2 chance
                gameMusic.Play();
                return;
            }
            gameMusic.pitch = pitches[Random.Range(2, pitches.Length)];
            //randomize pitch randomly ---------------------------------------------------------------------------------------------------

            //gameMusic.pitch = pitches[Random.Range(0,pitches.Length)];

            if (gameMusic.pitch < 0 && !loopGameMusic) // because its backwards
            {
                gameMusic.loop = true; // change this later(looping) when you dont want it to
                loopGameMusic = true;
                gameMusic.Play();
                //gameMusic.loop = false;
                //loopGameMusic = false;
                return;
            }

            //play music(if code hasn't been broken out of already-------------------------------------------------------------------------
            gameMusic.Play();

        }
    }

    void Start()
    {

    }
    /// <summary>
    /// See, there's this really cool thing called OnEnable() that I didn't really look into until now, and it would've likely been helpful if I k
    /// new about it earlier for several applications,
    /// It allows a piece of code to be called only one time everytime an object is enabled which is extremely helpful because you don't have to make 5 extra bools o
    /// r a timer so that piece of code only goes of once I guess you can just use a coroutine but if you put call that coroutine in the update function then you still have to
    /// make another bool to make sure it doesn't fire multiple times its just easier you know I could just have an object activate and everytime it activates its code plays
    /// exactly once well I guess now I know for next time
    /// I think this is a great way to optomize code that I should look into more often in future projects when I am trying to activate a single line of code once because
    /// it removes the clutter of extra unneccessary variables that even feel unneccessary in their nature
    /// </summary>
        //called every time enabled?
    void OnEnable() // there should be no reason to disable this object
    {
        if (haa)
        {
            Debug.Log("thisobject has been activated(scene manager-");
            Debug.Log("also why are you disactivating and reactivating the scene manager stop that");
            //SceneManager.LoadScene(scenee);
        }
        haa = true;
    }
    // Update is called once per frame
    void Update() // this is called a function
    {
        if (PlayerControlPlayBackMusic ==1 )
        {
            if (backMusic.mute == true)
            {
                backMusic.mute = false;
                menuMusic.mute = false;
            }
            if (shouldPlayBack)
            {
                PlayBackMusic();
            }
        }
        else
        {
            if (backMusic.mute == false)
            {
                backMusic.mute = true;
                menuMusic.mute = true;
            }
        }

        if (loopGameMusic)
        {
            gameMusic.loop = true;
        }
        else
        {
            gameMusic.loop = false;
        }
        //SceneManager.sceneLoaded += OnSceneLoaded; // this doesnt work and i dont know how to make it work and i dont need to make it work therefore it will not work

        Scene scene = SceneManager.GetActiveScene();

        if(oldScene != scene)
        {
            OnSceneLoadedY(scene);
        }

        oldScene = scene;
    }
    public void ContainSceneLoadingInfo(string scene)
    {
        sceneToLoad = scene;
    }

    public void OnSceneLoadedY(Scene scene)
    {
        FindLevelManager();
        //Debug.Log("uhuh");
        if(scene.buildIndex == 0)
        {
            StopAllCoroutines();
            theAudioMixer.SetFloat("WetLowPassMaster", -80);
            gameMusic.Stop();
            backMusic.Stop();
            PlayerControlPlayBackMusic = PlayerPrefs.GetInt("AlterMusicBool");// if options become available in game, move this
            shouldPlayBack = false;
            SetMasterVolume(PlayerPrefs.GetInt("UMasterVolume"));
            SetMusicVolume(PlayerPrefs.GetInt("UMusicVolume"));
            menuMusic.Play();
            
        }
        if(scene.buildIndex == 1)
        {
            StartCoroutine(LerpMasterVolume(-40));
        }
        if(scene.buildIndex == 2)
        {
            StopAllCoroutines(); // only works for the ones on this script?
            theAudioMixer.SetFloat("WetLowPassMaster", -80);
            menuMusic.Stop();
            SetMasterVolume(PlayerPrefs.GetInt("UMasterVolume"));
            SetMusicVolume(PlayerPrefs.GetInt("UMusicVolume"));
            gameMusic.Play();
            PlayerControlPlayBackMusic = PlayerPrefs.GetInt("AlterMusicBool"); // if options become available in game, move this
            shouldPlayBack = true;
        }
        if(scene.buildIndex == 3)
        {
            StopAllCoroutines();
            theAudioMixer.SetFloat("WetLowPassMaster", -80);
            menuMusic.Stop();
            gameMusic.Stop();
            backMusic.Stop();
            SetMasterVolume(PlayerPrefs.GetInt("UMasterVolume"));
            SetMusicVolume(PlayerPrefs.GetInt("UMusicVolume"));
            PlayerControlPlayBackMusic = PlayerPrefs.GetInt("AlterMusicBool");
            shouldPlayBack = false;
            backMusic.clip = backClips[2];
            backMusic.Play();
        }
        if(scene.buildIndex == 4)
        {
            StopAllCoroutines();
            theAudioMixer.SetFloat("WetLowPassMaster", -80);
            menuMusic.Stop();
            gameMusic.Stop();
            backMusic.Stop();
            SetMasterVolume(PlayerPrefs.GetInt("UMasterVolume"));
            SetMusicVolume(PlayerPrefs.GetInt("UMusicVolume"));
            PlayerControlPlayBackMusic = PlayerPrefs.GetInt("AlterMusicBool");
            shouldPlayBack = false;
            backMusic.clip = backClips[1];
            backMusic.Play();
        }
    }
    public void PlayBackMusic()
    {
        if (backMusic.isPlaying == false)
        {
            int clipToPlay = Random.Range(0, backClips.Length - 1);
            if (clipToPlay == lastClip)
            {
                clipToPlay += 1;
            }
            if (clipToPlay > backClips.Length)
            {
                clipToPlay = 1;
            }

            backMusic.clip = backClips[clipToPlay];

            lastClip = clipToPlay;
            backMusic.Play();

        }
    }

    public IEnumerator LerpMusicVolume(int volume)
    {
        float chung;
        theAudioMixer.GetFloat("MusicVolume", out chung);

        while (chung > (volume + 1) || chung < (volume - 1))
        {
            theAudioMixer.GetFloat("MusicVolume", out chung);

            theAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(chung, volume, 8f * Time.deltaTime));
            yield return new WaitForEndOfFrame();

        }
    }

    public IEnumerator LerpMasterVolume(int volume) //for load scene
    {
        LerpMasterVolumeRunning = true;
        //Debug.Log("flog");
        float chung;
        theAudioMixer.GetFloat("MasterVolume", out chung);

        while (chung > (volume + 1) || chung < (volume - 1))
        {
            theAudioMixer.GetFloat("MasterVolume", out chung);

            theAudioMixer.SetFloat("MasterVolume", Mathf.Lerp(chung, volume, 8f * Time.deltaTime));
            yield return new WaitForEndOfFrame();

        }
        LerpMasterVolumeRunning = false;
    }

    public void SetMasterVolume(int volume)
    {
        theAudioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(int volume)
    {
        theAudioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(int volume)
    {
        theAudioMixer.SetFloat("SFXVolume", volume);
    }

    public void FindLevelManager()//this is called a method
    {
        theLevelManager = FindObjectOfType<levelManager>();
        if (theLevelManager == null)
        {
            //Debug.Log("You didnd do it!");

        }
    }

    public void LowPassMaster(int amount)
    {
        //theAudioMixer.SetFloat("WetLowPassMaster", amount);
        StartCoroutine(LerpLowPassMaster(amount));
    }

    IEnumerator LerpLowPassMaster(int amount) //-40 to 0
    {
        //Debug.Log("ve");
        float chung;
        theAudioMixer.GetFloat("WetLowPassMaster", out chung);

        while (chung > (amount + 1) || chung < (amount - 1))
        {
            //Debug.Log(chung);
            theAudioMixer.GetFloat("WetLowPassMaster", out chung);

            theAudioMixer.SetFloat("WetLowPassMaster", Mathf.Lerp(chung, amount, 8f * Time.deltaTime));
            yield return new WaitForEndOfFrame();

        }

    }

    public void CheckMusic()
    {
        PlayerControlPlayBackMusic = PlayerPrefs.GetInt("AlterMusicBool");
    }
}
