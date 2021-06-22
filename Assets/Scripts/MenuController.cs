using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Загружает игрову
    public void OnStartButton()
    {
        SceneManager.LoadScene(1);
    }

    // Выход
    public void OnExitButton()
    {
        Application.Quit();
    }
}
