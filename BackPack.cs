using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackPack : BACKpack
{
    [SerializeField] private Player _playerS;
    [SerializeField] private CameraPlayer _cameraPlayerS;
    [SerializeField] private AttackPlayer _attackPlayerS;
    [SerializeField] private KeyCodeS _keyCodeS;
    [SerializeField] private UIWidget _uiWidgetS;
    [SerializeField] private UIPanel _uiPanelS;
    private Weapon _weaponOne; // этот объект скрипта для ColdWeapon[0] 
    private Weapon _weaponTwo; // этот объект скрипта для ColdWeapon[1] 
    private Weapon _throwingWeaponOne;          // этот объект скрипта для ThrowingWeapons[0,1,2] 
    private ObjectName _objectNameOne; // аналогично _weaponOne
    private ObjectName _objectNameTwo; // аналогично _weaponTwo
    private ObjectName _throwingObjectNameOne;  // аналогично _throwingWeaponOne

    [HideInInspector] public Color _backgroundWeaponColor;
    private Color _statusBarNormalColor;
    private Color _transparentWhiteColor;
    private Color _notTransparentWhiteColor;

    [SerializeField] private List<GameObject> _coldWeapon = new List<GameObject>();
    public List<GameObject> ColdWeapon { get { return _coldWeapon; } }
    [SerializeField] private List<GameObject> _throwingWeapons = new List<GameObject>();
    public List<GameObject> ThrowingWeapons { get { return _throwingWeapons; } }

    private void Start()
    {
        _statusBarNormalColor = new Color(_uiWidgetS.StatusBar.color.r, _uiWidgetS.StatusBar.color.g, _uiWidgetS.StatusBar.color.b, 1f);
        _transparentWhiteColor = new Color(1f, 1f, 1f, 0f);
        _notTransparentWhiteColor = new Color(1f, 1f, 1f, 1f);
    }

    private void Update()
    {
        if (ColdWeapon.Count == 1)
        {
            _weaponOne = ColdWeapon[0].GetComponent<Weapon>();
            _objectNameOne = ColdWeapon[0].GetComponent<ObjectName>();
        }
        if (ColdWeapon.Count == 2 && (ColdWeapon[0].activeInHierarchy || ColdWeapon[1].activeInHierarchy))
        {
            _weaponOne = ColdWeapon[0].GetComponent<Weapon>();
            _objectNameOne = ColdWeapon[0].GetComponent<ObjectName>();
            _weaponTwo = ColdWeapon[1].GetComponent<Weapon>();
            _objectNameTwo = ColdWeapon[1].GetComponent<ObjectName>();
        }
        if (ThrowingWeapons.Count > 0 && ThrowingWeapons[0].activeInHierarchy)
        {
            _throwingWeaponOne = ThrowingWeapons[0].GetComponent<Weapon>();
            _throwingObjectNameOne = ThrowingWeapons[0].GetComponent<ObjectName>();
            if (ThrowingWeapons.Count == 2) ThrowingWeapons[1].SetActive(false);
            if (ThrowingWeapons.Count == 3)
            {
                ThrowingWeapons[1].SetActive(false);
                ThrowingWeapons[2].SetActive(false);
            }
        }
        _backgroundWeaponColor = new Color(_uiWidgetS.EnduranceWeapon.color.r, _uiWidgetS.EnduranceWeapon.color.g, _uiWidgetS.EnduranceWeapon.color.b, 0.2f);

        if (ColdWeapon.Count > 0 || ThrowingWeapons.Count > 0)
        {
            WeaponsInTheBackpack();
            if ((ColdWeapon.Count == 1 && ColdWeapon[0].activeInHierarchy == true) || (ColdWeapon.Count == 2 && ColdWeapon[1].activeInHierarchy == false))
            {
                if (_weaponOne.WEndurance <= 0)
                    BreakingWeapons(_objectNameOne, ColdWeapon,
                        _uiWidgetS.FirstWeaponName, _uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.PictureFirstWeapon, 0);
            }
            if (ColdWeapon.Count == 2 && ColdWeapon[0].activeInHierarchy == false)
            {
                if (_weaponTwo.WEndurance <= 0)
                    BreakingWeapons(_objectNameTwo, ColdWeapon,
                        _uiWidgetS.SecondWeaponName, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.PictureSecondWeapon, 1);
            }
            if (ThrowingWeapons.Count > 0)
            {
                if (_throwingWeaponOne.WEndurance <= 0)
                    BreakingWeapons(_throwingObjectNameOne, ThrowingWeapons, _uiWidgetS.NameOfTheThrowingWeapon, _uiWidgetS.BackgroundThrowingWeapon, _uiWidgetS.PictureThrowingWeapon, 0);
            }
        }
    }

    private void WeaponsInTheBackpack()
    {
        switch (ColdWeapon.Count)
        {
            case 1:
                CaseOne();
                break;

            case 2:
                CaseTwo();
                break;
        }
        switch (ThrowingWeapons.Count)
        {
            case > 0:
                CaseGreaterThanZero();
                break;
        }
    }

    private void CaseOne() // !
    {
        if ((Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Alpha2)) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) && !_attackPlayerS.IsAttack)
            EnableAndDisableWeapon(0, 0, ColdWeapon);
        else if (Input.GetKey(KeyCode.Alpha1) && !_attackPlayerS.IsAttack)
        {
            EnableAndDisableWeapon(1, 0, ColdWeapon);
            UpdateStatusBar("Выбрано", _objectNameOne.ObjectNameS);
        }
        if (ColdWeapon[0].activeInHierarchy && Input.GetKey(_keyCodeS.DiscardObject)) ConfirmationReleaseWeapons(_objectNameOne);
        if (_playerS.PermissionDiscardObject) DiscardWeapon(_uiWidgetS.FirstWeaponName, 0, ColdWeapon, _weaponOne, _objectNameOne, _uiWidgetS.PictureFirstWeapon, _uiWidgetS.BackgroundFirstWeapon);
    }

    private void CaseTwo() // !
    {
        if ((Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Alpha4) && !_attackPlayerS.IsAttack)) EnableAndDisableWeapon(0, 0, ColdWeapon);
        else if (Input.GetKey(KeyCode.Alpha1) && !_attackPlayerS.IsAttack)
        {
            EnableAndDisableWeapon(1, 1, ColdWeapon);
            UpdateStatusBar("Выбрано", _objectNameOne.ObjectNameS);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && !_attackPlayerS.IsAttack)
        {
            EnableAndDisableWeapon(1, 0, ColdWeapon);
            UpdateStatusBar("Выбрано", _objectNameTwo.ObjectNameS);
        }

        if (ColdWeapon[0].activeInHierarchy && Input.GetKey(_keyCodeS.DiscardObject)) ConfirmationReleaseWeapons(_objectNameOne);
        if (ColdWeapon[0].activeInHierarchy && _playerS.PermissionDiscardObject)
            DiscardWeapon(_uiWidgetS.FirstWeaponName, 0, ColdWeapon, _weaponOne, _objectNameOne, _uiWidgetS.PictureFirstWeapon, _uiWidgetS.BackgroundFirstWeapon);

        else if (ColdWeapon[1].activeInHierarchy && Input.GetKey(_keyCodeS.DiscardObject)) ConfirmationReleaseWeapons(_objectNameTwo);
        if (ColdWeapon[1].activeInHierarchy && _playerS.PermissionDiscardObject)
            DiscardWeapon(_uiWidgetS.SecondWeaponName, 1, ColdWeapon, _weaponTwo, _objectNameTwo, _uiWidgetS.PictureSecondWeapon, _uiWidgetS.BackgroundSecondWeapon);
    }

    private void CaseGreaterThanZero() // !
    {
        if (Input.GetKey(KeyCode.Alpha3) && !_attackPlayerS.IsAttack)
        {
            EnableAndDisableWeapon(3, 0, _throwingWeapons);
            UpdateStatusBar("Выбрано", _throwingObjectNameOne.ObjectNameS);
        }
        else if (ThrowingWeapons.Count > 0 && ((Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha0)) || Input.GetKey(KeyCode.Alpha1)
                || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3)) && !_attackPlayerS.IsAttack) EnableAndDisableWeapon(2, 0, _throwingWeapons);
    }

    public void EnableAndDisableWeapon(int switchAction, int reverseActive, List<GameObject> weapons) // если switchAction = 0, то это отключение элемента, если 1, то это включение элемента 
    {
        switch (switchAction)
        {
            case 0:
                switch (weapons.Count)
                {
                    case 1:
                        weapons[0].SetActive(false);
                        ResetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon);
                        ResetTextParametersInWeaponMenu();
                        break;
                    case 2:
                        weapons[0].SetActive(false);
                        weapons[1].SetActive(false);
                        ResetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon);
                        ResetBackgroundWeaponMenu(_uiWidgetS.BackgroundSecondWeapon);
                        ResetTextParametersInWeaponMenu();
                        break;
                }
                break;
            case 1:
                switch (weapons.Count)
                {
                    case 1:
                        weapons[0].SetActive(true);
                        SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 0);
                        SetDamageTextInWeaponMenu(_weaponOne);
                        SetEnduranceTextInWeaponMenu(_weaponOne);
                        break;
                    case 2:
                        weapons[0].SetActive(false);
                        weapons[1].SetActive(true);
                        SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 1);
                        SetDamageTextInWeaponMenu(_weaponTwo);
                        SetEnduranceTextInWeaponMenu(_weaponTwo);
                        if (reverseActive == 1)
                        {
                            weapons[0].SetActive(true);
                            SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 0);
                            SetDamageTextInWeaponMenu(_weaponOne);
                            SetEnduranceTextInWeaponMenu(_weaponOne);
                            weapons[1].SetActive(false);
                        }
                        break;
                }
                break;
            case 2:
                weapons[0].SetActive(false);
                ResetBackgroundWeaponMenu(_uiWidgetS.BackgroundThrowingWeapon);
                ResetTextParametersInWeaponMenu();
                break;
            case 3:
                if (_coldWeapon.Count == 1 && _coldWeapon[0].activeInHierarchy) EnableAndDisableWeapon(0, 0, _coldWeapon);
                else if (_coldWeapon.Count == 2 && _coldWeapon[0].activeInHierarchy && _coldWeapon[1].activeInHierarchy) EnableAndDisableWeapon(0, 0, _coldWeapon);
                weapons[0].SetActive(true);
                SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 2);
                SetDamageTextInWeaponMenu(_throwingWeaponOne);
                SetEnduranceTextInWeaponMenu(_throwingWeaponOne);
                break;
        }
    }

    private void ConfirmationReleaseWeapons(ObjectName objectName)
    {
        _uiPanelS.OpenMenu(_uiPanelS.ConfirmationObjectEjection);
        _uiWidgetS.ConfirmationObjectEjection.text = $"Выбросить {objectName.ObjectNameS}";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerS.PermissionRotation = false;
        _cameraPlayerS.PermissionRotationCam = false;
    }

    private void UpdateStatusBar(string msg, string msgg)
    {
        _uiWidgetS.StatusBar.text = $"{msg} {msgg}";
        if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = _statusBarNormalColor;
    }

    private void BreakingWeapons(ObjectName objectName, List<GameObject> weapons, TextMeshProUGUI name, Image background, Image picture, int index)
    {
        weapons[index].transform.parent = null;
        weapons[index].GetComponent<Weapon>().AvailabilityInBackpack = 0;
        PlayerPrefs.SetInt($"AvailabilityInBackpack{weapons[index].GetComponent<ObjectName>().ObjectNameS}", weapons[index].GetComponent<Weapon>().AvailabilityInBackpack);
        _uiWidgetS.EnduranceWeapon.text = null;
        _uiWidgetS.DamageWeapon.text = null;
        _uiWidgetS.StatusBar.text = "Вы сломали " + objectName.ObjectNameS + "(-y)";
        if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = _statusBarNormalColor;
        name.text = null;
        background.color = _transparentWhiteColor;
        picture.color = _transparentWhiteColor;
        picture.sprite = null;
        weapons.Remove(weapons[index]);
    }

    private void DiscardWeapon(TextMeshProUGUI name, int index, List<GameObject> weapons, Weapon weapon, ObjectName objectName, Image p, Image bg)
    { // bg - это background , p - это picture
        Rigidbody rigidbody = weapons[index].GetComponent<Rigidbody>();
        weapons[index].transform.parent = null;
        weapon.AvailabilityInBackpack = 0;
        PlayerPrefs.SetInt($"AvailabilityInBackpack{objectName.ObjectNameS}", weapon.AvailabilityInBackpack);
        weapon.PossibilityOfInteraction = true;
        rigidbody.isKinematic = false;
        rigidbody.AddRelativeForce(Vector3.forward * weapon.Discardforce, ForceMode.Impulse);
        if (weapons == _coldWeapon)
        {
            p.sprite = null;
            p.color = _transparentWhiteColor;
            bg.color = _transparentWhiteColor;
            name.text = null;
            _uiWidgetS.EnduranceWeapon.text = null;
            _uiWidgetS.DamageWeapon.text = null;
        }
        else if (weapons == _throwingWeapons)
        {

        }
        _uiWidgetS.StatusBar.text = "Вы выбросили " + objectName.ObjectNameS + "(-y)";
        if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = _statusBarNormalColor;
        weapon.ChangingTheLayerDefault();
        int i = weapons.Count;
        weapons.Remove(weapons[index]);
        if (index == 0 && i == 2)
        {
            weapons[0].SetActive(true);
            SetPictureWeaponMenu(_uiWidgetS.PictureFirstWeapon, null);
            SetNameInWeaponMenu(_uiWidgetS.FirstWeaponName, weapons[0].GetComponent<ObjectName>());
            SetDamageTextInWeaponMenu(weapons[0].GetComponent<Weapon>());
            SetEnduranceTextInWeaponMenu(weapons[0].GetComponent<Weapon>());
            SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 0);
            _uiWidgetS.SecondWeaponName.text = null;
            _uiWidgetS.PictureSecondWeapon.sprite = null;
            _uiWidgetS.PictureSecondWeapon.color = _transparentWhiteColor;
        }
        else if (index == 1)
        {
            weapons[0].SetActive(true);
            SetPictureWeaponMenu(_uiWidgetS.PictureFirstWeapon, null);
            SetNameInWeaponMenu(_uiWidgetS.FirstWeaponName, weapons[0].GetComponent<ObjectName>());
            SetDamageTextInWeaponMenu(weapons[0].GetComponent<Weapon>());
            SetEnduranceTextInWeaponMenu(weapons[0].GetComponent<Weapon>());
            SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 0);
        }
        _playerS.PermissionDiscardObject = false;
    }

    public void SetPictureWeaponMenu(Image firstP, Image secondP) // P - это Picture
    {
        if (firstP != null)
        {
            RectTransform rect = firstP.GetComponent<RectTransform>();
            SpriteKeeper keeper = ColdWeapon[0].GetComponent<SpriteKeeper>();
            Weapon weapon = ColdWeapon[0].GetComponent<Weapon>();
            firstP.sprite = keeper.ColdWeaponImage;
            rect.localScale = new Vector3(weapon.WeaponImageScaleX, rect.localScale.y, rect.localScale.z);
            firstP.color = _notTransparentWhiteColor;
        }
        if (secondP != null)
        {
            RectTransform rect = secondP.GetComponent<RectTransform>();
            SpriteKeeper keeper = ColdWeapon[1].GetComponent<SpriteKeeper>();
            Weapon weapon = ColdWeapon[1].GetComponent<Weapon>();
            secondP.sprite = keeper.ColdWeaponImage;
            rect.localScale = new Vector3(weapon.WeaponImageScaleX, rect.localScale.y, rect.localScale.z);
            secondP.color = _notTransparentWhiteColor;
        }
    }

    public void SetTrhrowingPictureWeaponMenu(Image throwingP, int index) // P - это Picture; этот метод отдельно от SetPicture(), потому что количество холодных оружий на яйчеку не меняется (в плане в одной яйчейке                                                                                                                                          
    {                                                                      // не может два или больше холодных оружий, а вот метательных может)
        RectTransform rect = throwingP.GetComponent<RectTransform>();
        SpriteKeeper keeper = ThrowingWeapons[0].GetComponent<SpriteKeeper>();
        Weapon weapon = ThrowingWeapons[0].GetComponent<Weapon>();
        throwingP.sprite = keeper.ThrowingWeaponImage[index];
        throwingP.color = _notTransparentWhiteColor;
        rect.localScale = new Vector3(weapon.WeaponImageScaleX, rect.localScale.y, rect.localPosition.z);
    }

    public void SetBackgroundWeaponMenu(Image firstBg, Image secondBg, Image throwingBg, int index)
    {// этот метод подсвечивает выбранное оружие в WeaponMenuPanel (см. Canvas игры) BG - это Background; index это индекс эелемента в массиве
        if (index == 0)
        {
            firstBg.color = _backgroundWeaponColor;
            secondBg.color = _transparentWhiteColor;
        }
        else if (index == 1)
        {
            secondBg.color = _backgroundWeaponColor;
            firstBg.color = _transparentWhiteColor;
        }
        else if (index == 2)
        {
            throwingBg.color = _backgroundWeaponColor;
            firstBg.color = _transparentWhiteColor;
            secondBg.color = _transparentWhiteColor;
        }
    }

    public void ResetBackgroundWeaponMenu(Image bg)
    {
        bg.color = _transparentWhiteColor;
    }

    public void SetNameInWeaponMenu(TextMeshProUGUI weaponName, ObjectName objectName)
    {
        weaponName.text = objectName.ObjectNameS;
    }

    public void SetDamageTextInWeaponMenu(Weapon weapon)
    {
        _uiWidgetS.DamageWeapon.text = $"Урон: {weapon.MinDamage} - {weapon.MaxDamage}"; // из-за того что урон каждого оружия различается, пришлось для удобства вынести в отдельный метод
    }

    public void SetEnduranceTextInWeaponMenu(Weapon weapon)
    {
        _uiWidgetS.EnduranceWeapon.text = $"Прочность: {weapon.WEndurance}"; // из-за того что прочность оружия может изменяться, пришлось для удобства вынести в отдельный метод
    }



    public void ChangeWeaponLocationsInBackpack(int i)
    {
        GameObject item = ColdWeapon[i];
        switch (i)
        {
            case 0:
                ColdWeapon[0] = ColdWeapon[1];
                ColdWeapon[1] = item;
                break;
            case 1:
                ColdWeapon[1] = ColdWeapon[0];
                ColdWeapon[0] = item;
                break;
        }
    }
}

public class BACKpack : MonoBehaviour
{
    public UIWidget _uiWidgetSS;
    public void ResetTextParametersInWeaponMenu()
    {
        _uiWidgetSS.DamageWeapon.text = null;
        _uiWidgetSS.EnduranceWeapon.text = null;
    }
}
