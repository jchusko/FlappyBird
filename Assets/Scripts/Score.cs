using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text scoreText;
    private Text highScore;
    
    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highScore = transform.Find("HighScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        highScore.text = "High Score: " + ScoreHandler.GetHighScore().ToString();
    }
    
    private void Update()
    {
        scoreText.text = Level.GetInstance().GetPipesPassed().ToString();
    }
}
