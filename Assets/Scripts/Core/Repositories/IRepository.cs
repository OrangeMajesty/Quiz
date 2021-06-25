using UnityEngine;

namespace Core.Repositories {
    public interface IRepository {
        
        bool isInitialized { get; }
        // string id { get; }
        // int version { get; }
        
        void OnCreate();
        Coroutine InitializeAsync();
        void Start();

        string GetStatusStartInitializing();
        string GetStatusCompleteInitializing();
        string GetStatusStart();
    }
}