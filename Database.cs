using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Database : MonoBehaviour
{
    [SerializeField] private SaveObjects[] _objectInScene;
    public SaveObjects[] ObjectInScene { get { return _objectInScene; } set { _objectInScene = value; } }
    [SerializeField] private Monster[] _monsterInScene;
    public Monster[] MonsterInScene { get { return _monsterInScene; } set { _monsterInScene = value; } }
    [SerializeField] private Player _playerS;
    [SerializeField] private CameraPlayer _cameraPlayerS;
    [SerializeField] private UIWidget _uIWidgetS;
    [SerializeField] private EnvironmentAudio _environmentAudioS;
    [SerializeField] private Timer _timerS;
    [SerializeField] private Weapon[] _weaponsS;

    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform[] _savePoints;

    private int _numberOfGameLaunches = 0;

    private void Awake()
    {
        PlayerPrefs.SetFloat("SavePositionXPlayer", _startPoint.position.x);
        PlayerPrefs.SetFloat("SavePositionYPlayer", _startPoint.position.y);
        PlayerPrefs.SetFloat("SavePositionZPlayer", _startPoint.position.z);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("_numberOfGameLaunches") == 0) PlayerPrefs.SetInt("_numberOfGameLaunches", _numberOfGameLaunches);
        if (PlayerPrefs.GetInt("_numberOfGameLaunches") == 0)
        {
            _uIWidgetS.QuickMenuSentivityX.value = _playerS.SensitivityHor;
            _uIWidgetS.QuickMenuSentivityY.value = _cameraPlayerS.SensitivityVert;

            PlayerPrefs.SetFloat("EnvironmentMusicVolume", _uIWidgetS.QuickMenuVolumeMusic.value);
            _environmentAudioS.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EnvironmentMusicVolume");

            PlayerPrefs.SetInt("TimeMazeOne", 900);
            PlayerPrefs.SetInt("TimeMazeTwo", 1080);
            PlayerPrefs.SetInt("TimeMazeThree", 1320);

            _numberOfGameLaunches++;
            PlayerPrefs.SetInt("_numberOfGameLaunches", _numberOfGameLaunches);
        }
        if (PlayerPrefs.GetInt("_numberOfGameLaunches") >= 1)
        {
            _weaponsS = GameObject.FindObjectsOfType<Weapon>();
            for (int i = 0; i < _weaponsS.Length; i++)
            {
                if (_weaponsS[i].AvailabilityInBackpack == 1)
                {
                    _weaponsS[i].BindingBackpack();
                }
            }
            _playerS.SensitivityHor = PlayerPrefs.GetFloat("SentivityX");
            _cameraPlayerS.SensitivityVert = PlayerPrefs.GetFloat("SentivityY");
            _environmentAudioS.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("NewEnvironmentMusicVolume");
            _uIWidgetS.QuickMenuSentivityX.value = _playerS.SensitivityHor;
            _uIWidgetS.QuickMenuSentivityY.value = _cameraPlayerS.SensitivityVert;
            _uIWidgetS.QuickMenuVolumeMusic.value = _environmentAudioS.GetComponent<AudioSource>().volume;
            _numberOfGameLaunches++;
        }
    }

    private void Update()
    {

    }

    public void UpdateVolumeEnvironmentMusic()
    {
        PlayerPrefs.SetFloat("NewEnvironmentMusicVolume", _uIWidgetS.QuickMenuVolumeMusic.value);
        _environmentAudioS.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("NewEnvironmentMusicVolume");
    }

    public void UpdateSentivityX()
    {
        PlayerPrefs.SetFloat("SentivityX", _uIWidgetS.QuickMenuSentivityX.value);
        _playerS.SensitivityHor = PlayerPrefs.GetFloat("SentivityX");
    }

    public void UpdateSentivityY()
    {
        PlayerPrefs.SetFloat("SentivityY", _uIWidgetS.QuickMenuSentivityY.value);
        _cameraPlayerS.SensitivityVert = PlayerPrefs.GetFloat("SentivityY");
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("SavePositionXPlayer", _playerS.transform.position.x);
        PlayerPrefs.SetFloat("SavePositionYPlayer", _playerS.transform.position.y);
        PlayerPrefs.SetFloat("SavePositionZPlayer", _playerS.transform.position.z);
        _objectInScene = GameObject.FindObjectsOfType<SaveObjects>();
        _monsterInScene = GameObject.FindObjectsOfType<Monster>();
        for (int i = 0; i < ObjectInScene.Length; i++)
        {
            PlayerPrefs.SetFloat($"ObjectPositionX{i}", ObjectInScene[i].transform.position.x);
            PlayerPrefs.SetFloat($"ObjectPositionY{i}", ObjectInScene[i].transform.position.y);
            PlayerPrefs.SetFloat($"ObjectPositionZ{i}", ObjectInScene[i].transform.position.z);
        }
        for (int i = 0; i < MonsterInScene.Length; i++)
        {
            PlayerPrefs.SetInt($"MovementIndex{i}", MonsterInScene[i].MovementDirectionIndex);
            PlayerPrefs.SetFloat($"MonsterPositionX{i}", MonsterInScene[i].transform.position.x);
            PlayerPrefs.SetFloat($"MonsterPositionY{i}", MonsterInScene[i].transform.position.y);
            PlayerPrefs.SetFloat($"MonsterPositionZ{i}", MonsterInScene[i].transform.position.z);
        }
        PlayerPrefs.SetFloat("Time", _timerS.TimeLeft);
    }

    private void OnApplicationQuit()
    {

    }
}
