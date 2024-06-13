using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    FadeInOut fade;

    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }

    // Help Scene
    public void Help()
    {
        StartCoroutine(ChangeSceneHelp());
    }

    public IEnumerator ChangeSceneHelp()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("HelpScene");
    }

    // Level 1 Scene 
    public void PlayGame()
    {
        StartCoroutine(ChangeScene());
    }

    public IEnumerator ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene("Level 1");
    }
}
