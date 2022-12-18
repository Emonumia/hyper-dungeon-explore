using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : MonoBehaviour
{
    public void LoadScene(string Lobby)
    {
        SceneManager.LoadScene(Lobby);
    }
}