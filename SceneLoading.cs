using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private SceneManagerPlus theSceneManagerPlus;

    // Start is called before the first frame update
    void Start()
    {
        theSceneManagerPlus = FindObjectOfType<SceneManagerPlus>();
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation nextLevel = SceneManager.LoadSceneAsync(theSceneManagerPlus.sceneToLoad);
        nextLevel.allowSceneActivation = false;
        while (!nextLevel.isDone)
        {
            //while (nextLevel.progress < 0.9f)//!nextLevel.isDone)//nextLevel.progress < 1)
            //{
                //progressBar.value = nextLevel.progress;
                //yield return new WaitForEndOfFrame();
            //}
            progressBar.value = nextLevel.progress;
            if (nextLevel.progress >= 0.9f)
            {
                nextLevel.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
