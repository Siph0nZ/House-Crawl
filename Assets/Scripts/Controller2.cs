using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2 : MonoBehaviour
{   
    FadeInOut fade;

    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        fade.FadeIn();
    }
}
