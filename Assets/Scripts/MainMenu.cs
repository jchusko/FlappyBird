using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts.Enums;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("PlayButton").GetComponent<Button>().onClick.AddListener(() => Loader.Load(Scenes.GameScene));
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(() => Application.Quit());
}
