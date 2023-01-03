using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    public string PlayScene;

    private Animator myAnimator;

    public bool inOptionsMenu;

    [Header("Audio")]
    public Slider MasterVolumeSlider;
    public TMP_Text MasterVolumeRead;

    public Slider MusicVolumeSlider;
    public TMP_Text MusicVolumeRead;

    public Slider SFXVolumeSlider;
    public TMP_Text SFXVolumeRead;

    public int alterMusicBool = 1;
    public TMP_Text ConsiderReadMusic;

    [Header("Sensitivty")]
    public Slider XSensSlider;
    public TMP_Text XSensRead;
    public Slider YSensSlider;
    public TMP_Text YSensRead;

    public int alterSensBool = 0;
    public TMP_Text ConsiderRead;

    [Header("PrettyMenu")]

    public Button[] OptionButtons;

    public ColorBlock UnselectedOptionTabColorBlock;
    public ColorBlock SelectedOptionTabColorBlock;

    [Header("soudn")]
    public AudioSource Select1;
    public AudioSource Select2;

    //public AudioMixer theAudioMixer;

    private SceneManagerPlus theSceneManagerPlus;
    void Awake()
    {
        myAnimator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        theSceneManagerPlus = FindObjectOfType<SceneManagerPlus>();

        MasterVolumeSlider.value = PlayerPrefs.GetInt("UMasterVolume");
        MasterVolumeRead.SetText(MasterVolumeSlider.value + " dB");
        MusicVolumeSlider.value = PlayerPrefs.GetInt("UMusicVolume");
        MusicVolumeRead.SetText(MusicVolumeSlider.value + " dB");
        SFXVolumeSlider.value = PlayerPrefs.GetInt("USFXVolume");
        SFXVolumeRead.SetText(SFXVolumeSlider.value + " dB");

        XSensSlider.value = PlayerPrefs.GetInt("XSens");
        XSensRead.SetText(XSensSlider.value+ "");
        YSensSlider.value = PlayerPrefs.GetInt("YSens");
        YSensRead.SetText(YSensSlider.value+ "");

        alterMusicBool = PlayerPrefs.GetInt("AlterMusicBool");
        if (alterMusicBool == 1)
        {
            ConsiderReadMusic.SetText("YES");
        }
        else
        {
            ConsiderReadMusic.SetText("NO");
        }


        alterSensBool = PlayerPrefs.GetInt("AlterSensBool");
        if (alterSensBool == 1) 
        {
            ConsiderRead.SetText("YES");
        } else
        {
            ConsiderRead.SetText("NO");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToLoadOrSave()
    {
        Select1.Play();

        myAnimator.SetTrigger("ToLoadOrSave");
        theSceneManagerPlus.LowPassMaster(-40);
    }

    public void NewGame()
    {
        //save the ones you need (actual settings not items (audio settings etc
        int Mastervol = PlayerPrefs.GetInt("UMasterVolume");
        int Musicvol = PlayerPrefs.GetInt("UMusicVolume");
        int SFXvol = PlayerPrefs.GetInt("USFXVolume");
        int XS = PlayerPrefs.GetInt("XSens");
        int YS = PlayerPrefs.GetInt("YSens");
        int Con = PlayerPrefs.GetInt("AlterSensBool");
        int Mus = PlayerPrefs.GetInt("AlterMusicBool");

        //clear player prefs
        PlayerPrefs.DeleteAll();

        //restore settings
        PlayerPrefs.SetInt("UMasterVolume", Mastervol);
        PlayerPrefs.SetInt("UMusicVolume", Musicvol);
        PlayerPrefs.SetInt("USFXVolume", SFXvol);

        PlayerPrefs.SetInt("XSens", XS);
        PlayerPrefs.SetInt("YSens", YS);
        PlayerPrefs.SetInt("AlterSensBool", Con);
        PlayerPrefs.SetInt("AlterMusicBool", Mus);

        PlayerPrefs.SetInt("hardMode", 0);
        PlayerPrefs.SetInt("LoadSaveExists", 1);

        theSceneManagerPlus.ContainSceneLoadingInfo(PlayScene);
        SceneManager.LoadScene(1); //loadingscene
    }

    public void NewGameHard()
    {
        //save the ones you need (actual settings not items (audio settings etc
        int Mastervol = PlayerPrefs.GetInt("UMasterVolume");
        int Musicvol = PlayerPrefs.GetInt("UMusicVolume");
        int SFXvol = PlayerPrefs.GetInt("USFXVolume");
        int XS = PlayerPrefs.GetInt("XSens");
        int YS = PlayerPrefs.GetInt("YSens");
        int Con = PlayerPrefs.GetInt("AlterSensBool");
        int Mus = PlayerPrefs.GetInt("AlterMusicBool");

        //clear player prefs
        PlayerPrefs.DeleteAll();

        //restore settings
        PlayerPrefs.SetInt("UMasterVolume", Mastervol);
        PlayerPrefs.SetInt("UMusicVolume", Musicvol);
        PlayerPrefs.SetInt("USFXVolume", SFXvol);

        PlayerPrefs.SetInt("XSens", XS);
        PlayerPrefs.SetInt("YSens", YS);
        PlayerPrefs.SetInt("AlterSensBool", Con);
        PlayerPrefs.SetInt("AlterMusicBool", Mus);

        PlayerPrefs.SetInt("hardMode", 1);
        PlayerPrefs.SetInt("LoadSaveExists", 1);

        theSceneManagerPlus.ContainSceneLoadingInfo(PlayScene);
        SceneManager.LoadScene(1); //loadingscene
    }

    public void ToNewGameMenu()
    {
        Select2.Play();
        myAnimator.SetTrigger("ToNewGameMenu");

        //lowpass
        theSceneManagerPlus.LowPassMaster(0);
    }

    public void LoadGame()
    {
        theSceneManagerPlus.ContainSceneLoadingInfo(PlayScene);
        SceneManager.LoadScene(1);
    }

    public void Options() //default audio open
    {
        Select1.Play();
        myAnimator.SetTrigger("ToOptions");
        inOptionsMenu = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoBackToMainMenuStartState()
    {
        Select2.Play();
        myAnimator.SetTrigger("ToStartState");
        inOptionsMenu = false;
    }

    public void ToAudioOptions(Button meem)
    {
        Select1.Play();
        if (meem.colors == SelectedOptionTabColorBlock && inOptionsMenu) return;
        myAnimator.SetTrigger("ToOptions");
        meem.colors = SelectedOptionTabColorBlock;
        foreach(Button i in OptionButtons)
        {
            if (i == meem) continue;
            i.colors = UnselectedOptionTabColorBlock;
        }
        inOptionsMenu = true;
    }

    public void ToControlSettings(Button meem)
    {
        Select1.Play();
        if (meem.colors == SelectedOptionTabColorBlock && inOptionsMenu) return;
        myAnimator.SetTrigger("ToOptionsControl");
        meem.colors = SelectedOptionTabColorBlock;
        foreach (Button i in OptionButtons)
        {
            if (i == meem) continue;
            i.colors = UnselectedOptionTabColorBlock;
        }
        inOptionsMenu = true;
    }

    public void MasterVolume()
    {
        //theAudioMixer.SetFloat("MasterVolume", MasterVolumeSlider.value);
        theSceneManagerPlus.SetMasterVolume((int)MasterVolumeSlider.value);
        PlayerPrefs.SetInt("UMasterVolume", (int)MasterVolumeSlider.value);
        MasterVolumeRead.SetText(MasterVolumeSlider.value + " dB");
    }

    public void MusicVolume()
    {
        theSceneManagerPlus.SetMusicVolume((int)MusicVolumeSlider.value);
        PlayerPrefs.SetInt("UMusicVolume", (int)MusicVolumeSlider.value);
        MusicVolumeRead.SetText(MusicVolumeSlider.value + " dB");
    }

    public void SFXVolume()
    {
        theSceneManagerPlus.SetSFXVolume((int)SFXVolumeSlider.value);
        PlayerPrefs.SetInt("USFXVolume", (int)SFXVolumeSlider.value);
        SFXVolumeRead.SetText(SFXVolumeSlider.value + " dB");
    }

    public void XSensChange()
    {
        PlayerPrefs.SetInt("XSens", (int)XSensSlider.value);
        XSensRead.SetText(XSensSlider.value + "");
    }

    public void YSensChange()
    {
        PlayerPrefs.SetInt("YSens", (int)YSensSlider.value);
        YSensRead.SetText(YSensSlider.value + "");
    }

    public void ToggleShouldConsiderADS()
    {
        Select2.Play();
        if (alterSensBool == 1) {
            PlayerPrefs.SetInt("AlterSensBool", 0);
            ConsiderRead.SetText("NO");
            alterSensBool = 0;
        } else {
            PlayerPrefs.SetInt("AlterSensBool", 1);
            ConsiderRead.SetText("YES");
            alterSensBool = 1;
        }
    }

    public void ToggleMusic()
    {
        Select2.Play();
        if (alterMusicBool == 1)
        {
            PlayerPrefs.SetInt("AlterMusicBool", 0);
            ConsiderReadMusic.SetText("NO");
            alterMusicBool = 0;
        }
        else
        {
            PlayerPrefs.SetInt("AlterMusicBool", 1);
            ConsiderReadMusic.SetText("YES");
            alterMusicBool = 1;
        }
        theSceneManagerPlus.CheckMusic();
    }

    public void ResetToDefault() //resets settings
    {
        Select2.Play();

        theSceneManagerPlus.SetMasterVolume(8);
        PlayerPrefs.SetInt("UMasterVolume", 8);
        MasterVolumeSlider.value = PlayerPrefs.GetInt("UMasterVolume");
        MasterVolumeRead.SetText(MasterVolumeSlider.value + " dB");

        theSceneManagerPlus.SetMusicVolume(-10);
        PlayerPrefs.SetInt("UMusicVolume", -10);
        MusicVolumeSlider.value = PlayerPrefs.GetInt("UMusicVolume");
        MusicVolumeRead.SetText(MusicVolumeSlider.value + " dB");

        theSceneManagerPlus.SetSFXVolume(0);
        PlayerPrefs.SetInt("USFXVolume", 0);
        SFXVolumeSlider.value = PlayerPrefs.GetInt("USFXVolume");
        SFXVolumeRead.SetText(SFXVolumeSlider.value + " dB");

        PlayerPrefs.SetInt("XSens", 2);
        XSensSlider.value = PlayerPrefs.GetInt("XSens");
        XSensRead.SetText(XSensSlider.value + "");

        PlayerPrefs.SetInt("YSens", 2);
        YSensSlider.value = PlayerPrefs.GetInt("YSens");
        YSensRead.SetText(YSensSlider.value + "");

        PlayerPrefs.SetInt("AlterSensBool", 0);
        ConsiderRead.SetText("NO");
        alterSensBool = 0;

        alterMusicBool = 1;
        PlayerPrefs.SetInt("AlterMusicBool", 1);
        ConsiderReadMusic.SetText("YES");
        theSceneManagerPlus.CheckMusic();
    }

}
