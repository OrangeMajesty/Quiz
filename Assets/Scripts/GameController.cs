using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // prefab
    public GameObject block;

    // Количество отображаемых объектов
    public int count; 

    private float offsetY = 0;
    private float offsetX = 0;

    // Группы спрайтов
    private Dictionary<string, Dictionary<string, Object>> packs;
    string currentType;

    // Компонент вывода задачи
    public Text task;

    // Задача для игрока
    private string target;

    private int level;

    // Start is called before the first frame update
    void Start()
    {
        // Начальный уровень
        level = 1;

        if(task is null)
            task = GameObject.Find("Task").GetComponent<Text>();

        // Сброс задания для игрока
        if(!(task is null))
            task.text = "...";

        // Загрузка спрайтов
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
        
        // todo: Случайный выбор типа
        currentType = "Letter";
        var currentPack = packs[currentType];
        List<string> currentPackKeysList = new List<string>(currentPack.Keys);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Количество элементов в строке
        int rowLenght = 3;

        if(rowLenght == 0) {
            Debug.Log("Не определено кол-во элементов в строке");
            return;
        }

        float sizeBlockY = 100f;
        float sizeBlockX = 100f;

        // Находим смещение относительно 0
        offsetX = sizeBlockX;
        offsetY = (sizeBlockY * ((count / rowLenght) -1)) / 2;

        // Хранит выбранные спрайты
        var selectBlockKeys = new List<string>();

        // Раставляем блоки
        for (int y = 0; y < count / rowLenght; y++) {
            for (int x = 0; x < rowLenght; x++) {
                GameObject item = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                item.transform.SetParent(this.transform);
                item.GetComponent<RectTransform>().localScale = new Vector3(sizeBlockX, sizeBlockY, 1);
                item.GetComponent<RectTransform>().anchoredPosition = new Vector3(offsetX - (sizeBlockX * x), offsetY - (sizeBlockY * y), 0);

                // Выбор ключа спрайта
                var currentKey = "";
                for (int i = 0; i < 20; i++) {
                    currentKey = currentPackKeysList[Random.Range(0, currentPackKeysList.Count)];

                    // Проверка на дубликат
                    if(selectBlockKeys.IndexOf(currentKey) == -1)
                        break;
                }
                
                GameObject child = item.transform.GetChild(0).gameObject;
                child.GetComponent<SpriteRenderer>().sprite = (Sprite)currentPack[currentKey];

                // Запоминаем что выбрали
                selectBlockKeys.Add(currentKey);
            }      
        }

        // Выбираем цель
        target = selectBlockKeys[Random.Range(0, selectBlockKeys.Count)];

        // Назначаем задачу игроку
        if(!(task is null))
            task.text = "Find " + target;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Смена уровня
    private void ChangeLevel(int number = 1)
    {
         
    }
}
