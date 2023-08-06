using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameDevHQ_25dCert
{
    public class LiftingPlatform_cert : MonoBehaviour
    {
        [SerializeField] private Transform _targetA, _targetB;
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private bool _goingDown,  _playerInElevator,  _isMoving;

        private void Update() {
            if (_playerInElevator) {
                if (Keyboard.current.eKey.wasPressedThisFrame) {
                    CallElevator();
                }
            }
        }

        void FixedUpdate() {
            if (_isMoving) {
                if (_goingDown) {
                    transform.position =
                        Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _targetB.position) < 0.1f) {
                        _isMoving = false;
                    }
                }  else {
                    transform.position =
                        Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _targetA.position) < 0.1f) {
                        _isMoving = false;
                    }
                }
            }
        }

        public void CallElevator() {
            // know the current location of elevator
            _goingDown = !_goingDown;
            _isMoving = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                other.transform.parent = this.transform;
                _playerInElevator = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                other.transform.parent = null;
                _playerInElevator = true;
            }
        }
    }
}