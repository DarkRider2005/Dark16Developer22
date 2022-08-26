using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentObjectSplit : MonoBehaviour
{
    private HpObject _hpObject;

    private void Start()
    {
        _hpObject = GetComponentInParent<HpObject>();
        _hpObject.ComponentsOfTheObject.Add(gameObject);
    }

    private void Update()
    {
        if (_hpObject.HP <= 0) DestroyGameObject();
    }

    private void DestroyGameObject()
    {       
        if (gameObject.GetComponent<Rigidbody>() == null) gameObject.AddComponent<Rigidbody>();
        if (gameObject.GetComponent<BoxCollider>() == null) gameObject.AddComponent<BoxCollider>();
        Destroy(gameObject, Random.Range(6f, 10f)); 
    }

    private void OnDestroy()
    {
        _hpObject.ComponentsOfTheObject.Remove(gameObject);
    }
}
