using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InspectionCamera : MonoBehaviour
{
    public InspectionObject InspectionObjectS { get { return _inspectionObjectS; } set { _inspectionObjectS = value; } }
    [SerializeField] private KeyCodeS _keyCodeS;
    private InspectionObject _inspectionObjectS;

    public Transform ObjectRotate { get { return _objectRotate; } set { _objectRotate = value; } }
    [SerializeField] private Transform _objectRotate;

    [SerializeField] private TextMeshProUGUI _exitInspectionObject;
    [SerializeField] private TextMeshProUGUI _infoInspectionObject;
    [SerializeField] private TextMeshProUGUI _authorInspectionObject;

    [SerializeField] private GameObject _buttonArtStationAuthor;
    [SerializeField] private GameObject _infoRotationInspectionObject;
    [SerializeField] private GameObject _infoZoomInspectionObject;

    private Vector3 _targetPosition;

    private string _linkAuthorModel;
    private string _nameAuthorModel;

    [SerializeField] private float _zoomSpeed = 15f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _changePositionSpeed = 10f;

    private void Update()
    {
        ZoomObject();
        RotateObject();
    }

    private void ZoomObject()
    {
        _targetPosition = new Vector3(_targetPosition.x, _targetPosition.y, _targetPosition.z - Input.GetAxis("Mouse ScrollWheel"));
        _objectRotate.localPosition = Vector3.Lerp(_objectRotate.localPosition, _targetPosition, Time.deltaTime * _zoomSpeed);
    }

    private void RotateObject()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _objectRotate.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Time.deltaTime * _rotationSpeed, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * _rotationSpeed, Space.World);
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Light light = _objectRotate.GetComponentInChildren<Light>();
        if (light != null) light.enabled = false;
        else return;

        _linkAuthorModel = _inspectionObjectS.GetComponentInChildren<InfoAuthorModel>().AuthorLinkArtStation;
        _nameAuthorModel = _inspectionObjectS.GetComponentInChildren<InfoAuthorModel>().AuthorName;

        _buttonArtStationAuthor.SetActive(true);
        _infoRotationInspectionObject.SetActive(true);
        _infoZoomInspectionObject.SetActive(true);
        _exitInspectionObject.gameObject.SetActive(true);
        _exitInspectionObject.text = $"Чтобы выйти из режима осмотра, нажмите ({_keyCodeS.ExitInspection})";
        _infoInspectionObject.gameObject.SetActive(true);
        _infoInspectionObject.text = $"Название: {_inspectionObjectS.gameObject.GetComponent<ObjectName>().ObjectNameS}";
        _authorInspectionObject.gameObject.SetActive(true);
        _authorInspectionObject.text = $"Автор: {_nameAuthorModel}";

        _targetPosition.z = Mathf.Clamp(_targetPosition.z, InspectionObjectS.MinMaxZoom.x, InspectionObjectS.MinMaxZoom.y);
        _targetPosition.z = _inspectionObjectS.DefaultObjectZoom;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _linkAuthorModel = "";
        _nameAuthorModel = "";

        _buttonArtStationAuthor.SetActive(false);
        _infoRotationInspectionObject.SetActive(false);
        _infoZoomInspectionObject.SetActive(false);
        _exitInspectionObject.gameObject.SetActive(false);
        _exitInspectionObject.text = null;
        _infoInspectionObject.gameObject.SetActive(false);
        _infoInspectionObject.text = null;
        _authorInspectionObject.gameObject.SetActive(false);
        _authorInspectionObject.text = null;

        _objectRotate.localPosition = Vector3.zero;
        _objectRotate.localRotation = Quaternion.Euler(0f, -90f, 0f);
    }

    public void OpenArtstationAuthorInspectionObject()
    {
        Application.OpenURL(_linkAuthorModel);
    }
}
