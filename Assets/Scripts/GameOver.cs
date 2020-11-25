using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Scripts.Enums;
using CodeMonkey.Utils;

public class GameOver : MonoBehaviour
{
    private Text scoreText;
    private Text highScoreText;

    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();
        highScoreText = transform.Find("HighScore").GetComponent<Text>();

        transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(RestartGame);
        transform.Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() => Loader.Load(Scenes.MainMenu));

        Bird.GetInstance().OnGameOver += OnGameOver;
        Hide();
    }

    private void RestartGame()
    {
        Loader.Load(Scenes.GameScene);
    }

    private void OnGameOver(object sender, System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassed().ToString();

        if(Level.GetInstance().GetPipesPassed() >= ScoreHandler.GetHighScore())
        {
            highScoreText.text = "New High Score!";
        }
        else
        {
            highScoreText.text = "High Score: " + ScoreHandler.GetHighScore();
        }

        Show();
    }

    private void Hide() 
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
