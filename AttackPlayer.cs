using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private UIWidget _uIWidgetS;
    [SerializeField] private KeyCodeS _keyCodeS;
    [SerializeField] private GameController _gameControllerS;
    private BackPack _backPackS;
    private HealthPlayer _healthPlayerS;
    private Player _playerS;
    private HandPlayer _handPlayerS;
    private CameraPlayer _cameraPlayerS;

    private Animator _animator;
    [SerializeField] private SkinnedMeshRenderer[] _legSkinnedMeshRenderers;

    private bool _isAttack = false;
    private bool _handAttack = false;
    private bool _weaponAttack = false;
    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool HandAttacks { get { return _handAttack; } set { _handAttack = value; } }
    public bool WeaponAttacks { get { return _weaponAttack; } set { _weaponAttack = value; } }

    [SerializeField] private string _crossAnimName;

    [SerializeField] private float _delayInDisablingHandDamage;

    private void Start()
    {
        _backPackS = GetComponentInChildren<BackPack>();
        _handPlayerS = GetComponentInChildren<HandPlayer>();
        _playerS = GetComponent<Player>();
        _cameraPlayerS = GetComponentInChildren<CameraPlayer>();
        _healthPlayerS = GetComponent<HealthPlayer>();

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_gameControllerS.GameStateS == GameController.GameState.Game)
        {
            if (Input.GetMouseButtonUp(0) && !_playerS.Run && !_playerS.PermissionRotation)
            {
                if ((_backPackS.ColdWeapon.Count == 1 && _backPackS.ColdWeapon[0].activeInHierarchy) || (_backPackS.ColdWeapon.Count == 2 && (_backPackS.ColdWeapon[0].activeInHierarchy ||
                    _backPackS.ColdWeapon[1].activeInHierarchy))) Weapon();

                if (_backPackS.ThrowingWeapons.Count > 0 && _backPackS.ThrowingWeapons[0].activeInHierarchy) Throw();

                if (((_backPackS.ColdWeapon.Count == 0) || (_backPackS.ColdWeapon.Count == 1 && !_backPackS.ColdWeapon[0].activeInHierarchy) || (_backPackS.ColdWeapon.Count == 2
                 && !_backPackS.ColdWeapon[0].activeInHierarchy&& !_backPackS.ColdWeapon[1].activeInHierarchy)) && (_backPackS.ThrowingWeapons.Count == 0 ||
                 _backPackS.ThrowingWeapons.Count > 0 && !_backPackS.ThrowingWeapons[0].activeInHierarchy)) Cross();
            }

            if (Input.GetKeyDown(_keyCodeS.Kick))
            {
                if (!_playerS.Run) Kick();
            }
            if (_weaponAttack) StartCoroutine(DelayInDisablingWeaponDamage());
            if (_handAttack) StartCoroutine(DelayInDisablingHandDamage());
        }
    }

    private void Weapon()
    {
        if (_backPackS.ColdWeapon.Count == 1 || (_backPackS.ColdWeapon.Count == 2 && _backPackS.ColdWeapon[1].activeInHierarchy == false))
        {
            Weapon weapon = _backPackS.ColdWeapon[0].GetComponent<Weapon>();
            if (_backPackS.ColdWeapon[0].activeInHierarchy && _healthPlayerS.CurrentER >= weapon.WChangeER)
            {
                _isAttack = true;
                _weaponAttack = true;
                _handAttack = false;
                _healthPlayerS.CurrentER -= weapon.WChangeER;
                _animator.Play(weapon.AttackAnimName);
            }

            else if (_healthPlayerS.CurrentER < weapon.WChangeER)
            {
                _isAttack = false;
                _weaponAttack = false;
                StartCoroutine(ForbiddenActionText());
            }
        }

        if (_backPackS.ColdWeapon.Count == 2)
        {
            Weapon weapon = _backPackS.ColdWeapon[1].GetComponent<Weapon>();
            if (_backPackS.ColdWeapon[1].activeInHierarchy && _healthPlayerS.CurrentER >= weapon.WChangeER)
            {
                _isAttack = true;
                _weaponAttack = true;
                _handAttack = false;
                _healthPlayerS.CurrentER -= weapon.WChangeER;
                _animator.Play(weapon.AttackAnimName);
            }
            else if (_healthPlayerS.CurrentER < weapon.WChangeER)
            {
                _isAttack = false;
                _weaponAttack = false;
                StartCoroutine(ForbiddenActionText());
            }
        }
    }

    private void Throw()
    {
        _animator.Play(_backPackS.ThrowingWeapons[0].GetComponent<Weapon>().AttackAnimName);
        StartCoroutine(ThrowWeapon());
    }

    private void Cross()
    {
        _animator.Play(_crossAnimName);
        _isAttack = true;
        _weaponAttack = false;
        _handAttack = true;
    }

    private void Kick()
    {
        for (int i = 0; i < _legSkinnedMeshRenderers.Length; i++)
        {
            _legSkinnedMeshRenderers[i].enabled = true;
        }
        LegPlayer legPlayer = GetComponentInChildren<LegPlayer>();
        _animator.SetTrigger(legPlayer._kickAnimName);
        StartCoroutine(LegSkinnedMeshRenderersOff());
    }

    private IEnumerator ForbiddenActionText()
    {
        _uIWidgetS.ForbiddenAction.text = _uIWidgetS.NotEnoughStaminaToAttack;
        yield return new WaitForSeconds(1.5f);
        _uIWidgetS.ForbiddenAction.text = null;
    }

    private IEnumerator DelayInDisablingWeaponDamage()
    {
        Weapon weaponOne;
        Weapon weaponTwo;
        if (_backPackS.ColdWeapon.Count == 1)
        {
            weaponOne = _backPackS.ColdWeapon[0].GetComponent<Weapon>();
            yield return new WaitForSeconds(weaponOne.DelayInDisablingDamage);
            _isAttack = false;
            _weaponAttack = false;
        }
        else if (_backPackS.ColdWeapon.Count == 2)
        {
            weaponOne = _backPackS.ColdWeapon[0].GetComponent<Weapon>();
            weaponTwo = _backPackS.ColdWeapon[1].GetComponent<Weapon>();
            if (_backPackS.ColdWeapon[0].activeInHierarchy == true)
            {
                yield return new WaitForSeconds(weaponOne.DelayInDisablingDamage);

            }
            else if (_backPackS.ColdWeapon[1].activeInHierarchy == true)
            {
                yield return new WaitForSeconds(weaponTwo.DelayInDisablingDamage);
                _isAttack = false;
                _weaponAttack = false;
            }
        }
    }

    private IEnumerator DelayInDisablingHandDamage()
    {
        yield return new WaitForSeconds(_delayInDisablingHandDamage);
        _isAttack = false;
        _handAttack = false;
    }

    private IEnumerator ThrowWeapon()
    {
        //   yield return new WaitForSeconds(0.5f); _backPackS.ThrowingWeapons[0].transform.parent = null;
        //   _backPackS.ThrowingWeapons[0].GetComponent<Rigidbody>().isKinematic = false;
        ///    _backPackS.ThrowingWeapons[0].transform.position = Vector3.Lerp(transform.position, _cameraPlayerS.Hit.transform.position, 100f * Time.deltaTime);
        //   _backPackS.ThrowingWeapons.Remove(_backPackS.ThrowingWeapons[0]);
        return null;
    }

    private IEnumerator LegSkinnedMeshRenderersOff()
    {
        yield return new WaitForSeconds(1.3f);
        for (int i = 0; i < _legSkinnedMeshRenderers.Length; i++)
        {
            _legSkinnedMeshRenderers[i].enabled = false;
        }
    }
}
