using UnityEngine;

namespace Core.Interactors {
    public interface IInteractor {
        bool isInitialized { get; }

        void OnCreate();
        Coroutine InitializeAsync();
        void Start();
        
        string GetStatusStartInitializing();
        string GetStatusCompleteInitializing();
        string GetStatusStart();
    }
}