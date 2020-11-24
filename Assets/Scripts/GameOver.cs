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

    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();

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
