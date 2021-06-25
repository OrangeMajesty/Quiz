using Core;
using UnityEngine;

namespace CustomScripts
{
    public class GameManager : Core.GameManager
    {
        protected override void OnGameLaunched() {
            GameQuiz.Run();
        }
    }
}