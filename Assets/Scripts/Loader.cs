using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scripts.Enums;

public static class Loader
{
    private static Scenes targetScene;

    public static void Load(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}
