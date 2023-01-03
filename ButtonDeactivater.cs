using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDeactivater : MonoBehaviour
{
    private Button me;
    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Button>();

        if(PlayerPrefs.GetInt("LoadSaveExists") == 0)
        {
            //deactiveate
            me.interactable = false;
        } else
        {
            //activate
            me.interactable = true;
        }
    }
}
