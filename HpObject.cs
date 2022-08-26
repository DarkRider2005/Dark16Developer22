using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpObject : MonoBehaviour
{
    public enum TypeObject
    {
        Walls,
        Environment
    }

    public TypeObject TypeObjectS;

    [SerializeField] private Player _playerS;

    [HideInInspector] public BoxCollider BoxCollider;
    [HideInInspector] public CapsuleCollider CapsuleCollider;

    [SerializeField] private Transform _smokeTransform;

    public List<GameObject> ComponentsOfTheObject = new List<GameObject>();

    public TextMeshPro EnduranceObject;

    [SerializeField] private GameObject _smokeDestroy;
    private GameObject _smoke;

    public float HP = 1;
    public float ChangeEnduranceWeapon;

    public int MinChangeEnduranceWeapon;
    public int MaxChangeEnduranceWeapon;
    [SerializeField] private int _quantityDestroyObject; // для понимания смотри этот скрипт - AddingPhysicsObjects()

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        if (BoxCollider == null || CapsuleCollider == null) return;
    }

    private void Update()
    {
        EnduranceObject.text = $"Прочность: {HP}";
        EnduranceObject.transform.localEulerAngles = new Vector3(_playerS.transform.localEulerAngles.x, _playerS.transform.localEulerAngles.y * -3f, _playerS.transform.localEulerAngles.z);
        if (HP <= 0)
        {            
            if(_smoke == null) _smoke = Instantiate(_smokeDestroy, _smokeTransform.position, _smokeDestroy.transform.rotation);
            EnduranceObject.text = "Прочность: 0";
        }
        if (ComponentsOfTheObject.Count == 0 && _smoke.GetComponent<ParticleSystem>().isPlaying == false) Destroy(gameObject);
    }

    public void ChangeHP(float damage)
    {
        HP -= damage;
    }

    public void AddingPhysicsObjects() // метод для добавления физических свойств рандомным состaвляющим объекта 
    {
        for (int i = 0; i < _quantityDestroyObject; i++)
        {
            int index = Random.Range(0, ComponentsOfTheObject.Count - 1);
            if (ComponentsOfTheObject[index].GetComponent<Rigidbody>() == null) ComponentsOfTheObject[index].AddComponent<Rigidbody>().mass = Random.Range(10f, 20f);
            if (ComponentsOfTheObject[index].GetComponent<BoxCollider>() == null) ComponentsOfTheObject[index].AddComponent<BoxCollider>();
        }
    }
}
