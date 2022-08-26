using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePointControl : MonoBehaviour
{
    /// <summary>
    /// верхн€€(_upPosition) и нижн€€(_downPosition) точка, дл€ перемещени€ по этим точкам объекта, к которому присоединен этот скрипт
    /// </summary>
    private Vector3 _upPosition, _downPosition;

    [SerializeField] private Light[] _mainPointLights;

    [SerializeField] private float _moveSpeed, _rotateSpeed;

    private bool _move = true;

    private void Start()
    {
        _upPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        _downPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        transform.position = _downPosition;
    }

    private void FixedUpdate()
    {
        IntensityPointLights(2f, 2f);
        if (transform.position == _upPosition) _move = false;
        else if (transform.position == _downPosition) _move = true;

        if (_move) MoveNewPosition(_upPosition);
        else MoveNewPosition(_downPosition);
    }

    private void MoveNewPosition(Vector3 newPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, _moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * _rotateSpeed * Time.fixedDeltaTime);
    }

    private void IntensityPointLights(float speedUpIntensityLight, float speedDownIntensityLight)
    {
        if (_move)
        {
            foreach (Light light in _mainPointLights)
            {
                light.intensity = Mathf.MoveTowards(light.intensity, 2f, speedUpIntensityLight * Time.fixedDeltaTime);
            }
        }
        else
        {
            foreach (Light light in _mainPointLights)
            {              
                light.intensity = Mathf.MoveTowards(light.intensity, 0f, speedDownIntensityLight * Time.fixedDeltaTime); ;
            }
        }
    }
}
