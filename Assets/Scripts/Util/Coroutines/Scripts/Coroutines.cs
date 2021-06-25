using System.Collections;
using  UnityEngine;

namespace VavilichevGD.Tools
{
    public class Coroutines : MonoBehaviour {
        private static Coroutines _instance => GetInstance();
        private static Coroutines _m_instance;
        private static bool isInitialized => _m_instance != null;
        
        private const string _name = "CoroutineManager";

        private static Coroutines GetInstance() {
            if (!isInitialized)
                _m_instance = CreateSingleton();
            return _m_instance;
        }

        private static Coroutines CreateSingleton() {
            Coroutines createdManager = new GameObject(_name).AddComponent<Coroutines>();
            createdManager.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(createdManager.gameObject);
            return createdManager;
        }

        public static Coroutine StartRoutine(IEnumerator enumerator) {
            return _instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine) {
            if (routine != null)
                _instance.StopCoroutine(routine);
        }

        public static void StopRoutine(string routineName) {
            _instance.StopCoroutine(routineName);
        }
    }
}