using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Game,
        Pause,
        ViewingTheMap,
        GameOver,
        Victory,
        InspectionObject,
        Task
    }

    public GameState GameStateS;

    [SerializeField] private Player _playerS;
    [SerializeField] private BackPack _backPackS;
    [SerializeField] private Timer _timerS;
    [SerializeField] private KeyCodeS _keyCodeS;
    [SerializeField] private MapKeeper _mapKeeperS;
    [SerializeField] private UIPanel _uIpanelS;
    [SerializeField] private UIButtonController _uiButtonControllerS;
    [SerializeField] private UIWidget _uiWidgetS;
    [SerializeField] private WallsMove[] _wallsMovesS;
    [SerializeField] private ActiveObject[] _activeObjectsS;
    private UIBlur _uIBlurS;

    [SerializeField] private GameObject[] _brazierPrefab;

    [SerializeField] private Image[] _imageInScene; // это массив картинок со сцены. Он нужен для изменения цвета темы игры
    [SerializeField] private List<Image> _imagesInScene = new List<Image>(); // этот список нужен чтобы отсортировать массив <_imageInScene>. Это нужно чтобы удалить некоторые картинки цвет которых изменять не надо

    [SerializeField] private TextMeshProUGUI[] _textsInScene;

    public int CounterVisualBackpack = 0;
    private int _counterTouch = 0;

    private void Awake()
    {
        _brazierPrefab = GameObject.FindGameObjectsWithTag("Brazier");
        _imageInScene = GameObject.FindObjectsOfType<Image>();
        _activeObjectsS = GameObject.FindObjectsOfType<ActiveObject>();
        _textsInScene = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        for (int i = 0; i < _imageInScene.Length; i++) // цикл сортировки массива <_imageInScene>
        {
            if (_imageInScene[i].tag != "Map") _imagesInScene.Add(_imageInScene[i]);
        }
        for (int i = 0; i < _imagesInScene.Count; i++) // цикл смены темы игры в соответсивии с последним сохранением игры
        {
            _imagesInScene[i].color = new Color(PlayerPrefs.GetFloat("ThemeColorR"), PlayerPrefs.GetFloat("ThemeColorG"), PlayerPrefs.GetFloat("ThemeColorB"), PlayerPrefs.GetFloat("ThemeColorA"));
        }
        for (int i = 0; i < _activeObjectsS.Length; i++) // цикл отключения некоторых UI панелей
        {
            _activeObjectsS[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameStateS != GameState.GameOver && GameStateS != GameState.InspectionObject)
        {
            if (Input.GetKeyDown(_keyCodeS.QuickMenu))
            {
                _uiButtonControllerS.QuickMenuCounter++;
                GameStateS = GameState.Pause;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _uIpanelS.OpenMenu(_uIpanelS.QuickMenu);
                if (_uiButtonControllerS.QuickMenuCounter == 2)
                {
                    _uiButtonControllerS.QuickMenuCounter = 0;
                    GameStateS = GameState.Game;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    StartCoroutine(_uIpanelS.CloseMenu(0.2f, _uIpanelS.QuickMenu, _uIpanelS.QuickMenu.GetComponent<Animator>(), "DeActive"));
                }
            }
            if (Input.GetKeyDown(_keyCodeS.VisualBackPack) && _uIpanelS.QuickMenu.activeInHierarchy == false)
            {
                CounterVisualBackpack++;
                _uIpanelS.OpenMenu(_uIpanelS.QuickMenuBackPack);
                GameStateS = GameState.ViewingTheMap;
                if (CounterVisualBackpack == 2)
                {
                    GameStateS = GameState.Game;
                    StartCoroutine(_uIpanelS.CloseMenu(0.2f, _uIpanelS.QuickMenuBackPack, _uIpanelS.QuickMenuBackPack.GetComponent<Animator>(), "DeActive"));
                    CounterVisualBackpack = 0;
                }
            }
        }

        if (_playerS.GetComponent<HealthPlayer>().CurrentHP <= 0)
        {
            GameStateS = GameState.GameOver;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameraPlayer camera = _playerS.GetComponentInChildren<CameraPlayer>();
            Animator animator = camera.GetComponent<Animator>();
            animator.SetBool("Death", true);
            _uIpanelS.DeathPanel.SetActive(true);
            _uIBlurS = _uIpanelS.DeathPanel.GetComponent<UIBlur>();
            StartCoroutine(_uIBlurS.BeginBlurCoroutine(0.05f));
        }
    }

    public void ChangeThemeGame(Color newColor)
    {
        Color color = new Color(newColor.r, newColor.g, newColor.b, 0.5f);
        PlayerPrefs.SetFloat("ThemeColorR", newColor.r);
        PlayerPrefs.SetFloat("ThemeColorG", newColor.g);
        PlayerPrefs.SetFloat("ThemeColorB", newColor.b);
        PlayerPrefs.SetFloat("ThemeColorA", newColor.a);
        for (int i = 0; i < _imagesInScene.Count; i++)
        {
            _imagesInScene[i].color = newColor;
        }
        for (int i = 0; i < _textsInScene.Length; i++)
        {
            _textsInScene[i].fontSharedMaterial.SetColor("_UnderlayColor", color);
        }
    }

    private void OnLevelWasLoaded(int level)
    {

    }
}
