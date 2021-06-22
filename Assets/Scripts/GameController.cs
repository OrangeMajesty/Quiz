using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    // Количество элементов в строке
    const int ROW_LIGHT = 3;

    // Макс. уровень
    const int MAX_LEVEL = 3;

    // prefab
    public GameObject block;

    // Меню
    public GameObject finishGamePanel;
    public GameObject pauseGamePanel;

    // Количество отображаемых объектов
    private int count; 

    private float offsetY = 0;
    private float offsetX = 0;

    // Размер блоков
    private float sizeBlockX = 400f;
    private float sizeBlockY = 400f;

    // Группы спрайтов
    private Dictionary<string, Dictionary<string, Object>> packs;

    // Хранит выбранные спрайты
    private Dictionary<string, GameObject> loadedSprites;

    // Выбранная группа
    string currentType;

    // Компонент вывода задачи
    public Text task;

    // Задача для игрока
    private string target;

    // Текущий уровень
    private int level;

    // Текущий номер задачи в уровне
    private int taskInLevel;

    // Количество заданий в уровне
    public int countTaskInLevel;

    // Start is called before the first frame update
    void Start()
    {
        // Начальный уровень
        level = 1;

        if(task is null)
            task = GameObject.Find("Task").GetComponent<Text>();

        // Сброс задания для игрока
        if(!(task is null))
            task.text = "";

        loadedSprites = new Dictionary<string, GameObject>();

        // Загрузка спрайтов
        LoadSpritesPack();

        // Установка уровня
        ChangeLevel(1);
    }
    
    // Загрузка спрайтов
    private void LoadSpritesPack()
    {
        string pathToPacks = "Sprites/Packs/";
        string pathToResources = "Assets/Resources/" + pathToPacks;
        packs = new Dictionary<string, Dictionary<string, Object>>();

        if(Directory.Exists(pathToResources)) {
        
            var files = Directory.GetFiles(pathToResources, "*.png");
            foreach (string file in files) {
                string packName = System.IO.Path.GetFileNameWithoutExtension(file);
                var type = packName.Split('_')[0];
                
                Object[] sprites = Resources.LoadAll(pathToPacks + packName);

                var pack = new Dictionary<string, Object>();
                if(packs.ContainsKey(type)) {
                    pack = packs[type];
                }

                foreach (var sprite in sprites) {
                    if(sprite.GetType() == typeof(Sprite)) {
                        pack.Add(sprite.name, sprite);
                    }
                }

                packs[type] = pack;
            }

        }
    }

    // Обработчик клика по спрайтам
    void HandlerClickToSprite(GameObject obj)
    {
        BlockController controller = obj.GetComponent<BlockController>();
        
        if(controller.key == target) {
            GameObject child = obj.transform.GetChild(0).gameObject;

            // Анимация при верном выборе
            if(child) {
                RectTransform objRect = child.GetComponent<RectTransform>();
                // if(objRect)
                    // objRect.DOShakeAnchorPos(0.3f, new Vector3(0.2f, 0, 0), 5, 0);
            }

            UpLevel();
        } else {
            RectTransform objRect = obj.GetComponent<RectTransform>();
            // Анимация при не верном выборе
            if(objRect)
                objRect.DOShakeAnchorPos(0.3f, new Vector3(10, 0, 0), 10, 0);
        }
    }

    // Повышает сложность
    private void UpLevel()
    {
        // Проверяем на возможные ошибки
        if(countTaskInLevel < 0)
            countTaskInLevel = 3;

        if(level <= MAX_LEVEL) {
            if(taskInLevel < 0) {
                taskInLevel = 1;
            } else {
                if(++taskInLevel >= countTaskInLevel) {
                    ChangeLevel(level + 1);
                } else {
                    RestartScene();
                }
            }
         } else {
            FinishGame();
         }
    }

    // Смена уровня
    private void ChangeLevel(int number = 1)
    {
        level = number;
        count = level * ROW_LIGHT;
        taskInLevel = 0;
        
        if(level <= MAX_LEVEL) {
            RestartScene();
        } else {
            FinishGame();
        }
    }

    // Удаление ранее загруженых объектов
    private async Task RemoveSprites()
    {
        var sequences = new List<Sequence>();

        foreach (KeyValuePair<string, GameObject> loadedSprite in loadedSprites) {
            Sequence sequence = DOTween.Sequence();

            // Анимация перед удалением
            sequence.Append(loadedSprite.Value.transform.DOScale(new Vector3(sizeBlockX, sizeBlockY, 1), 0.1f));
            sequence.Append(loadedSprite.Value.transform.DOScale(new Vector3(sizeBlockX-50, sizeBlockY-50, 1), 0.25f));
            sequence.Append(loadedSprite.Value.transform.DOScale(new Vector3(sizeBlockX+30, sizeBlockY+30, 1), 0.15f));
            sequence.Append(loadedSprite.Value.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => {
                Object.Destroy(loadedSprite.Value);
                loadedSprites.Remove(loadedSprite.Key);
            }));

            sequences.Add(sequence);
        }

        foreach (var sequence in sequences) {
            sequence.Play();
        }

        while (loadedSprites.Count > 0) {
            await Task.Yield();
        }
    }

    // Перезагрузка сцены
    private async void RestartScene()
    {
        task.DOFade(0f, 0.3f);

        // Удаление ранее загруженых объектов
        await RemoveSprites();

        var types = packs.Keys.ToList();
        currentType = types[Random.Range(0, types.Count)];

        var currentPack = packs[currentType];
        List<string> currentPackKeysList = new List<string>(currentPack.Keys);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if(ROW_LIGHT == 0) {
            Debug.Log("Не определено кол-во элементов в строке");
            return;
        }

        // Находим смещение относительно 0
        offsetX = sizeBlockX;
        offsetY = (sizeBlockY * ((count / ROW_LIGHT) -1)) / 2;

        // Хранит выбранные спрайты
        var selectBlockKeys = new List<string>();
        var rectTransforms = new List<RectTransform>();

        // Раставляем блоки
        for (int y = 0; y < count / ROW_LIGHT; y++) {
            for (int x = 0; x < ROW_LIGHT; x++) {
                GameObject item = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                
                RectTransform itemRect = item.GetComponent<RectTransform>();
                rectTransforms.Add(itemRect);
                
                item.GetComponent<Canvas>().sortingOrder = (y * ROW_LIGHT + x) + 1;
                item.transform.SetParent(this.transform);
                itemRect.anchoredPosition = new Vector3(offsetX - (sizeBlockX * x), offsetY - (sizeBlockY * y), 1);
                item.transform.localScale = Vector3.zero;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(item.transform.DOScale(new Vector3(sizeBlockX+30, sizeBlockY+30, 1), 0.25f));
                sequence.Append(item.transform.DOScale(new Vector3(sizeBlockX-50, sizeBlockY-50, 1), 0.15f));
                sequence.Append(item.transform.DOScale(new Vector3(sizeBlockX, sizeBlockY, 1), 0.1f));
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
                item.GetComponent<BlockController>().key = currentKey;
                
                GameObject child = item.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = (Sprite)currentPack[currentKey];
                // child.GetComponent<SpriteRenderer>().sprite = (Sprite)currentPack[currentKey];

                item.GetComponent<Button>().onClick.AddListener(() => HandlerClickToSprite(item));

                // Запоминаем что выбрали
                selectBlockKeys.Add(currentKey);
                loadedSprites[currentKey] = item;
            }      
        }

        // Выбираем цель
        target = selectBlockKeys[Random.Range(0, selectBlockKeys.Count)];

        // Назначаем задачу игроку
        if(!(task is null)) {
            task.DOFade(1f, 0.3f);
            task.text = "Найдите " + target;
        }

        rectTransforms.Clear();
    }

    // Завершение игры
    private async void FinishGame()
    {
        // Сброс задания для игрока
        if(!(task is null))
            task.text = "";

        // Удаление ранее загруженых объектов
        await RemoveSprites();

        finishGamePanel.SetActive(true);
        Debug.Log("Finish Game");
    }

    public void ShowPauseGamePanel()
    {
        pauseGamePanel.SetActive(true);
    }

    
    public void HidePauseGamePanel()
    {
        pauseGamePanel.SetActive(false);
    }

    // Переход к первой сцене
    public void GoToHome()
    {
        SceneManager.LoadScene(0);
    }

    // Перезагрузка уровня
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
