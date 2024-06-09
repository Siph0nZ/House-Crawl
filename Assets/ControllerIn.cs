using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerIn : MonoBehaviour
{
    FadeInOut fade;
    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        fade.FadeIn();
    }
}
