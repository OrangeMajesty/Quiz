using System;
using Core;
using CustomScripts.Target;
using DG.Tweening;
using UnityEngine;

namespace CustomScripts
{
    public class Card : MonoBehaviour
    {
        // Размер блоков
        public const float sizeBlockX = 400f;
        public const float sizeBlockY = 400f;
        public string key;
        
        public GameObject child;

        [SerializeField]
        private string target;
        private TargetInteractor _targetInteractor;
        private LevelInteractor _levelInteractor;

        public RectTransform rect;
        public RectTransform childRect;

        public void Start()
        {
            this._targetInteractor = Game.GetInteractor<TargetInteractor>();
            this._levelInteractor = Game.GetInteractor<LevelInteractor>();

            this.target = _targetInteractor.target;

            this.rect = GetComponent<RectTransform>();
            
            this.child = transform.GetChild(0).gameObject;
            this.childRect = child.GetComponent<RectTransform>();
        }

        // Обработчик клика по спрайтам
        public void OnClick()
        {
            if (key.Length == 0)
                throw new Exception("Не установлен ключ корточки");
                    
            if (key == target)
            {
                // GameObject child = obj.transform.GetChild(0).gameObject;
            
                // Анимация при верном выборе
                if (child)
                {
                    // RectTransform objRect = child.GetComponent<RectTransform>();
                    // if(objRect)
                    // objRect.DOShakeAnchorPos(0.3f, new Vector3(0.2f, 0, 0), 5, 0);
                }
                
                _levelInteractor.UpLevel();
            } 
            else
            {
                // Анимация при не верном выборе
                if (rect)
                    rect.DOShakeAnchorPos(0.3f, new Vector3(10, 0, 0), 10, 0);
            }
        }

        public Sequence BeforeDestroy()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(transform.DOScale(new Vector3(sizeBlockX, sizeBlockY, 1), 0.1f));
            sequence.Append(transform.DOScale(new Vector3(sizeBlockX-50, sizeBlockY-50, 1), 0.25f));
            sequence.Append(transform.DOScale(new Vector3(sizeBlockX+30, sizeBlockY+30, 1), 0.15f));
            sequence.Append(transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => {
                GameObject.Destroy(this);
            }));

            return sequence;
        }
    }
}