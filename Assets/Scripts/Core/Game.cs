using System.Collections.Generic;
using Core.Interactors;
using Core.Repositories;
using Core.Scenes;
using Core.Scenes.Config;
using UnityEngine.Events;

namespace Core {
    public enum State {
        NotInitialized,
        Initializing,
        Initialized
    }
    
    public abstract class Game {

        #region DELEGATES

        public delegate void GameHandler();
        public static event GameHandler OnGameInitializedEvent;

        #endregion
        
        protected static Game instance;
        public static State state { get; private set; }
        public static bool isInitialized => state == State.Initialized;
        public static ISceneManager sceneManager { get; private set; }

        #region Initializing

        public Game() {
            state = State.NotInitialized;
        }

        public void Initialize() {
            state = State.Initializing;
            
            this.InitSceneManager();
            this.LoadFirstScene(this.OnSceneLoadCompleted);
        }

        private void InitSceneManager() {
            sceneManager = this.CreateSceneManager();
        }

        protected abstract SceneManagerBase CreateSceneManager();
        protected abstract void LoadFirstScene(UnityAction<ISceneConfig> callback);

        private void OnSceneLoadCompleted(ISceneConfig config) {
            state = State.Initialized;
            OnGameInitializedEvent?.Invoke();
        }
        
        #endregion

        public static T GetInteractor<T>() where T : Interactor {
            return sceneManager.sceneActual.GetInteractor<T>();
        }

        public static IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return sceneManager.sceneActual.GetInteractors<T>();
        }

        public static T GetRepository<T>() where T : Repository {
            return sceneManager.sceneActual.GetRepository<T>();
        }

        public static IEnumerable<T> GetRepositories<T>() where T : IRepository
        {
            return sceneManager.sceneActual.GetRepositories<T>();
        }
    }
}