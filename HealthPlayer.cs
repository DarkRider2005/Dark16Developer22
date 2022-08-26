using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private PlayerAudio _playerAudioS;
    private Player _playerS;

    public float FullHP { get { return _fullHP; } }
    public float FullER { get { return _fullER; } }
    public float HalfHP { get { return _halfHP; } }
    public float AmountReplenushedHP { get { return _amountReplenushedHP; } }
    public float CurrentHP { get { return _currentHP; } set { _currentHP = value; } }
    public float CurrentER { get { return _currentER; } set { _currentER = value; } }

    [SerializeField] private float _changeERRun;
    private float _recoverySpeedHP = 10f;
    private float _recoverySpeedER = 30f;
    private float _fullHP = 200f, _fullER = 200f, _halfHP = 100f, _amountReplenushedHP = 80f, _currentHP = 1, _currentER = 200;

    private void Start()
    {
        _playerS = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (_currentHP < _halfHP) StartCoroutine(RestoringHP());
        if (_currentHP < _halfHP / 2)
        {
            _playerS.LifePlayerAudioSource.volume = 0.7f;
            _playerAudioS.PlayAudio(_playerS.LifePlayerAudioSource, _playerAudioS.HeartbeatAudioClip);
        }
        else if (_currentHP > _halfHP / 2 && !_playerS.RecoveryER) _playerAudioS.StopAudio(_playerS.LifePlayerAudioSource);
        if (_currentER < _fullER && _playerS.RecoveryER) StartCoroutine(RestoringER());
        else if ((!_playerS.RecoveryER || _currentER == _fullER) && _currentHP > _halfHP / 2) _playerAudioS.StopAudio(_playerS.LifePlayerAudioSource);
    }
    /// <summary>
    /// Получение урона игроком
    /// </summary>
    /// <param name="monsterDamage">урон</param>
    public void ChangeHP(float monsterDamage)
    {
        _currentHP -= monsterDamage;
    }
    /// <summary>
    /// Уменьшение выносливости игрока при беге
    /// </summary>
    public void ChangeHPInRun()
    {
        _currentER = Mathf.MoveTowards(_currentER, 0, _changeERRun * Time.deltaTime);
    }
    /// <summary>
    /// Востановление жизни игрока
    /// </summary>
    /// <returns></returns>
    public IEnumerator RestoringHP()
    {
        yield return new WaitForSeconds(2f);
        _currentHP = Mathf.MoveTowards(_currentHP, _halfHP, _recoverySpeedHP * Time.deltaTime);
    }
    /// <summary>
    /// Востановление выносливости игрока
    /// </summary>
    /// <returns></returns>
    public IEnumerator RestoringER()
    {
        yield return new WaitForSeconds(2f);
        _currentER = Mathf.MoveTowards(_currentER, _fullER, _recoverySpeedER * Time.deltaTime);
        _playerS.LifePlayerAudioSource.volume = 0.2f;
        _playerAudioS.PlayAudio(_playerS.LifePlayerAudioSource, _playerAudioS.ShortnessAudioClip);
    }
}