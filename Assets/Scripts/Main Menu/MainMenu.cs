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

    public void PlayGame()
    {
        StartCoroutine(ChangeScene());
    }

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

    public IEnumerator ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level 1");
    }
}
