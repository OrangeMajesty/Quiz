using Core;
using Core.Interactors;
using UnityEngine;

namespace CustomScripts.Target
{
    public class TargetInteractor : Interactor
    {
        private GameObject _gameObject;

        public string target { get; private set; }

        public TargetInteractor()
        {
        }

        public void SetTarget(string target)
        {
            SetMessage("Найдите " + target);
            this.target = target;
        }
        
        public void ConnectGameObject(GameObject gameObject)
        {
            this._gameObject = gameObject;
        }

        public void SetMessage(string target = "")
        {
            if(_gameObject)
                _gameObject.SendMessage("SetMessage", target);
        }

        public void ShowFade()
        {
            if(_gameObject)
                _gameObject.SendMessage("ShowFade");
        }

        public void HideFade()
        {
            if(_gameObject)
                _gameObject.SendMessage("HideFade");
        }
    }
}