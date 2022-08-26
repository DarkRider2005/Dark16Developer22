using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraPlayer : MonoBehaviour
{
    [Header("Объекты скриптов")]
    [SerializeField] private UIWidget _uiWidgetS;
    [SerializeField] protected UIPanel _uIPanelS;
    [SerializeField] private KeyCodeS _keyCodeS;
    [SerializeField] private Player _playerS;
    [SerializeField] private GameController _gameControllerS;
    [SerializeField] private Database _dataS;
    [SerializeField] private PlayerAudio _playerAudioS;
    private ObjectName _objectNameS;
    private BackPack _backPackS;
    private Weapon _weaponS;

    private Camera _camera;

    public float SensitivityVert { get { return _sensitivityVert; } set { _sensitivityVert = value; } }
    [SerializeField] private float _sensitivityVert;
    [SerializeField] private float _minVert;
    [SerializeField] private float _maxVert;
    private float _rotationX = 0f;
    private float _interactiveDistance = 15f;

    public bool PermissionRotationCam { get { return _permissionRotationCam; } set { _permissionRotationCam = value; } }
    private bool _permissionRotationCam = true;

    public Ray Ray { get; set; }
    public RaycastHit Hit;

    [Header("Все связанное с осмотром объекта")]
    [SerializeField] private GameObject[] _mainCanvasUIObjects;
    [SerializeField] private GameObject _inspectionCanvas;
    private GameObject _inspectionObject;
    [SerializeField] private InspectionCamera _inspectionCameraS;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _backPackS = GetComponentInChildren<BackPack>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_gameControllerS.GameStateS == GameController.GameState.Game)
        {
            if (_permissionRotationCam) Rotation();
            GettingObjectRayCast();
        }
        if (_gameControllerS.GameStateS == GameController.GameState.InspectionObject)
        {
            if (Input.GetKeyDown(_keyCodeS.ExitInspection)) ExitInspectionMode();
        }
    }

    private void Rotation()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
        transform.localEulerAngles = new Vector3(_rotationX, 0f, 0f);
    }

    private void GettingObjectRayCast()
    {
        Vector3 rayCreatePosition = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray = _camera.ScreenPointToRay(rayCreatePosition);

        if (Physics.Raycast(Ray, out Hit))
        {
            GameObject _hitObject = Hit.transform.gameObject;
            _objectNameS = _hitObject.GetComponent<ObjectName>();
            _weaponS = _hitObject.GetComponent<Weapon>();
            if (_objectNameS != null && Hit.distance <= _interactiveDistance)
            {
                _uiWidgetS.InspectionObject.text = $"Осмотреть {_objectNameS.ObjectNameS}({_keyCodeS.InspectionObject})";
                if (Input.GetKey(_keyCodeS.InspectionObject))
                {
                    _permissionRotationCam = false;
                    _uiWidgetS.InspectionObject.text = null;
                    _inspectionCanvas.SetActive(true);
                    for (int i = 0; i < _mainCanvasUIObjects.Length; i++)
                    {
                        _mainCanvasUIObjects[i].SetActive(false);
                    }
                    _inspectionObject = Instantiate(_hitObject, _inspectionCameraS.transform.GetChild(0));
                    InspectionObject inspectionObject = _inspectionObject.GetComponent<InspectionObject>();
                    _inspectionCameraS.InspectionObjectS = inspectionObject;
                    _inspectionCameraS.gameObject.SetActive(true);
                    _inspectionObject.transform.localPosition = Vector3.zero + inspectionObject.PositionOffset;
                    _inspectionObject.transform.localRotation = Quaternion.Euler(Vector3.zero + inspectionObject.RotateOffset);
                    _inspectionObject.GetComponent<Rigidbody>().isKinematic = true;
                    _gameControllerS.GameStateS = GameController.GameState.InspectionObject;
                }

                if (_hitObject.CompareTag("Weapon") && _weaponS.PossibilityOfInteraction)
                {
                    if (_weaponS.WeaponTypeS == Weapon.WeaponType.Cold)
                    {
                        _weaponS.PointLights.intensity = 0;

                        _uiWidgetS.Cursors.color = _weaponS.PointLights.color;
                        _uiWidgetS.InteractiveObject.text = $"Взять {_objectNameS.ObjectNameS}({_keyCodeS.InteractionObject})";

                        if (Input.GetKey(_keyCodeS.InteractionObject) && _backPackS.ColdWeapon.Count < 2)
                        {
                            _playerAudioS.PlayAudio(_playerS.InteractiveAudioSource, _playerAudioS.InteractiveAudioClip);
                            _weaponS.PossibilityOfInteraction = false;
                            _weaponS.AvailabilityInBackpack = 1;

                            if (_backPackS.ThrowingWeapons.Count > 0) _backPackS.ThrowingWeapons[0].SetActive(false);

                            _uiWidgetS.StatusBar.text = $"Вы взяли {_objectNameS.ObjectNameS}";
                            if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = new Color(_uiWidgetS.StatusBar.color.r, _uiWidgetS.StatusBar.color.g, _uiWidgetS.StatusBar.color.b, 1f);

                            _weaponS.BindingHand();
                            _weaponS.ChangingTheLayerWeapon();

                            _backPackS.ColdWeapon.Add(_hitObject);
                            if (_backPackS.ColdWeapon.Count == 1)
                            {
                                _backPackS.SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 0);
                                _backPackS.SetPictureWeaponMenu(_uiWidgetS.PictureFirstWeapon, null);
                                _backPackS.SetNameInWeaponMenu(_uiWidgetS.FirstWeaponName, _objectNameS);
                                _backPackS.SetDamageTextInWeaponMenu(_weaponS);
                                _backPackS.SetEnduranceTextInWeaponMenu(_weaponS);
                            }
                            if (_backPackS.ColdWeapon.Count == 2)
                            {
                                _backPackS.ColdWeapon[0].SetActive(false);
                                _backPackS.SetBackgroundWeaponMenu(_uiWidgetS.BackgroundFirstWeapon, _uiWidgetS.BackgroundSecondWeapon, _uiWidgetS.BackgroundThrowingWeapon, 1);
                                _backPackS.SetPictureWeaponMenu(null, _uiWidgetS.PictureSecondWeapon);
                                _backPackS.SetNameInWeaponMenu(_uiWidgetS.SecondWeaponName, _objectNameS);
                                _backPackS.SetDamageTextInWeaponMenu(_weaponS);
                                _backPackS.SetEnduranceTextInWeaponMenu(_weaponS);
                            }
                        }
                        else if (Input.GetKey(_keyCodeS.InteractionObject) && _backPackS.ColdWeapon.Count == 2) _uiWidgetS.ForbiddenAction.text = _uiWidgetS.CannotTwoColdWeapons;
                    }
                    if (_weaponS.WeaponTypeS == Weapon.WeaponType.Propellat)
                    {
                        if (_weaponS.WEndurance > 0)
                        {
                            _weaponS.AvailabilityInBackpack = 1;
                            _weaponS.PointLights.intensity = 0;

                            _uiWidgetS.Cursors.color = _weaponS.PointLights.color;
                            _uiWidgetS.InteractiveObject.text = $"Взять {_objectNameS.ObjectNameS}({_keyCodeS.InteractionObject})";

                            if (Input.GetKey(_keyCodeS.InteractionObject) && _backPackS.ThrowingWeapons.Count < 3)
                            {
                                _weaponS.AvailabilityInBackpack = 1;
                                _weaponS.PossibilityOfInteraction = false;

                                if (_backPackS.ColdWeapon.Count == 1) _backPackS.ColdWeapon[0].SetActive(false);
                                if (_backPackS.ColdWeapon.Count == 2)
                                {
                                    _backPackS.ColdWeapon[0].SetActive(false);
                                    _backPackS.ColdWeapon[1].SetActive(false);
                                }

                                _uiWidgetS.DamageWeapon.text = $"Урон: {_weaponS.MinDamage} - {_weaponS.MaxDamage}";
                                _uiWidgetS.StatusBar.text = $"Вы взяли {_objectNameS.ObjectNameS}";
                                if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = new Color(_uiWidgetS.StatusBar.color.r, _uiWidgetS.StatusBar.color.g, _uiWidgetS.StatusBar.color.b, 1f);

                                _weaponS.BindingHand();
                                _backPackS.ThrowingWeapons.Add(_hitObject);
                                _weaponS.ChangingTheLayerWeapon();
                                _backPackS.ThrowingWeapons[0].SetActive(true);
                            }
                        }
                    }
                }
                if (_hitObject.CompareTag("Chest"))
                {
                    _uiWidgetS.Cursors.color = Color.green;
                    _uiWidgetS.InteractiveObject.text = $"Открыть {_objectNameS.ObjectNameS}({_keyCodeS.InteractionObject})";
                    if (Input.GetKey(_keyCodeS.InteractionObject))
                    {
                        _hitObject.GetComponent<Animator>().SetTrigger("Open");
                    }
                }
                if (_hitObject.CompareTag("Task"))
                {
                    _uiWidgetS.InteractiveObject.text = "Решить задачу и получить бонус";
                    if (Input.GetKey(_keyCodeS.InteractionObject))
                    {
                        Task task = _hitObject.GetComponent<Task>();
                        TaskPanel taskPanel = _uIPanelS.TaskPanel.GetComponent<TaskPanel>();
                        _uIPanelS.OpenMenu(_uIPanelS.TaskPanel);
                        _gameControllerS.GameStateS = GameController.GameState.Task;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        taskPanel.CurrentTask = task.Tasks;
                        taskPanel.CorrectAnswers = task.CorrectAnswers;
                    }                 
                }
            }

            else if (_objectNameS == null)
            {
                _uiWidgetS.Cursors.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                _uiWidgetS.InteractiveObject.text = null;
                _uiWidgetS.InspectionObject.text = null;
                _uiWidgetS.ForbiddenAction.text = null;
            }
        }
    }

    private void ExitInspectionMode()
    {
        _gameControllerS.GameStateS = GameController.GameState.Game;
        _inspectionCameraS.gameObject.SetActive(false);
        _inspectionCanvas.SetActive(false);
        for (int i = 0; i < _mainCanvasUIObjects.Length; i++)
        {
            _mainCanvasUIObjects[i].SetActive(true);
        }
        Destroy(_inspectionObject);
        _permissionRotationCam = true;
    }
}
