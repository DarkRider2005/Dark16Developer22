using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWidget : MonoBehaviour
{
    [Header("Объекты скриптов")]
    [SerializeField] private HealthPlayer _healthPlayerS;
    [SerializeField] private LegPlayer _legPlayerS;
    [SerializeField] private BackPack _backPackS;
    [SerializeField] private Timer _timerS;

    [Header("Слайдеры")]
    public Slider QuickMenuVolumeMusic;
    public Slider QuickMenuSentivityX;
    public Slider QuickMenuSentivityY;
    [SerializeField] private Slider _erSlider;
    [SerializeField] private Slider _hpSlider;

    [Header("Тексты")]
    public TextMeshProUGUI InteractiveObject;
    public TextMeshProUGUI InspectionObject;
    public TextMeshProUGUI ForbiddenAction;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI ConfirmationObjectEjection; // используется в скрипте BackPack
    public TextMeshProUGUI StatusBar; // используется в скрипте BackPack

    public TextMeshProUGUI FirstWeaponName;             //@ <- Эта метка значит, что это для холодного оружия
    public TextMeshProUGUI SecondWeaponName;            //@
    public TextMeshProUGUI EnduranceWeapon;             //@!
    public TextMeshProUGUI DamageWeapon;                //@!
    public TextMeshProUGUI InformationAboutImpacts;     //@!
    public TextMeshProUGUI NameOfTheThrowingWeapon;         //! <- Эта метка значит, что это для метательного оружия

    [Header("Картинки")]
    public Image Cursors;
    public Image PictureFirstWeapon;            //@
    public Image BackgroundFirstWeapon;         //@
    public Image PictureSecondWeapon;           //@
    public Image BackgroundSecondWeapon;        //@
    public Image PictureThrowingWeapon;             //!
    public Image BackgroundThrowingWeapon;          //!

    [Header("Менюшки для изменения разрешения экрана и цвета интерфейса")]
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown ThemeDropdown;

    [HideInInspector] public string CannotTwoColdWeapons = "Нельзя носить более двух холодных оружий!";
    [HideInInspector] public string CannotThreeDagger = "Нельзя носить более трех метательных оружий!";
    [HideInInspector] public string NotEnoughStaminaToAttack = "Не хватает выносливости для атаки!";
    [HideInInspector] public string YouCanOnlyBreakWallsWithWeapons = "Ломать стены можно только оружием!";

    private Color _normalColorEnduranceText;
    private Color _backgroundWeaponColor;

    private void Start()
    {
        ResolutionDropdown.value = PlayerPrefs.GetInt("ValueResolutionDropdown");
        ThemeDropdown.value = PlayerPrefs.GetInt("DropdownThemesValue");
        _normalColorEnduranceText = new Color(EnduranceWeapon.color.r, EnduranceWeapon.color.g, EnduranceWeapon.color.b, EnduranceWeapon.color.a);
        _backgroundWeaponColor = new Color(1f, 1f, 1f, 0.2f);
    }

    private void Update()
    {
        Sliders();
        Texts();
        if (StatusBar.text != null || InformationAboutImpacts.text != null || ForbiddenAction.text != null)
        {
            ChangeAlphaChannelInfoText();
        }
    }

    private void Sliders()
    {
        _erSlider.maxValue = _healthPlayerS.FullER;
        _erSlider.value = _healthPlayerS.CurrentER;

        _hpSlider.maxValue = _healthPlayerS.FullHP;
        _hpSlider.value = _healthPlayerS.CurrentHP;
    }

    private void Texts()
    {
        InteractiveObject.color = Cursors.color;

        Timer.text = string.Format("{0:00} : {1:00}", _timerS.Minutes, _timerS.Seconds);
        if (_timerS.TimeLeft < _timerS.HalfTime)
        {
            Timer.color = Color.yellow;
        }
        if (_timerS.TimeLeft < _timerS.QuarterTime) Timer.color = Color.red;
    }

    private void ChangeAlphaChannelInfoText()
    {
        if (StatusBar.text != null) StatusBar.color = new Color(StatusBar.color.r, StatusBar.color.g, StatusBar.color.b, StatusBar.color.a - 0.2f * Time.deltaTime);
        else return;

        if (InformationAboutImpacts.text != null) InformationAboutImpacts.color = new Color(InformationAboutImpacts.color.r, InformationAboutImpacts.color.g, InformationAboutImpacts.color.b,
             InformationAboutImpacts.color.a - 0.2f * Time.deltaTime);
        else return;

        if (ForbiddenAction.text != null) ForbiddenAction.color = new Color(ForbiddenAction.color.r, ForbiddenAction.color.g,
             ForbiddenAction.color.b, ForbiddenAction.color.a - 0.2f * Time.deltaTime);
        else return;
    }
    public IEnumerator ChangeColorEnduranceWeaponTextWhenAttack() // для болле глубокого понимания, смотри Weapon - OnCollisionEnter()
    {
        EnduranceWeapon.fontSize += 10;
        EnduranceWeapon.color = Color.red;
        yield return new WaitForSeconds(1.5f);
        EnduranceWeapon.color = _normalColorEnduranceText;
        EnduranceWeapon.fontSize -= 10;
    }

    public IEnumerator YouCanOnlyBreakWallsWithWeapon()
    {
        yield return new WaitForSeconds(2f);
        ForbiddenAction.text = null;
        _legPlayerS.ForbiddenActionText = false;
    }
}
