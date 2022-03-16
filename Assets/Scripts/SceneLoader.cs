using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// FADE INTO BLACK SMOOTHLY WHEN TRANSITIONING BETWEEN SCENES (MUST BE ON AN OBJECT IN EVERY SCENE)

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public static SceneLoader instance;
    public float transitionTime;

    // CREATE INSTANCE ON AWAKE SO IT CAN BE ACCESSED BY OTHER SCRIPTS WITHOUT REFERENCE
    private void Awake()
    {
        instance = this;
    }

    // CALLS THE LOADER BY SCENE NAME
    public void Load(string sceneName)
    {
        StartCoroutine(LoaderByName(sceneName));
    }

    public void Exit()
    {
        StartCoroutine(ExitLoader());
    }

    // CALLS THE LOADER FOR THE NEXT SCENE IN THE SCENE BUILD INDEX ORDER
    public void LoadNextScene()
    {
        StartCoroutine(LoaderByIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    // LOAD SCENE BY ITS NAME
    private IEnumerator LoaderByName(string sceneName)
    {
        transition.SetTrigger("SceneChange");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    // LOAD NEXT SCENE
    private IEnumerator LoaderByIndex(int sceneIndex)
    {
        transition.SetTrigger("SceneChange");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator ExitLoader()
    {
        transition.SetTrigger("SceneChange");
        yield return new WaitForSeconds(transitionTime);
        Application.Quit();
    }
}
