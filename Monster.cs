using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{ // ваще нахрен все  переписать в жопу
    public enum State
    {
        Walking,
        Escape,
        AttackPlayer,
        AttackEnvironment,
        Death
    }

    public State StateS = State.Walking;

    [SerializeField] private GameController _gameControllerS;

    private Animator _animator;
    private Rigidbody _rigidbody;

    [SerializeField] private Transform _rayCreate;
    [SerializeField] private Transform[] _trajectoryOfMovement;

    [SerializeField] private GameObject _player;
    [HideInInspector] public GameObject HitObject;

    [SerializeField] private Material _eyeMaterial;

    [Header("Свойства монстра")]
    [SerializeField] private string AttackAnimName;
    [SerializeField] private string RunAnimName;
    [SerializeField] private string DeathAnimName;

    public int MinChangeEnduranceWeaponM;   // приставка М в конце для понимания в других скриптах что это именно переменная которая относится к монстру, потому что подобных переменных не мало
    public int MaxChangeEnduranceWeaponM;
    public int MinDamageM;
    public int MaxDamageM;
    public int MovementDirectionIndex { get; set; } = 0;

    public float ChangeEnduranceWeaponM;
    public float DamageM;
    public float HP = 150f;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _escapeSpeed;
    [SerializeField] private float _interactiveEnvironmentDistance; // для определения объекта окружения перед монстром для запуска последующих действий
    [SerializeField] private float _attackDistance; // максимальная дистанция между монстром и игроком для атаки
    [SerializeField] private float _distanceTurnFromTheWall; // максимальная дистанция между монстром и стеной для поворота монстра от нее
    [SerializeField] private float _playerDetectionDistance; // максимальная дистанция обнаружения игрока

    private bool _isMoveTrajectory = true;
    private bool _takingDamage = false;
    private bool _isAttack = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        SetColorEye();
    }

    private void Update()
    {
        if (_gameControllerS.GameStateS == GameController.GameState.Game || _gameControllerS.GameStateS == GameController.GameState.ViewingTheMap)
        {
            GettingObjectRaycast();
            if (_isMoveTrajectory && StateS != State.AttackPlayer && StateS != State.AttackEnvironment && StateS != State.Escape
                && StateS != State.Death) TrajectoryMovement();
            else if (!_isMoveTrajectory && StateS != State.AttackPlayer && StateS != State.AttackEnvironment && StateS != State.Escape
                && StateS != State.Death) ReverseMovementAlongTheTrajectory();
            if (StateS == State.Escape)
            {
                EscapeMove();
            }
            if (HP <= 0) Death();
            if (!_rigidbody.freezeRotation) StartCoroutine(FreezeRotation());
        }
    }

    private void GettingObjectRaycast()
    {
        Ray ray = new Ray(_rayCreate.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 4f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            HpObject hpObject = hitObject.GetComponent<HpObject>();

            if (hitObject.CompareTag("DestroyEnvironment") && hit.distance <= _interactiveEnvironmentDistance)
            {
                AttackEnvironment();
            }
            if (hitObject.CompareTag("PlayerS") && hit.distance <= _playerDetectionDistance)
            {
                AttackPlayer();
            }
            if (hitObject.CompareTag("Walls") && (hit.distance <= _distanceTurnFromTheWall) && StateS == State.Escape)
            {
                TurnFromTheWall();
            }
        }
    }

    private void TrajectoryMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _trajectoryOfMovement[MovementDirectionIndex].position, _walkingSpeed * Time.deltaTime);
        if (transform.position == _trajectoryOfMovement[MovementDirectionIndex].position && MovementDirectionIndex < _trajectoryOfMovement.Length - 1)
        {
            MovementDirectionIndex++;
            transform.LookAt(_trajectoryOfMovement[MovementDirectionIndex]);
        }
        if (transform.position == _trajectoryOfMovement[MovementDirectionIndex].position && MovementDirectionIndex == _trajectoryOfMovement.Length - 1) _isMoveTrajectory = false;
    }

    private void ReverseMovementAlongTheTrajectory()
    {
        transform.position = Vector3.MoveTowards(transform.position, _trajectoryOfMovement[MovementDirectionIndex].position, _walkingSpeed * Time.deltaTime);
        if (transform.position == _trajectoryOfMovement[MovementDirectionIndex].position && MovementDirectionIndex > 0)
        {
            MovementDirectionIndex--;
            transform.LookAt(_trajectoryOfMovement[MovementDirectionIndex]);
        }
        if (transform.position == _trajectoryOfMovement[MovementDirectionIndex].position && MovementDirectionIndex == 0) _isMoveTrajectory = true;
    }

    private void TurnFromTheWall()
    {
        float angles = Random.Range(-120, 120);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + angles, transform.localEulerAngles.z);
    }

    private void EscapeMove()
    {
        transform.LookAt(null);
        Vector3 move = new Vector3(0, _rigidbody.velocity.y, _escapeSpeed);
        move = transform.TransformVector(move);
        _rigidbody.velocity = move;
    }

    private void AttackPlayer()
    {
        StateS = State.AttackPlayer;
        SetColorEye();
        if (Vector3.Distance(transform.position, _player.transform.position) > _attackDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _attackSpeed * Time.deltaTime);
            transform.LookAt(_player.transform);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _attackDistance)
        {
            _isAttack = true;
            transform.LookAt(_player.transform);
            _animator.SetTrigger(AttackAnimName);
            FurtherActions();
        }
    }

    private void AttackEnvironment()
    {
        StateS = State.AttackEnvironment;
        _animator.SetTrigger(AttackAnimName);
        StartCoroutine(AbilityToMoveAround());
    }

    private void Death()
    {
        StateS = State.Death;
        SetColorEye();
        _animator.SetTrigger(DeathAnimName);
        //  _rigidbody.isKinematic = true;
        Destroy(gameObject, 20f);
    }

    private void SetColorEye()
    {
        if (_eyeMaterial != null)
        {
            switch (StateS)
            {
                case State.Walking:
                    _eyeMaterial.EnableKeyword("_EMISSION");
                    Color color = new Color(0f, 1f, 1f);
                    _eyeMaterial.SetColor("_EmissionColor", color);
                    break;
                case State.AttackPlayer:
                    _eyeMaterial.EnableKeyword("_EMISSION");
                    _eyeMaterial.SetColor("_EmissionColor", Color.red);
                    break;
                case State.Death:
                    _eyeMaterial.DisableKeyword("_EMISSION");
                    _eyeMaterial.color = Color.black;
                    break;
                case State.Escape:
                    _eyeMaterial.EnableKeyword("_EMISSION");
                    _eyeMaterial.SetColor("_EmissionColor", Color.blue);
                    break;
            }
        }
    }

    private void FurtherActions()
    {
        if (_isAttack == true && _takingDamage == true)
        {
            _animator.SetTrigger(RunAnimName);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.localEulerAngles.y - 180f, transform.rotation.z);
            _isAttack = false;
            _takingDamage = false;
            StateS = State.Escape;
            SetColorEye();
        }

        if (_isAttack == true && _takingDamage == false)
        {
            _isAttack = false;
            StateS = State.AttackPlayer;
            SetColorEye();
        }
    }

    public void ChangeHPMonster(float weaponDamage)
    {
        HP -= weaponDamage;
        _takingDamage = true;
    }

    private IEnumerator AbilityToMoveAround()
    {
        yield return new WaitForSeconds(2.5f);
        StateS = State.Walking;
        SetColorEye();
    }

    private IEnumerator FreezeRotation()
    {
        yield return new WaitForSeconds(1);
        _rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
