using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LiftingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 12f;
    [SerializeField]
    private Transform _startingPosition;
    [SerializeField]
    private Transform _endingPosition;
    private Vector3 _nextPosition;
    private bool _goinUp = true;
    private bool _isMoving = false;


    void FixedUpdate() {
        if (_isMoving) {
            MovePlatform();
        }
    }

    private void Start() {
        _nextPosition = _endingPosition.position;
    }

    private void MovePlatform() {
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, step);

        if (Vector3.Distance(transform.position, _nextPosition) < 0.01f)
        {
            _isMoving = false;
            if (_goinUp) {
                _nextPosition = _endingPosition.position;
            }  else   {
                _nextPosition = _startingPosition.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
            StartCoroutine(StartMoving());
        }
    }

    private IEnumerator StartMoving() {
        yield return new WaitForSeconds(0.5f);
        _isMoving = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            other.transform.parent = null;
        }
    }
}