using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Transform lastSpawnedNote;
    public GameObject losePanel;
    public float spawnInterval = 1f;
    public Note notePrefab;
    public NoteWariant[] noteWariantPrefab;
    public float noteSpeed = 5f;
    public const int NotesToSpawn = 20;
    public static GameController Instance { get; private set; }
    public Transform noteContainer;
    private bool gameStarted;
    public bool GameStarted
    {
        get => gameStarted;
        set
        {
            gameStarted = value;
            OnGameStartedChanged?.Invoke(gameStarted);
        }
    }

    private bool gameOver;
    public bool GameOver
    {
        get => gameOver;
        set
        {
            gameOver = value;
        }
    }
    private bool showGameOverScreen;
    public bool ShowGameOverScreen
    {
        get => showGameOverScreen;
        set
        {
            showGameOverScreen = value;
            OnShowGameOverScreenChanged?.Invoke(showGameOverScreen);
        }
    }

    public delegate void GameStartedChanged(bool newState);
    public event GameStartedChanged OnGameStartedChanged;

    public delegate void ScoreChanged(int newScore);
    //public event ScoreChanged OnScoreChanged;

    public delegate void ShowGameOverScreenChanged(bool newState);
    public event ShowGameOverScreenChanged OnShowGameOverScreenChanged;

    public int LastPlayedNoteId { get; set; } = 0;
    public AudioSource audioSource;
    public bool PlayerWon { get; set; } = false;

    public float bpm;
    private float noteSpawnInterval;
    public float totalTiles;
    public int spawnedTiles;
    //private int tileCounter = 0;
    public int tilesToSpawnPerIteration = 20;
    public GameBG gameBG;
    public GameOver Over;

    public int spawnVariantEveryNNotes = 5;

    public GameObject panel;
    public GameObject SecondPanelGameOver;

    public PanelWictory panelWictory;
    public GameObject WictoryPanel;
    public int everyPlatform = 10;

    [Header("время остановки музыки")]
    public float stopTime;

    [Header("Взаимодействие с магазином")]
    public TMP_Text textScoreInShop;
    [Header("Проверка свёз после игры")]
    public MainMenu mainMenu;
    public SwipePanel swipePanel;
    public GameObject sliderBG;

    public Sprite whiteStar;
    public Sprite yellowStar;
    public List<Image> imageStar;

    [Header("Параметры для счёта")]
    public int countStar = 0; // ВАЛЮТА
    public Text textScore;
    public Slider sliderScore;
    public Image[] starImage;
    public Sprite starYelloy;
    List<int> result;

    [Header("Слайдер для окончания игры")]
    public Slider gameOverSlider;
    public Image[] starImageGameOver;
    public Text scoreGameOver;

    [Header("массив кнопок для проверки был ли куплент или нет")]
    public RecordInShop[] recordInShops;

    private static float noteHeight;
    private static float noteWidth;
    private Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    private int prevRandomIndex = -1;
    private int lastNoteId = 1;
    private bool lastSpawn = false;

    private void Awake()
    {
        Instance = this;
        GameStarted = false;
        GameOver = false;
        ShowGameOverScreen = false;
    }

    private float CalculateNoteSpeed()
    {
        return noteHeight / noteSpawnInterval;
    }

    private void CalculateTotalTiles()
    {
        totalTiles = audioSource.clip.length * (bpm / 60);
    }

    private void Update()
    {
        DetectNoteClicks();

        if (GameStarted && !Instance.GameOver)
        {

            noteContainer.Translate(Vector2.down * noteSpeed * Time.deltaTime);
        }
    }

    public void DetectStart()
    {
        GameStarted = true;
        SetDataForNoteGeneration();
        CalculateNoteSpawnInterval();
        CalculateTotalTiles();
        noteSpeed = CalculateNoteSpeed();
        sliderScore.maxValue = totalTiles;
        gameOverSlider.maxValue = totalTiles;
        ChangeSlider(sliderScore, true, starImage, textScore);
        audioSource.Play();
        SpawnNotes();
        gameBG.StartAnimGB();
        ActiveLosePanel();
        StopAnimaRemordInMainMenu();
    }

    public void RestartLevel()
    {
        foreach (Transform child in noteContainer.transform)
        {
            Destroy(child.gameObject);
        }

        GameOver = false;
        lastSpawn = false;
        spawnedTiles = 0;
        LastPlayedNoteId = 0;
        lastNoteId = 1;
        audioSource.Stop();
        audioSource.Play();
        SetDataForNoteGeneration();
        SpawnNotes();
        gameBG.StartAnimGB();
        Data.dataInstance.SaveStarCount();
        Over.Con();
        ActiveLosePanel();
        StopAnimaRemordInMainMenu();
        sliderScore.value = 0;
    }

    public void StopAnimaRemordInMainMenu()
    {
        for (int i = 0; i < mainMenu.records.Length; i++)
        {
            mainMenu.records[i].StopCoroutineAnimaRecord();
        }
    }
    public void StartAnimaRemordInMainMenu()
    {
        for (int i = 0; i < mainMenu.records.Length; i++)
        {
            mainMenu.records[i].StartCoroutineAnimaRecord();
        }
    }

    public void ActiveLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void DisActiveLosePanel()
    {
        losePanel.SetActive(false);
    }

    public Canvas panelTrigger;
    public Camera cameraa;

    public GameObject bad;
    public GameObject cool;
    public GameObject perfect;

    void DetectNoteClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);

            if (hit)
            {
                GameObject gameObject = hit.collider.gameObject;
                if (gameObject.CompareTag("Note"))
                {
                    Note note = gameObject.GetComponent<Note>();
                    note.Play();

                    Article(note.gameObject);
                }
            }
        }
    }
    public void Article(GameObject go)
    {
        RectTransform canvasRectTransform = panelTrigger.GetComponent<RectTransform>();
        Vector3 canvasCenterScreen = Camera.main.WorldToScreenPoint(canvasRectTransform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, canvasCenterScreen, Camera.main, out Vector2 canvasCenterLocal);
        Vector3 panelCenter = go.GetComponent<SpriteRenderer>().bounds.center;
        Vector3 panelCenterScreen = Camera.main.WorldToScreenPoint(panelCenter);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, panelCenterScreen, Camera.main, out Vector2 panelCenterLocal);

        float distance = Vector2.Distance(panelCenterLocal, canvasCenterLocal);

        float distanceThreshold1Min = 160f;
        float distanceThreshold1Max = 180f;
        float distanceThreshold2Min = 181f;
        float distanceThreshold2Max = 220f;

        if (distance >= distanceThreshold1Min && distance <= distanceThreshold1Max)
        {
            StartCoroutine(ActivatePanel(perfect));
        }
        else if (distance >= distanceThreshold2Min && distance <= distanceThreshold2Max)
        {
            StartCoroutine(ActivatePanel(cool));
        }
        else
        {
            StartCoroutine(ActivatePanel(bad));
        }
    }
    IEnumerator ActivatePanel(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
    }

    private void CalculateNoteSpawnInterval()
    {
        noteSpawnInterval = 60f / bpm;
    }

    // расчёт размера плиток
    private void SetDataForNoteGeneration()
    {
        var topRight = new Vector3(Screen.width, Screen.height, 0);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;

        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;

        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();
        noteLocalScale = new Vector3(
            noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
            noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);

        var noteBoxCollider2D = notePrefab.GetComponent<BoxCollider2D>();
        if (noteBoxCollider2D != null)
        {
            noteBoxCollider2D.size = new Vector2(noteWidth / noteLocalScale.x, noteHeight / noteLocalScale.y);
        }

        var leftmostPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2));
        var leftmostPointPivot = leftmostPoint.x + noteWidth / 2;
        noteSpawnStartPosX = leftmostPointPivot;
    }

    // спавн плиток
    public void SpawnNotes()
    {
        if (lastSpawn || spawnedTiles >= totalTiles) return;

        var noteSpawnStartPosY = lastSpawnedNote.position.y + noteHeight;
        Note note = null;
        NoteWariant noteWariant = null;
        var timeTillEnd = audioSource.clip.length - audioSource.time;
        int notesToSpawn = NotesToSpawn;
        if (timeTillEnd < NotesToSpawn || spawnedTiles + NotesToSpawn > totalTiles)
        {
            notesToSpawn = Mathf.CeilToInt(timeTillEnd);
            lastSpawn = true;
        }

        for (int i = 0; spawnedTiles < totalTiles; i++)
        {
            int random = UnityEngine.Random.Range(0, noteWariantPrefab.Length);
            var randomIndex = GetRandomIndex();

            if (i % 5 == 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    noteWariant = Instantiate(noteWariantPrefab[random], noteContainer.transform);
                    noteWariant.transform.localScale = noteLocalScale;
                    noteWariant.transform.position = new Vector2(noteSpawnStartPosX + noteWidth * j, noteSpawnStartPosY);
                    noteWariant.Visible = (j == randomIndex);
                    if (noteWariant.Visible)
                    {
                        noteWariant.Id = lastNoteId;
                        lastNoteId++;
                    }
                    else
                    {
                        Destroy(noteWariant.gameObject);
                    }
                }
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    note = Instantiate(notePrefab, noteContainer.transform);
                    note.transform.localScale = noteLocalScale;
                    note.transform.position = new Vector2(noteSpawnStartPosX + noteWidth * j, noteSpawnStartPosY);
                    note.Visible = (j == randomIndex);
                    if (note.Visible)
                    {
                        note.Id = lastNoteId;
                        lastNoteId++;
                    }
                    else 
                    {
                        Destroy(note.gameObject);
                    }
                }
            }

            noteSpawnStartPosY += noteHeight;
            spawnedTiles++;
        }
    }

    private int GetRandomIndex()
    {
        var randomIndex = UnityEngine.Random.Range(0, 4);
        while (randomIndex == prevRandomIndex) randomIndex = UnityEngine.Random.Range(0, 4);
        prevRandomIndex = randomIndex;
        return randomIndex;
    }

    public void EndGame()
    {
        GameOver = true;
        CheckStarAfterGame();
        StopAtSpecificTime();
        Data.dataInstance.SaveRecordStar();
        ShowGameOverScreen = true;
        Debug.Log("GameOVER");
    }


    int count = 0;
    public void ChangeSlider(Slider slider = null, bool restartScore = false, Image[] star = null, Text text = null)
    {
        if (restartScore == false)
        {
            if (slider == sliderScore)
            {
                slider.value++;
                text.text = $"{slider.value}";
            }
            else
            {
                slider.value++;
                text.text = $"SCORE: {slider.value}";
            }
        }
        else
        {
            result = new List<int>();
            text.text = "0";
            float maxValue = slider.maxValue;
            float[] percentages = { 0.125f, 0.3f, 0.5f, 0.7f, 0.9f };

            for (int i = 0; i < 6; i++)
            {
                if (i < percentages.Length)
                {
                    int value = (int)(percentages[i] * maxValue);
                    result.Add(value);
                    Debug.Log($"Iteration {i + 1}: {percentages[i] * 100}% of {maxValue} = {value}");
                }
                else
                {
                    int value = (int)maxValue; // В шестой итерации берем полное значение maxValue
                    result.Add(value);
                }
            }
        }
        if (slider.value >= result[4])
        {
            if (star[0].sprite != starYelloy)
            {
                star[0].sprite = starYelloy;
                countStar++;
                count++;
                Data.dataInstance.SaveStarCount();
            }
        }
        if (slider.value >= result[3])
        {
            if (star[1].sprite != starYelloy)
            {
                star[1].sprite = starYelloy;
                count=2;
                Data.dataInstance.SaveStarCount();
            }
        }
        if (slider.value >= result[2])
        {
            if (star[2].sprite != starYelloy)
            {
                star[2].sprite = starYelloy;
                countStar++;
                count =3;
                Data.dataInstance.SaveStarCount();
            }
        }
        if (slider.value >= result[1])
        {
            if (star[3].sprite != starYelloy)
            {
                star[3].sprite = starYelloy;
                countStar++;
                count = 4;
                Data.dataInstance.SaveStarCount();
            }
        }
        if (slider.value == result[0] - 1)
        {
            if (star[4].sprite != starYelloy)
            {
                star[4].sprite = starYelloy;
                countStar++;
                count = 5;
                Data.dataInstance.SaveStarCount();
            }
        }
    }
    public void StarComparison()
    {
        int count = swipePanel.currentPage;
        count--;
        for (int i =0; i < mainMenu.records[count].star.Count; i++)
        {
            starImage[i].sprite = mainMenu.records[count].star[i].sprite;
        }
    }
    public void CountStarInSlider()
    {
        panelWictory.textStar.text = count.ToString();
        int index = swipePanel.currentPage;
        index--;
        for (int i =0; i < mainMenu.records[index].star.Count; i++)
        {
            if (mainMenu.records[index].star[i].sprite != yellowStar)
            {
                mainMenu.records[index].star[i].sprite = yellowStar;
            }
        }
        Data.dataInstance.SaveRecordStar();
        count = 0;
    }


    // Остановка музыки на определённом месте
    public void StopAtSpecificTime()
    {
        audioSource.Pause(); // Приостановка воспроизведения аудио
        stopTime = audioSource.time; // Сохранение текущего времени воспроизведения
    }

    // Возобновление воспроизведения музыки с того же места
    public void ResumeMusic()
    {
        audioSource.time = stopTime; // Установка времени воспроизведения на сохранённое время
        audioSource.Play(); // Возобновление воспроизведения аудио
    }



    // Установка количества звезд в магазине
    public void SetStarInShop()
    {
        textScoreInShop.text = countStar.ToString(); // Устанавливаем текстовое значение количества звезд в магазине
    }

    // Проверка звезд после игры
    public void CheckStarAfterGame()
    {
        imageStar = new List<Image>(); // Создаем новый список для изображений звезд
        // Проходим через все изображения в sliderBG, исключая само sliderBG
        foreach (var image in sliderBG.GetComponentsInChildren<Image>())
        {
            if (image.gameObject != sliderBG.gameObject)
            {
                imageStar.Add(image); // Добавляем изображения в список imageStar
            }
        }
        int count = swipePanel.currentPage; // Получаем текущую страницу панели свайпа
        count--; // Уменьшаем значение count на 1, чтобы использовать его как индекс
        // Проходим через все изображения в imageStar
        for (int i = 0; i < imageStar.Count; i++)
        {
            if (imageStar[i].sprite == yellowStar) // Если изображение звезды желтое
            {
                mainMenu.records[count].star[i].sprite = yellowStar; // Устанавливаем соответствующую звезду в mainMenu как желтую
            }
        }
    }

    // Проверка состояния кнопок покупки
    public void CheckButtonIsBuy()
    {
        // Проходим через все записи в магазине
        for (int i = 0; i < recordInShops.Length; i++)
        {
            // Если кнопка не куплена (isBuyButton == 0), отображаем кнопку покупки
            if (recordInShops[i].isBuyButton == 0)
            {
                recordInShops[i].buttonBuy.gameObject.SetActive(true);
            }
            // Если кнопка куплена (isBuyButton != 0), скрываем кнопку покупки
            else
            {
                recordInShops[i].buttonBuy.gameObject.SetActive(false);
            }
        }
    }

    // Остановка музыки
    public void StopMusic()
    {
        audioSource.Stop(); // Останавливаем воспроизведение аудио
    }

    public void PanelToRestart()
    {
        panel.SetActive(false);
        GameOver = false;
        ResumeMusic();
    }
    public void ContiniumLevel()
    {
        if (countStar >= 5)
        {
            countStar -= 5;
            Data.dataInstance.SaveStarCount();
            ChangeSlider(gameOverSlider, false, starImageGameOver, scoreGameOver);// счёт
            ChangeSlider(sliderScore, false, starImage, textScore);// счёт
            Over.Con();
            panel.SetActive(true);
            SecondPanelGameOver.SetActive(false);
        }
    }
}