using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsMove : MonoBehaviour
{
    private enum DirectionMoveWalls
    {
        X,
        Z
    }

    [SerializeField] private DirectionMoveWalls DirectionMoveWallsS;

    [SerializeField] private float _distance;
    private float _speed = 8f;

    [SerializeField] private Vector3 _newPosition;

    [HideInInspector] public bool PermisionChangePosition = false;

    private void Start()
    {
        switch (DirectionMoveWallsS)
        {
            case DirectionMoveWalls.Z:
                _newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - _distance);
                break;
            case DirectionMoveWalls.X:
                _newPosition = new Vector3(transform.position.x - _distance, transform.position.y, transform.position.z);
                break;
        }
    }

    private void Update()
    {
        if (PermisionChangePosition == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPosition, _speed * Time.deltaTime);
        }
    }
}
