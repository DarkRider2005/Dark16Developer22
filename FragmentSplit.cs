using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSplit : MonoBehaviour
{
    private Weapon _weaponS;

    private void Start()
    {
        _weaponS = GetComponentInParent<Weapon>();
        _weaponS.ComponentsOfTheObjects.Add(gameObject);
    }

    private void Update()
    {
        if (_weaponS.WEndurance <= 0)
        {
            _weaponS.PointLights.intensity = 0f;
            DestroyGameObject(_weaponS.gameObject);
        }
    }

    private void DestroyGameObject(GameObject destroyObject)
    {
        if (gameObject.GetComponent<Rigidbody>() == null) gameObject.AddComponent<Rigidbody>();
        if (gameObject.GetComponent<BoxCollider>() == null) gameObject.AddComponent<BoxCollider>();
        Destroy(destroyObject, 5f);
    }
}
