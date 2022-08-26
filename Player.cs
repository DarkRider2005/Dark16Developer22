using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private KeyCodeS _keyCodeS;
    [SerializeField] private UIWidget _uiWidgetS;
    [SerializeField] private GameController _gameControllerS;
    [SerializeField] private PlayerAudio _playerAudioS;
    private BackPack _backPackS;
    private HealthPlayer _healthPlayerS;
    private AttackPlayer _attackPlayerS;

    public AudioSource InteractiveAudioSource;
    public AudioSource LifePlayerAudioSource;
    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioSource _runAudioSource;
    private Animator _animator;
    private Rigidbody _rigidbody;

    public bool PermissionRotation { get { return _permissionRotationAxesY; } set { _permissionRotationAxesY = value; } }
    public bool PermissionDiscardObject { get { return _permissionDiscardObject; } set { _permissionDiscardObject = value; } }
    public bool Run { get { return _run; } set { _run = value; } }
    public bool RecoveryER { get { return _recoveryER; } }
    private bool _recoveryER = true;
    private bool _permissionRun = true;
    private bool _useMedicalKit = false;
    private bool _permissionJump = true;
    private bool _permissionRotationAxesY = true;
    private bool _permissionDiscardObject = false; //! что эта переменная здесь делает?
    private bool _run;

    public float SensitivityHor { get { return _sensitivityHor; } set { _sensitivityHor = value; } }
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sensitivityHor;

    private float _defaultWalkSpeed = 15f;

    private void Start()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("SavePositionXPlayer"), PlayerPrefs.GetFloat("SavePositionYPlayer"), PlayerPrefs.GetFloat("SavePositionZPlayer"));
        _healthPlayerS = GetComponent<HealthPlayer>();
        _attackPlayerS = GetComponent<AttackPlayer>();
        _backPackS = GetComponentInChildren<BackPack>();

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_gameControllerS.GameStateS == GameController.GameState.Game)
        {
            Move();

            if (_permissionRotationAxesY) Rotation();
            if (_healthPlayerS.CurrentER == 0)
            {
                _permissionRun = false;
                _walkSpeed = _defaultWalkSpeed;
            }
            else _permissionRun = true;
        }
    }

    private void Move()
    {
        float directionForward = 0f;
        if (Input.GetKey(_keyCodeS.Forward) && _permissionJump) // если на игрок на земле
        {
            directionForward = Direction(1f);
            swe();
        }
        if (Input.GetKey(_keyCodeS.Back) && _permissionJump) // если на игрок на земле
        {
            directionForward = Direction(-1f);
            swe();
        }
        float directionRight = 0f;
        if (Input.GetKey(_keyCodeS.Left) && _permissionJump) // если на игрок на земле
        {
            directionRight = Direction(-1f);
            swe();
        }
        if (Input.GetKey(_keyCodeS.Right) && _permissionJump) // если на игрок на земле
        {
            directionRight = Direction(1f);
            swe();
        }
        if (directionForward == 0 && directionRight == 0)
        {
            _animator.SetBool("IsWalk", false);
            _playerAudioS.StopAudio(_walkAudioSource);
            _playerAudioS.StopAudio(_runAudioSource);
        }
        if (Input.GetKey(_keyCodeS.Jump) && _permissionJump == true && !_run)
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _permissionJump = false;
        }
        if (((Input.GetKey(_keyCodeS.Forward) || Input.GetKey(_keyCodeS.Back) || Input.GetKey(_keyCodeS.Right) || Input.GetKey(_keyCodeS.Left)) && Input.GetKey(_keyCodeS.Run)) && _permissionRun)
        {
            _run = true;
            _playerAudioS.StopAudio(_walkAudioSource);
            _playerAudioS.PlayAudio(_runAudioSource, _playerAudioS.RunAudioClip);
            _animator.SetBool("IsRun", true);
            _recoveryER = false;
            _healthPlayerS.ChangeHPInRun();
            _walkSpeed = _runSpeed;
        }
        else if ((!Input.GetKey(_keyCodeS.Forward) || !Input.GetKey(_keyCodeS.Back) || !Input.GetKey(_keyCodeS.Right) || !Input.GetKey(_keyCodeS.Left)) || !Input.GetKey(_keyCodeS.Run))
        {
            _run = false;
            _animator.SetBool("IsRun", false);
            _recoveryER = true;
            _walkSpeed = _defaultWalkSpeed;
            _playerAudioS.StopAudio(_runAudioSource);
        }
        Vector3 _movement = new Vector3(directionRight * _walkSpeed, _rigidbody.velocity.y, directionForward * _walkSpeed);
        _movement = transform.TransformDirection(_movement);
        _movement = Vector3.ClampMagnitude(_movement, _walkSpeed);
        _rigidbody.velocity = _movement;
    }

    private void swe()
    {
        _animator.SetBool("IsWalk", true);
        _playerAudioS.PlayAudio(_walkAudioSource, _playerAudioS.WalkAudioClip);
    }

    private float Direction(float value)
    {
        return value;
    }

    private void Rotation()
    {
        float _delta = Input.GetAxis("Mouse X") * _sensitivityHor;
        float _rotationY = transform.localEulerAngles.y + _delta;
        transform.localEulerAngles = new Vector3(0, _rotationY, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SavePositionPlayer"))
        {
            _uiWidgetS.StatusBar.text = "Сохранено";
            if (_uiWidgetS.StatusBar.color.a < 1f) _uiWidgetS.StatusBar.color = new Color(_uiWidgetS.StatusBar.color.r, _uiWidgetS.StatusBar.color.g, _uiWidgetS.StatusBar.color.b, 1f);
            PlayerPrefs.SetFloat("SavePositionXPlayer", other.transform.position.x);
            PlayerPrefs.SetFloat("SavePositionYPlayer", other.transform.position.y);
            PlayerPrefs.SetFloat("SavePositionZPlayer", other.transform.position.z);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") && !_permissionJump)
        {
            _permissionJump = true;
            _playerAudioS.PlayAudio(InteractiveAudioSource, _playerAudioS.JumpAudioClip); // через InteractiveAudioSource, потому что у него "Loop" выключен
        }
    }
}