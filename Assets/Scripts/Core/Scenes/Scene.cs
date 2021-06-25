using System.Collections;
using System.Collections.Generic;
using Core.Interactors;
using Core.Repositories;
using Core.Scenes.Config;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Tools;

namespace Core.Scenes {
    public sealed class Scene : IScene {
        public ISceneConfig sceneConfig { get; }
        public RepositoriesBase repositoriesBase { get; }
        public InteractorsBase interactorsBase { get; }

        public Scene(ISceneConfig config) {
            this.sceneConfig = config;
            this.repositoriesBase = new RepositoriesBase(config);
            this.interactorsBase = new InteractorsBase(config);
        }

        
        #region CREATE INSTANCES

        public void CreateInstances()
        {
            this.CreateAllRepositories();
            this.CreateAllInteractors();
        }
        
        private void CreateAllRepositories()
        {
            this.repositoriesBase.CreateAllRepositories();
        }

        private void CreateAllInteractors()
        {
            this.interactorsBase.CreateAllInteractors();
        }

        #endregion


        #region INITIALIZE

        public Coroutine InitializeAsync() {
            return Coroutines.StartRoutine(this.InitializeAsyncRoutine());
        }

        private IEnumerator InitializeAsyncRoutine() {
            yield return this.repositoriesBase.InitializeAllRepositories();
            yield return this.interactorsBase.InitializeAllInteractors();
        }

        #endregion


        #region START

        public void Start() {
            this.repositoriesBase.StartAllRepositories();
            this.interactorsBase.StartAllInteractors();
        }

        #endregion

        public T GetRepository<T>() where T : IRepository
        {
            return this.repositoriesBase.GetRepository<T>();
        }

        public IEnumerable<T> GetRepositories<T>() where T : IRepository
        {
            return this.repositoriesBase.GetRepositories<T>();
        }

        public T GetInteractor<T>() where T : IInteractor
        {
            return this.interactorsBase.GetInteractor<T>();
        }
        
        public IEnumerable<T> GetInteractors<T>() where T : IInteractor
        {
            return this.interactorsBase.GetInteractors<T>();
        }

    }
}