using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitLobbyScene : MonoBehaviour
{
    public void LoadScene(string Menu)
    {
        SceneManager.LoadScene(Menu);
    }
}