using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Interactors;
using UnityEngine;

namespace CustomScripts
{
    public sealed class LevelInteractor : Interactor
    {
        public const int startLevel = 1;
        public int currentLevel { get; private set; }
        
        // Количество элементов в строке
        public const int rowInLine = 3;

        // Макс. уровень
        private const int _maxLevel = 3;
        
        // Текущий номер задачи в уровне
        private int _taskInLevel;

        // Количество заданий в уровне
        public int countTaskInLevel = 3;
        
        // Количество отображаемых объектов
        public int count { get; private set; } 
        
        #region DELEGATES
        
        public delegate void LevelInteractorHandler();
        public event LevelInteractorHandler OnRestartLevel;
        public event LevelInteractorHandler OnFinishLevel;
        
        #endregion

        public LevelInteractor()
        {
            currentLevel = startLevel;
        }

        protected override void Initialize()
        {
            ChangeLevel(startLevel);
        }
        
        public void Save() {}

        private void ChangeLevel(int level)
        {
            currentLevel = level;
            count = currentLevel * rowInLine;
            _taskInLevel = 0;
        
            if(currentLevel <= _maxLevel) {
                OnRestartLevel?.Invoke();
                // RestartScene();
            } else
            {
                FinishLevel();
            }
        }
        
        // Повышает сложность
        public void UpLevel()
        {
            // Проверяем на возможные ошибки
            if(countTaskInLevel <= 0)
                countTaskInLevel = 3;

            if(currentLevel <= _maxLevel) {
                if(_taskInLevel < 0) {
                    _taskInLevel = 1;
                } else {
                    if(++_taskInLevel >= countTaskInLevel) {
                        ChangeLevel(currentLevel + 1);
                    } else {
                        OnRestartLevel?.Invoke();
                    }
                }
            } else
            {
                FinishLevel();
            }
        }

        public void FinishLevel()
        {
            OnFinishLevel?.Invoke();
            // todo ??
            GameObject.Find("Canvas").GetComponent<ButtonHandler>().finishPanel.SetActive(true);
        }
    }
}

