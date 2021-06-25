using Core;
using Core.Scenes;
using Core.Scenes.Config;
using UnityEngine.Events;

namespace CustomScripts
{
    public sealed class GameQuiz : Game
    {
        public static void Run() {
            bool singletonCreated = CreateSingleton();
            if (singletonCreated)
            {
                OnGameInitializedEvent += OnGameInitialized;
                instance.Initialize();
            }
        }

        private static bool CreateSingleton() {
            if (instance != null)
                return false;
            
            instance = new GameQuiz();
            return true;
        }

        #region Events

        private static void OnGameInitialized() {
            OnGameInitializedEvent -= OnGameInitialized;
        }

        #endregion

        protected override SceneManagerBase CreateSceneManager() {
            return new SceneManagerL1();
        }

        protected override void LoadFirstScene(UnityAction<ISceneConfig> callback) {
            sceneManager.InitializeCurrentScene(callback);
        }
    }
}