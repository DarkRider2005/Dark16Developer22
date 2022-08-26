using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Database _databaseS;
    private Timer _timerS;
    private OptionsMenuNew _optionsMenuS;
    private Player _playerS;

    private int sd = 0;
    private int _numbersGameLaunche = 0;

    private bool _fullScreen;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("sd", sd) == 0)
        {
            DontDestroyOnLoad(this);
            sd++;
            PlayerPrefs.SetInt("sd", sd);
        }
        if (PlayerPrefs.GetInt("_numbersGameLaunche") == 0)
        {
            PlayerPrefs.SetInt("_numbersGameLaunche", _numbersGameLaunche);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            _optionsMenuS = GameObject.FindObjectOfType<OptionsMenuNew>();
            _optionsMenuS.ResolutionDropDown.value = PlayerPrefs.GetInt("ValueResolutionDropdown");
            if (PlayerPrefs.GetInt("FSSwitch") == 0) _fullScreen = false;
            if (PlayerPrefs.GetInt("FSSwitch") == 1) _fullScreen = true;
            Screen.SetResolution(PlayerPrefs.GetInt("NewResolutionW"), PlayerPrefs.GetInt("NewResolutionW"), _fullScreen);
        }
        if (level == 1)
        {
            Screen.SetResolution(PlayerPrefs.GetInt("NewResolutionH"), PlayerPrefs.GetInt("NewResolutionW"), Screen.fullScreen);
            _numbersGameLaunche++;
            PlayerPrefs.SetInt("_numbersGameLaunche", _numbersGameLaunche);
            if (PlayerPrefs.GetInt("_numbersGameLaunche") > 1)
            {
                _playerS = GameObject.FindObjectOfType<Player>();
                _databaseS = GameObject.FindObjectOfType<Database>();
                _timerS = GameObject.FindObjectOfType<Timer>();
                _databaseS.ObjectInScene = GameObject.FindObjectsOfType<SaveObjects>();
                _databaseS.MonsterInScene = GameObject.FindObjectsOfType<Monster>();
                _playerS.transform.position = new Vector3(PlayerPrefs.GetFloat("SavePositionXPlayer"), PlayerPrefs.GetFloat("SavePositionYPlayer"), PlayerPrefs.GetFloat("SavePositionZPlayer"));
                for (int i = 0; i < _databaseS.ObjectInScene.Length; i++)
                {
                    _databaseS.ObjectInScene[i].transform.position = new Vector3(PlayerPrefs.GetFloat($"ObjectPositionX{i}"), PlayerPrefs.GetFloat($"ObjectPositionY{i}"),
                        PlayerPrefs.GetFloat($"ObjectPositionZ{i}"));
                }
                for (int i = 0; i < _databaseS.MonsterInScene.Length; i++)
                {
                    _databaseS.MonsterInScene[i].transform.position = new Vector3(PlayerPrefs.GetFloat($"MonsterPositionX{i}"), PlayerPrefs.GetFloat($"MonsterPositionY{i}"), PlayerPrefs.GetFloat($"MonsterPositionZ{i}"));
                    _databaseS.MonsterInScene[i].MovementDirectionIndex = PlayerPrefs.GetInt($"MovementIndex{i}");
                }
            }
        }
    }
}
