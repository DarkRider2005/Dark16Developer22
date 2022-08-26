using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualBackPack : MonoBehaviour
{
    [SerializeField] private BackPack _backPackS;

    private string _informationChangeWeaponLocations = "Чтобы поменять местами оружие 1 и 2, нажмите на картинку с оружием";

    [SerializeField] private TextMeshProUGUI _informationChangeWeaponLocation;
    [Header("Первое оружие")]
    [SerializeField] private Image _imageWeaponOne;
    [SerializeField] private TextMeshProUGUI _nameWeaponOne;
    [SerializeField] private TextMeshProUGUI _enduranceWeaponOne;
    [SerializeField] private TextMeshProUGUI _damageWeaponOne;
    [SerializeField] private TextMeshProUGUI _typeWeaponOne;
    [SerializeField] private TextMeshProUGUI _qualityWeaponOne;
    [Header("Второе оружие")]
    [SerializeField] private Image _imageWeaponTwo;
    [SerializeField] private TextMeshProUGUI _nameWeaponTwo;
    [SerializeField] private TextMeshProUGUI _enduranceWeaponTwo;
    [SerializeField] private TextMeshProUGUI _damageWeaponTwo;
    [SerializeField] private TextMeshProUGUI _typeWeaponTwo;
    [SerializeField] private TextMeshProUGUI _qualityWeaponTwo;
    [Header("Метательное оружие")]
    [SerializeField] private Image _imageThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _nameThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _enduranceThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _damageThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _quantityThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _typeThrowingWeapon;
    [SerializeField] private TextMeshProUGUI _qualityThrowingWeapon;
    [Header("Расходники")]
    [SerializeField] private TextMeshProUGUI _quantityMedicalKit;

    private void Start()
    {

    }

    private void Update()
    {
        if (_backPackS.ColdWeapon.Count == 0)
        {
            ResetParameters(_imageWeaponOne, _nameWeaponOne, _enduranceWeaponOne, _damageWeaponOne, _typeWeaponOne, _qualityWeaponOne, null);
            ResetParameters(_imageWeaponTwo, _nameWeaponTwo, _enduranceWeaponTwo, _damageWeaponTwo, _typeWeaponTwo, _qualityWeaponTwo, null);
        }
        if (_backPackS.ColdWeapon.Count == 1)
        {
            ResetParameters(_imageWeaponTwo, _nameWeaponTwo, _enduranceWeaponTwo, _damageWeaponTwo, _typeWeaponTwo, _qualityWeaponTwo, null);
            SetWeaponParameters(_backPackS.ColdWeapon[0].GetComponent<Weapon>(), _backPackS.ColdWeapon[0].GetComponent<SpriteKeeper>(), _backPackS.ColdWeapon[0].GetComponent<ObjectName>(), _imageWeaponOne,
                 _nameWeaponOne, _enduranceWeaponOne, _damageWeaponOne, _typeWeaponOne, _qualityWeaponOne);
        }
        if (_backPackS.ColdWeapon.Count == 2)
        {
            SetWeaponParameters(_backPackS.ColdWeapon[0].GetComponent<Weapon>(), _backPackS.ColdWeapon[0].GetComponent<SpriteKeeper>(), _backPackS.ColdWeapon[0].GetComponent<ObjectName>(), _imageWeaponOne,
                 _nameWeaponOne, _enduranceWeaponOne, _damageWeaponOne, _typeWeaponOne, _qualityWeaponOne);
            SetWeaponParameters(_backPackS.ColdWeapon[1].GetComponent<Weapon>(), _backPackS.ColdWeapon[1].GetComponent<SpriteKeeper>(), _backPackS.ColdWeapon[1].GetComponent<ObjectName>(), _imageWeaponTwo,
                _nameWeaponTwo, _enduranceWeaponTwo, _damageWeaponTwo, _typeWeaponTwo, _qualityWeaponTwo);
            _informationChangeWeaponLocation.text = _informationChangeWeaponLocations;
        }

        if (_backPackS.ThrowingWeapons.Count == 0)
        {
            ResetParameters(_imageThrowingWeapon, _nameThrowingWeapon, _damageThrowingWeapon, _enduranceThrowingWeapon, _typeThrowingWeapon, _qualityThrowingWeapon, _quantityThrowingWeapon);
        }
        if (_backPackS.ThrowingWeapons.Count == 1)
        {
            SetThrowingWeaponParameters(_backPackS.ThrowingWeapons[0].GetComponent<Weapon>(), _backPackS.ThrowingWeapons[0].GetComponent<SpriteKeeper>(), _backPackS.ThrowingWeapons[0].GetComponent<ObjectName>(),
              0, _imageThrowingWeapon, _nameThrowingWeapon, _enduranceThrowingWeapon, _damageThrowingWeapon, _quantityThrowingWeapon, _typeThrowingWeapon, _qualityThrowingWeapon);
        }
        if (_backPackS.ThrowingWeapons.Count == 2)
        {
            SetThrowingWeaponParameters(_backPackS.ThrowingWeapons[0].GetComponent<Weapon>(), _backPackS.ThrowingWeapons[0].GetComponent<SpriteKeeper>(), _backPackS.ThrowingWeapons[0].GetComponent<ObjectName>(),
              1, _imageThrowingWeapon, _nameThrowingWeapon, _enduranceThrowingWeapon, _damageThrowingWeapon, _quantityThrowingWeapon, _typeThrowingWeapon, _qualityThrowingWeapon);
        }
        if (_backPackS.ThrowingWeapons.Count == 3)
        {
            SetThrowingWeaponParameters(_backPackS.ThrowingWeapons[0].GetComponent<Weapon>(), _backPackS.ThrowingWeapons[0].GetComponent<SpriteKeeper>(), _backPackS.ThrowingWeapons[0].GetComponent<ObjectName>(),
              2, _imageThrowingWeapon, _nameThrowingWeapon, _enduranceThrowingWeapon, _damageThrowingWeapon, _quantityThrowingWeapon, _typeThrowingWeapon, _qualityThrowingWeapon);
        }
    }

    private void SetWeaponParameters(Weapon weapon, SpriteKeeper keeper, ObjectName objectName, Image image, TextMeshProUGUI name, TextMeshProUGUI endurance, TextMeshProUGUI damage, TextMeshProUGUI type, TextMeshProUGUI quality)
    {
        image.sprite = keeper.ColdWeaponImage;
        image.GetComponent<RectTransform>().localScale = new Vector3(weapon.WeaponImageScaleXVisualBackPack, weapon.WeaponImageScaleYVisualBackPack, 0f);
        image.color = Color.white;
        name.text = $"Название: {objectName.ObjectNameS}";
        endurance.text = $"Прочность: {weapon.WEndurance}";
        damage.text = $"Урон: {weapon.MinDamage} - {weapon.MaxDamage}";
        type.text = $"Тип оружия: {weapon.WeaponTypeRus}";
        quality.text = $"Качество оружия: {weapon.WeaponQualityRus}";
    }

    private void SetThrowingWeaponParameters(Weapon weapon, SpriteKeeper keeper, ObjectName objectName, int i, Image image, TextMeshProUGUI name, TextMeshProUGUI endurance, TextMeshProUGUI damage, TextMeshProUGUI quantity, TextMeshProUGUI type, TextMeshProUGUI quality)
    {
        image.sprite = keeper.ThrowingWeaponImage[i];
        image.GetComponent<RectTransform>().localScale = new Vector3(weapon.WeaponImageScaleXVisualBackPack, weapon.WeaponImageScaleYVisualBackPack, 0f);
        image.color = Color.white;
        name.text = $"Название: {objectName.ObjectNameS}";
        endurance.text = $"Прочность: {weapon.WEndurance}";
        damage.text = $"Урон: {weapon.MinDamage} - {weapon.MaxDamage}";
        quantity.text = $"Количество: {i + 1}";
        type.text = $"Тип оружия: {weapon.WeaponTypeRus}";
        quality.text = $"Качество оружия: {weapon.WeaponQualityRus}";
    }

    private void SetConsumablesParameters(int i, TextMeshProUGUI name, string names)
    {
        name.text = names + i.ToString();
    }

    private void ResetParameters(Image image, TextMeshProUGUI name, TextMeshProUGUI endurance, TextMeshProUGUI damage, TextMeshProUGUI type, TextMeshProUGUI quality, TextMeshProUGUI quantity)
    {
        image.sprite = null;
        image.color = new Color(1f, 1f, 1f, 0f);
        name.text = null;
        endurance.text = null;
        damage.text = null;
        type.text = null;
        quality.text = null;
        if (quantity != null) quantity.text = null;
    }

    public void ChangeWeaponLocationFirst()
    {
        if (_backPackS.ColdWeapon.Count == 2) _backPackS.ChangeWeaponLocationsInBackpack(1);
    }

    public void ChangeWeaponLocationSecond()
    {
        if (_backPackS.ColdWeapon.Count == 2) _backPackS.ChangeWeaponLocationsInBackpack(0);
    }
}
