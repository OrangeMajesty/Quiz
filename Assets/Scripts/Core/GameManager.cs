using UnityEngine;

namespace Core {
    public class GameManager : MonoBehaviour
    {
        #region DELEGATES
        
        public delegate void GameManagerHandler();

        public static event GameManagerHandler OnApplicationPausedEvent;
        public static event GameManagerHandler OnApplicationUnpausedEvent;
        public static event GameManagerHandler OnApplicationQuitEvent;

        #endregion
        
        #region Start

        private void Start() {
            DontDestroyOnLoad(this.gameObject);
            
            this.OnGameLaunched();
        }

        protected virtual void OnGameLaunched(){ }
        
        #endregion
        
        #region Pause/Unpause

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                
                this.OnApplicationPaused();  
                OnApplicationPausedEvent?.Invoke();
            }
            else {
                this.OnApplicationUnpaused();
                OnApplicationUnpausedEvent?.Invoke();
            }
        }

        protected virtual void OnApplicationPaused() { }
        protected virtual void OnApplicationUnpaused() { }

        #endregion
        
        
        #region Quit

        private void OnApplicationQuit() {
            this.OnApplicationQuitted();
            OnApplicationQuitEvent?.Invoke();
        }

        protected virtual void OnApplicationQuitted() { }

        #endregion
        
    }
}