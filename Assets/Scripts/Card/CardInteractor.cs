using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Interactors;
using CustomScripts.Target;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
    public sealed class CardInteractor : Interactor
    {
        private CardRepository _repository;
        public Dictionary<string, Dictionary<string, Object>> sprites => this._repository.sprites;
        public Dictionary<string, GameObject> loadedCards;
        
        public const string prefabName = "Block";

        private GameObject _prefab;
        private GameObject _cardArea;

        private TargetInteractor _targetInteractor;
        private PrefabInteractor _prefabInteractor;
        private LevelInteractor _levelInteractor;
        
        private float _offsetY = 0;
        private float _offsetX = 0;
        
        // Выбранная группа
        private string selectedType;
        
        #region DELEGATES

        public delegate void GameHandler();
        public static event GameHandler OnPrepareInteractorDone;

        #endregion

        public CardInteractor()
        {
            loadedCards = new Dictionary<string, GameObject>();
        }

        public Dictionary<string, Object> GetOfType(string type)
        {
            var map = new Dictionary<string, Object>();
            
            if (!(sprites is null))
                sprites.TryGetValue(type, out map);

            return map;
        }

        public string[] GetTypes()
        {
            return sprites.Keys.ToArray();
        }

        protected override void Initialize()
        {
            // Корточки добавляются в объект с тегом CardArea если он есть
            _cardArea = GameObject.FindGameObjectWithTag("CardArea");
            
            Game.OnGameInitializedEvent += InitializeDependInteractors;
            OnPrepareInteractorDone += LoadingCards;
        }

        private void InitializeDependInteractors()
        {
            this._repository = Game.GetRepository<CardRepository>();
            this._targetInteractor = Game.GetInteractor<TargetInteractor>();
            this._prefabInteractor = Game.GetInteractor<PrefabInteractor>();
            this._levelInteractor = Game.GetInteractor<LevelInteractor>();

            _levelInteractor.OnRestartLevel += ReloadCards;
            _prefab = _prefabInteractor.GetPrefabByName(prefabName);
            
            OnPrepareInteractorDone?.Invoke();
        }
        
        public void Save() {}

        public async void ReloadCards()
        {
            await ClearLoadedCards();
            LoadingCards();
        }
        
        public void LoadingCards()
        {
            var types = sprites.Keys.ToList();
            selectedType = types[Random.Range(0, types.Count)];

            List<string> currentPackKeysList = new List<string>(sprites[selectedType].Keys);
            var levelInteractor = Game.GetInteractor<LevelInteractor>();
            var rowInLine = LevelInteractor.rowInLine;
            var count = levelInteractor.count;
            
            // float screenWidth = Screen.width;
            // float screenHeight = Screen.height;
        
            if(rowInLine == 0) {
                Debug.Log("Не определено кол-во элементов в строке");
                return;
            }
        
            // Находим смещение относительно 0
            _offsetX = Card.sizeBlockX;
            _offsetY = (Card.sizeBlockY * ((count / rowInLine) -1)) / 2;
        
            // Хранит выбранные спрайты
            var selectBlockKeys = new List<string>();

            // Раставляем блоки
            for (int y = 0; y < count / rowInLine; y++) {
                for (int x = 0; x < rowInLine; x++) {
                    GameObject item = GameObject.Instantiate(_prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    
                    RectTransform itemRect = item.GetComponent<RectTransform>();
                    
                    item.GetComponent<Canvas>().sortingOrder = (y * rowInLine + x) + 1;
                    if (!(_cardArea is null))
                        item.transform.SetParent(_cardArea.transform);
                    itemRect.anchoredPosition = new Vector3(_offsetX - (Card.sizeBlockX * x), _offsetY - (Card.sizeBlockY * y), 1);
                    item.transform.localScale = Vector3.zero;
        
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(item.transform.DOScale(new Vector3(Card.sizeBlockX+30, Card.sizeBlockY+30, 1), 0.25f));
                    sequence.Append(item.transform.DOScale(new Vector3(Card.sizeBlockX-50, Card.sizeBlockY-50, 1), 0.15f));
                    sequence.Append(item.transform.DOScale(new Vector3(Card.sizeBlockX, Card.sizeBlockY, 1), 0.1f));
                    sequence.Play();
        
                    // Выбор ключа спрайта
                    var currentKey = "";
                    for (int i = 0; i < 20; i++) {
                        currentKey = currentPackKeysList[Random.Range(0, currentPackKeysList.Count)];
        
                        // Проверка на дубликат
                        if(selectBlockKeys.IndexOf(currentKey) == -1)
                            break;
                    }
        
                    // Установка значения блока
                    Card itemCard = item.GetComponent<Card>();
                    itemCard.key = currentKey;

                    GameObject child = item.transform.GetChild(0).gameObject;
                    child.GetComponent<Image>().sprite = (Sprite)sprites[selectedType][currentKey];
                    item.GetComponent<Button>().onClick.AddListener(() => itemCard.OnClick());
        
                    // Запоминаем что выбрали
                    selectBlockKeys.Add(currentKey);
                    loadedCards[currentKey] = item;
                }      
            }
        
            // Выбираем цель и назначаем задачу игроку
            _targetInteractor.SetTarget(selectBlockKeys[Random.Range(0, selectBlockKeys.Count)]);
        }
        
        // Удаление ранее загруженых объектов
        private async Task ClearLoadedCards()
        {
            var sequences = new List<Sequence>();

            foreach (KeyValuePair<string, GameObject> loadedCard in loadedCards)
            {
                var sequence = loadedCard.Value.GetComponent<Card>().BeforeDestroy();
                sequence.OnComplete(() =>
                {
                    Object.Destroy(loadedCard.Value);
                    loadedCards.Remove(loadedCard.Key);
                });
                sequences.Add(sequence);
            }

            foreach (var sequence in sequences) {
                sequence.Play();
            }

            while (loadedCards.Count > 0) {
                await Task.Yield();
            }
        }
    }
}

