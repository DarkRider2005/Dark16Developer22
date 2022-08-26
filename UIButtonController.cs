using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
    private UIPanel _uiPanelS;
    private UIWidget _uiWidgetS;
    [SerializeField] private Player _playerS;
    [SerializeField] private CameraPlayer _cameraPlayerS;
    [SerializeField] private GameController _gameControllerS;
    [SerializeField] private Database _databaseS;

    [SerializeField] private GameObject _databaseObject;

    private Color BlueThemesColor;
    private Color OrangeThemesColor;
    private Color GreenThemesColor;
    private Color RedThemesColor;

    private int _counter = 0;
    [HideInInspector] public int QuickMenuCounter;

    private bool _fullScreen;

    private void Start()
    {
        BlueThemesColor = new Color(0f, 0.6677432f, 1f, 1f);
        OrangeThemesColor = new Color(1f, 0.8589724f, 0.4575472f, 1f);
        GreenThemesColor = new Color(0.6558253f, 1f, 0.6179246f, 1f);
        RedThemesColor = new Color(1f, 0.4492382f, 0.4386792f, 1f);

        _uiPanelS = GetComponent<UIPanel>();
        _uiWidgetS = GetComponent<UIWidget>();
    }

    public void PermissionObjectEjection()
    {
        _cameraPlayerS.PermissionRotationCam = true;
        _playerS.PermissionRotation = true;

        _playerS.PermissionDiscardObject = true;
        StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.ConfirmationObjectEjection, _uiPanelS.ConfirmationObjectEjection.GetComponent<Animator>(), "DeActive"));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProhibitObjectEjection()
    {
        _cameraPlayerS.PermissionRotationCam = true;
        _playerS.PermissionRotation = true;

        _playerS.PermissionDiscardObject = false;
        StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.ConfirmationObjectEjection, _uiPanelS.ConfirmationObjectEjection.GetComponent<Animator>(), "DeActive"));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ContinueSavePoint(GameObject button) // метод для продолжения с последней точки сохранения игры после смерти игрока
    {
        QuickMenuCounter = 0;
        button.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (_uiPanelS.QuickMenu.activeInHierarchy) StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenu, _uiPanelS.QuickMenu.GetComponent<Animator>(), "DeActive"));
        StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.DeathPanel, _uiPanelS.DeathPanel.GetComponent<Animator>(), "DeActive"));
        _playerS.GetComponent<HealthPlayer>().CurrentHP = _playerS.GetComponent<HealthPlayer>().HalfHP;
        _gameControllerS.GameStateS = GameController.GameState.Game;
        UIBlur uIBlur = _uiPanelS.DeathPanel.GetComponent<UIBlur>();
        StartCoroutine(uIBlur.EndBlurCoroutine(1f));
        _uiPanelS.DeathPanel.SetActive(false);
        _cameraPlayerS.GetComponent<Animator>().SetBool("Death", false);
        _playerS.transform.position = new Vector3(PlayerPrefs.GetFloat("SavePositionXPlayer"), PlayerPrefs.GetFloat("SavePositionYPlayer"), PlayerPrefs.GetFloat("SavePositionZPlayer"));
    }

    public void QuickMenuContinue()
    {
        QuickMenuCounter = 0;
        StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenu, _uiPanelS.QuickMenu.GetComponent<Animator>(), "DeActive"));
        if (_uiPanelS.QuickMenuBackPack.activeInHierarchy)
        {
            StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenuBackPack, _uiPanelS.QuickMenuBackPack.GetComponent<Animator>(), "DeActive"));
            _gameControllerS.CounterVisualBackpack = 0;
        }
        _gameControllerS.GameStateS = GameController.GameState.Game;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuickMenuBackpack()
    {
        if (_uiPanelS.QuickMenuBackPack.activeInHierarchy == false) _uiPanelS.OpenMenu(_uiPanelS.QuickMenuBackPack);
        else if (_uiPanelS.QuickMenuBackPack.activeInHierarchy == true) StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenuBackPack, _uiPanelS.QuickMenuBackPack.GetComponent<Animator>(), "DeActive"));
    }

    public void QuickMenuSettings()
    {
        _counter++;
        _uiPanelS.OpenMenu(_uiPanelS.QuickMenuSettings);
        if (_counter == 2)
        {
            _counter = 0;
            StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenuSettings, _uiPanelS.QuickMenuSettings.GetComponent<Animator>(), "DeActive"));
        }
    }
    public void QuickMenuResolutionDropdown()
    {
        if (PlayerPrefs.GetInt("FSSwitch") == 0) _fullScreen = false;
        else if (PlayerPrefs.GetInt("FSSwitch") == 1) _fullScreen = true;
        if (_uiWidgetS.ResolutionDropdown.value == 0)
        {
            Screen.SetResolution(1366, 766, _fullScreen);
            PlayerPrefs.SetInt("NewResolutionW", Screen.width);
            PlayerPrefs.SetInt("NewResolutionH", Screen.height);
            PlayerPrefs.SetInt("ValueResolutionDropdown", _uiWidgetS.ResolutionDropdown.value);
        }
        if (_uiWidgetS.ResolutionDropdown.value == 1)
        {
            Screen.SetResolution(1920, 1080, _fullScreen);
            PlayerPrefs.SetInt("NewResolutionW", Screen.width);
            PlayerPrefs.SetInt("NewResolutionH", Screen.height);
            PlayerPrefs.SetInt("ValueResolutionDropdown", _uiWidgetS.ResolutionDropdown.value);
        }
        if (_uiWidgetS.ResolutionDropdown.value == 2)
        {
            Screen.SetResolution(2560, 1440, _fullScreen);
            PlayerPrefs.SetInt("NewResolutionW", Screen.width);
            PlayerPrefs.SetInt("NewResolutionH", Screen.height);
            PlayerPrefs.SetInt("ValueResolutionDropdown", _uiWidgetS.ResolutionDropdown.value);
        }
        if (_uiWidgetS.ResolutionDropdown.value == 3)
        {
            Screen.SetResolution(3840, 2160, _fullScreen);
            PlayerPrefs.SetInt("NewResolutionW", Screen.width);
            PlayerPrefs.SetInt("NewResolutionH", Screen.height);
            PlayerPrefs.SetInt("ValueResolutionDropdown", _uiWidgetS.ResolutionDropdown.value);
        }
    }

    public void DropdownThemes()
    {
        if (_uiWidgetS.ThemeDropdown.value == 0)
        {
            _gameControllerS.ChangeThemeGame(BlueThemesColor);
            PlayerPrefs.SetInt("DropdownThemesValue", _uiWidgetS.ThemeDropdown.value);
        }
        if (_uiWidgetS.ThemeDropdown.value == 1)
        {
            _gameControllerS.ChangeThemeGame(OrangeThemesColor);
            PlayerPrefs.SetInt("DropdownThemesValue", _uiWidgetS.ThemeDropdown.value);
        }
        if (_uiWidgetS.ThemeDropdown.value == 2)
        {
            _gameControllerS.ChangeThemeGame(GreenThemesColor);
            PlayerPrefs.SetInt("DropdownThemesValue", _uiWidgetS.ThemeDropdown.value);
        }
        if (_uiWidgetS.ThemeDropdown.value == 3)
        {
            _gameControllerS.ChangeThemeGame(RedThemesColor);
            PlayerPrefs.SetInt("DropdownThemesValue", _uiWidgetS.ThemeDropdown.value);
        }
    }

    public void QuickMenuExit()
    {
        _uiPanelS.OpenMenu(_uiPanelS.QuickMenuExitConfirmation);
    }
    public void QuickMenuExitConfirmationTrue()
    {
        _databaseS.SaveGame();
        SceneManager.LoadScene(0);
    }
    public void QuickMenuExitConfirmationFalse()
    {
        StartCoroutine(_uiPanelS.CloseMenu(0.2f, _uiPanelS.QuickMenuExitConfirmation, _uiPanelS.QuickMenuExitConfirmation.GetComponent<Animator>(), "DeActive"));
    }
}
