using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Загружает игрову
    public void OnStartButton()
    {
        Application.LoadLevel(1);
    }

    // Выход
    public void OnExitButton()
    {
        Application.Quit();
    }
}
