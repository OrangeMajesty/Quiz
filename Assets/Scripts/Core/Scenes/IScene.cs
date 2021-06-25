using System.Collections.Generic;
using Core.Interactors;
using Core.Repositories;
using Core.Scenes.Config;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Scenes {
    public interface IScene {
        
        ISceneConfig sceneConfig { get; }
        RepositoriesBase repositoriesBase { get; }
        InteractorsBase interactorsBase { get; }
        
        void CreateInstances();
        Coroutine InitializeAsync();
        void Start();

        T GetRepository<T>() where T : IRepository;
        IEnumerable<T> GetRepositories<T>() where T : IRepository;
        
        T GetInteractor<T>() where T : IInteractor;
        IEnumerable<T> GetInteractors<T>() where T : IInteractor;
    }
}