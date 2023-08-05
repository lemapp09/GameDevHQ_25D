using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{ 
    private Vector3 _startingPosition;
    private Vector3 _endingPosition;
    [SerializeField] private float _xDistanceToTravel = 1f;
    private bool _movingRight = true;

    private void Start()
    {
        _startingPosition = transform.position;
        _endingPosition = transform.position + new Vector3(_xDistanceToTravel, 0, 0);
    }
    void Update()
    {
        if (_movingRight) {
            transform.position = Vector3.MoveTowards(transform.position, _endingPosition, Time.deltaTime);
            // calculate distance between two position
            if(Vector3.Distance(_endingPosition, transform.position) < 0.1f) {
                _movingRight = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startingPosition, Time.deltaTime);
            if(Vector3.Distance(_startingPosition, transform.position) < 0.1f) {
                _movingRight = true;
            }
            
        }
    }
}
