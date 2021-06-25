using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomScripts
{
    // todo переписать
    public class ButtonHandler : MonoBehaviour
    {
        public GameObject finishPanel;
        
        public void SendPauseGame(GameObject panel)
        {
            panel.SetActive(true);

            // todo Вызвать событие
        }        
        
        public void SendResumeGame(GameObject panel)
        {
            panel.SetActive(false);

            // todo Вызвать событие
        }        
        
        public void SendFinisheGame(GameObject panel)
        {
            panel.SetActive(true);

            // todo Вызвать событие
        }        
        
            
        // Переход к первой сцене
        public void LoadFirstScene()
        {
            SceneManager.LoadScene(0);
        }
    
        // Перезагрузка уровня
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.sceneCount);
        }    

        
        public void NextLevel()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
        
    }
}