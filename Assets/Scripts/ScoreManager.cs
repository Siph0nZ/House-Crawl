using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public int score = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {   
        scoreText.text = "Paper: " + score.ToString();
    }

    public void AddScore()
    {
        score += 1;
        scoreText.text = "Paper: " + score.ToString();
    }
}
