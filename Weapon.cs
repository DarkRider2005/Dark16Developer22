using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponQuality
    {
        Simple,
        Rare,
        Epic
    }
    public enum WeaponType
    {
        Cold,
        Propellat,
        Remote
    }

    public WeaponQuality WeaponQualityS;
    public WeaponType WeaponTypeS;

    [SerializeField] private UIWidget _uIWidgetS;
    [SerializeField] private Player _playerS;
    [SerializeField] private BackPack _backPackS;
    private AttackPlayer _attackPlayerS;
    private HpObject _hpObjectS;
    private Monster _monsterS;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Light PointLights { get { return _pointLight; } set { _pointLight.intensity = value.intensity; } }
    private Rigidbody _rigidbody;
    private Light _pointLight;
    private Collider[] _mainColliders;

    [SerializeField] private Transform _weaponPositionHand;

    public bool PossibilityOfInteraction { get { return _possibilityOfInteraction; } set { _possibilityOfInteraction = value; } }
    private bool _possibilityOfInteraction = true;

    public int MinDamage;
    public int MaxDamage;
    public int AvailabilityInBackpack { get; set; }
    private int _objectTouchCounter;
    private int _numberOfGameLaunches = 0;

    public float WEndurance { get { return _wEndurance; } set { _wEndurance = value; } }
    public float WChangeER { get { return _wChangeER; } }
    public float Discardforce { get { return _discardForce; } }
    public float IntensityLight { get; set; }
    public float WeaponImageScaleX { get { return _weaponImageScaleX; } }
    public float WeaponImageScaleXVisualBackPack { get { return _weaponImageScaleXVisualBackPack; } }
    public float WeaponImageScaleYVisualBackPack { get { return _weaponImageScaleYVisualBackPack; } }
    public float DelayInDisablingDamage { get { return _delayInDisablingDamage; } }
    private float _damage;
    private float _halfEndurance;
    private float _quarterEndurance;
    [SerializeField] private float _wEndurance, _wChangeER, _discardForce, _weaponImageScaleX;
    [SerializeField] private float _weaponImageScaleXVisualBackPack, _weaponImageScaleYVisualBackPack, _delayInDisablingDamage;

    public string AttackAnimName;
    public string WeaponQualityRus { get; set; }
    public string WeaponTypeRus { get; set; }

    public List<GameObject> ComponentsOfTheObjects { get { return _componentsOfTheObjects; } }
    private List<GameObject> _componentsOfTheObjects = new List<GameObject>();

    private Color _colorLight; // цвет подстветки оружи€

    private void Awake()
    {

    }

    private void Start()
    {
        _mainColliders = GetComponents<Collider>();
        for (int i = 0; i < _mainColliders.Length; i++)
        {
            Physics.IgnoreCollision(_mainColliders[i], _playerS.GetComponent<Collider>());
        }
        switch (WeaponQualityS) 
        {
            case WeaponQuality.Simple:
                WeaponQualityRus = "ѕростое";
                break;
            case WeaponQuality.Rare:
                WeaponQualityRus = "–едкое";
                break;
            case WeaponQuality.Epic:
                WeaponQualityRus = "Ёпическое";
                break;
        }
        switch (WeaponTypeS)
        {
            case WeaponType.Cold:
                WeaponTypeRus = "’олодное";
                break;
            case WeaponType.Propellat:
                WeaponTypeRus = "ћетательное";
                break;
            case WeaponType.Remote:
                WeaponTypeRus = "ќгнестрельное";
                break;
        } 
        SetColorAndIntensityLight();

        _halfEndurance = WEndurance / 2;
        _quarterEndurance = WEndurance / 4;

        _rigidbody = GetComponent<Rigidbody>();
        _pointLight = GetComponentInChildren<Light>();
        _pointLight.intensity = IntensityLight;
        _pointLight.color = _colorLight;
    }

    private void Update()
    {
        _attackPlayerS = GetComponentInParent<AttackPlayer>(); //желательно от этого избавитьс€
        if (_attackPlayerS == null) return;
        SetColorEnduranceText();
        if (!_possibilityOfInteraction) IntensityLight = 0; // Ёто условие выполнитс€ только если объект этого класса будет находитс€ в рюкзаке (класс BackPack)
    }

    public void BindingHand()
    {
        transform.SetParent(_weaponPositionHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        _rigidbody.MovePosition(_weaponPositionHand.position);
        _rigidbody.isKinematic = true;
    }

    public void BindingBackpack()
    {
        if (WeaponTypeS == WeaponType.Cold)
        {
            transform.SetParent(_weaponPositionHand);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _possibilityOfInteraction = false;

            _rigidbody.MovePosition(_weaponPositionHand.position);
            _rigidbody.isKinematic = true;
            _backPackS.ColdWeapon.Add(gameObject);
            _backPackS.SetPictureWeaponMenu(_uIWidgetS.PictureFirstWeapon, null);
            _backPackS.SetNameInWeaponMenu(_uIWidgetS.FirstWeaponName, GetComponent<ObjectName>());
            _backPackS.ColdWeapon[0].SetActive(false);
            if (_backPackS.ColdWeapon.Count == 2)
            {
                _backPackS.SetPictureWeaponMenu(null, _uIWidgetS.PictureSecondWeapon);
                _backPackS.SetNameInWeaponMenu(_uIWidgetS.SecondWeaponName, GetComponent<ObjectName>());
                _backPackS.ColdWeapon[1].SetActive(false);
            }
        }
    }

    public void ChangingTheLayerWeapon() // метод дл€ перемещени€ состaвл€ющих оружи€ на слой Weapon
    {
        gameObject.layer = 8;
        for (int i = 0; i < _componentsOfTheObjects.Count; i++)
        {
            _componentsOfTheObjects[i].layer = 8;
        }
    }

    public void ChangingTheLayerDefault() //метод дл€ перемещени€ состaвл€ющих оружи€ на слой Default
    {
        gameObject.layer = 0;
        for (int i = 0; i < _componentsOfTheObjects.Count; i++)
        {
            _componentsOfTheObjects[i].layer = 0;
        }
    }

    private void SetColorAndIntensityLight()
    {
        switch (WeaponQualityS)
        {
            case WeaponQuality.Simple:
                _colorLight = new Color(0f, 1f, 0f, 0.5f);
                IntensityLight = 2f;
                break;

            case WeaponQuality.Rare:
                _colorLight = new Color(0f, 0.4f, 1f, 0.5f);
                IntensityLight = 3f;
                break;

            case WeaponQuality.Epic:
                _colorLight = new Color(1f, 1f, 0f, 0.5f);
                IntensityLight = 5f;
                break;
        }
    }

    private void SetColorEnduranceText()
    {
        if (_wEndurance <= _halfEndurance) _uIWidgetS.EnduranceWeapon.color = Color.yellow;
        if (_wEndurance <= _quarterEndurance) _uIWidgetS.EnduranceWeapon.color = Color.red;
    }

    private void WhenEnduranceWeaponZero()
    {
        AttackPlayer attackPlayer = GameObject.FindObjectOfType<AttackPlayer>();
        attackPlayer.IsAttack = false;
        attackPlayer.WeaponAttacks = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("DestroyEnvironment"))
        {
            if (_attackPlayerS != null)
            {
                if (_attackPlayerS.IsAttack && _attackPlayerS.WeaponAttacks)
                {
                    _objectTouchCounter++;
                    if (_objectTouchCounter <= 1)
                    {
                        StartCoroutine(_uIWidgetS.ChangeColorEnduranceWeaponTextWhenAttack());

                        _damage = Random.Range(MinDamage, MaxDamage);
                        _uIWidgetS.InformationAboutImpacts.text = $"-{_damage}";
                        if (_uIWidgetS.InformationAboutImpacts.color.a < 1f) _uIWidgetS.InformationAboutImpacts.color = Color.green;

                        _hpObjectS = other.gameObject.GetComponent<HpObject>();
                        _hpObjectS.ChangeHP(_damage);
                        _hpObjectS.ChangeEnduranceWeapon = Random.Range(_hpObjectS.MinChangeEnduranceWeapon, _hpObjectS.MaxChangeEnduranceWeapon);
                        _hpObjectS.AddingPhysicsObjects();

                        _wEndurance -= _hpObjectS.ChangeEnduranceWeapon;

                        if (_hpObjectS.HP <= 0)
                        {
                            _attackPlayerS.IsAttack = false;
                            _attackPlayerS.WeaponAttacks = false;
                            _objectTouchCounter = 0;
                            if (_hpObjectS.BoxCollider != null) _hpObjectS.BoxCollider.enabled = false;
                            if (_hpObjectS.CapsuleCollider != null) _hpObjectS.CapsuleCollider.enabled = false;
                        }
                    }
                }
                else return;
            }
        }
        if (other.gameObject.CompareTag("Monster"))
        {
            if (_attackPlayerS != null)
            {
                if (_attackPlayerS.IsAttack && _attackPlayerS.WeaponAttacks)
                {
                    _objectTouchCounter++;
                    if (_objectTouchCounter <= 1)
                    {
                        StartCoroutine(_uIWidgetS.ChangeColorEnduranceWeaponTextWhenAttack());
                        _damage = Random.Range(MinDamage, MaxDamage);
                        _uIWidgetS.InformationAboutImpacts.text = $"-{_damage}";
                        if (_uIWidgetS.InformationAboutImpacts.color.a < 1f) _uIWidgetS.InformationAboutImpacts.color = Color.green;

                        _monsterS = other.gameObject.GetComponent<Monster>();
                        Rigidbody rb = _monsterS.GetComponent<Rigidbody>();
                        rb.freezeRotation = false;
                        //    rb.AddForce(_monsterS.HitObject.transform.position * sd, ForceMode.Force);
                        //    rb.AddTorque(transform.position * sd, ForceMode.Impulse);
                        _monsterS.ChangeHPMonster(_damage);
                        _monsterS.ChangeEnduranceWeaponM = Random.Range(_monsterS.MinChangeEnduranceWeaponM, _monsterS.MaxChangeEnduranceWeaponM);
                        _wEndurance -= _monsterS.ChangeEnduranceWeaponM;
                    }

                    if (_monsterS.HP <= 0)
                    {
                        _attackPlayerS.IsAttack = false;
                        _attackPlayerS.WeaponAttacks = false;
                    }

                    if (_wEndurance <= 0) WhenEnduranceWeaponZero();
                }
                else return;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("DestroyEnvironment"))
        {
            if (_attackPlayerS != null)
            {
                _objectTouchCounter = 0;
                _attackPlayerS.IsAttack = false;
                _attackPlayerS.WeaponAttacks = false;
                if (WEndurance <= 0) WhenEnduranceWeaponZero();
            }
        }
        if (other.gameObject.CompareTag("Monster"))
        {
            if (_attackPlayerS != null)
            {
                _objectTouchCounter = 0;
                _attackPlayerS.IsAttack = false;
                _attackPlayerS.WeaponAttacks = false;
                if (_wEndurance <= 0) WhenEnduranceWeaponZero();
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt($"AvailabilityInBackpack{GetComponent<ObjectName>().ObjectNameS}", AvailabilityInBackpack);
    }
}
