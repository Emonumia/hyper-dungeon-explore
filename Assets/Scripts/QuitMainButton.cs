using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMainButton : MonoBehaviour
{
    public void LoadScene(string Menu)
    {
        SceneManager.LoadScene(Menu);
    }
}