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
        Debug.Log("Awaking GO Window");
        scoreText = transform.Find("Score").GetComponent<Text>();
        transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(RestartGame);
        Bird.GetInstance().OnGameOver += OnGameOver;
        Hide();
    }

    private void RestartGame()
    {
        Debug.Log("Restarting Game");
        Loader.Load(Scenes.GameScene);
    }

    private void OnGameOver(object sender, System.EventArgs e)
    {
        Debug.Log("Game Over");
        scoreText.text = Level.GetInstance().GetPipesPassed().ToString();
        Show();
    }

    private void Hide() 
    {
        Debug.Log("Hiding GO Window");
        gameObject.SetActive(false);
    }

    private void Show()
    {
        Debug.Log("Showing GO Window");
        gameObject.SetActive(true);
    }
}
