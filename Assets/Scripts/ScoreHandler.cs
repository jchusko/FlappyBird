using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public static string highScoreStr = "HighScore";

    public static void Start()
    {
        Bird.GetInstance().OnGameOver += OnGameOver;
    }

    private static void OnGameOver(object sender, System.EventArgs e)
    {
        IsNewHighScore(Level.GetInstance().GetPipesPassed());
    }    

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(highScoreStr);
    }

    public static bool IsNewHighScore(int score)
    {
        if(score > GetHighScore())
        {
            PlayerPrefs.SetInt(highScoreStr, score);
            PlayerPrefs.Save();
            return true;
        }

        return false;
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt(highScoreStr, 0);
        PlayerPrefs.Save();
    }
}
