using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{   
    FadeInOut fade;

    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        fade.FadeOut();
    }
}
