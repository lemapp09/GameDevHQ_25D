using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GameDevHQ_25dCert
{
    public class LiftingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _targetA, _targetB;
        [SerializeField] private float _speed = 3.0f, _timeDelay = 5f;
        [SerializeField] private bool _goingDown,  _playerInElevator,  _isMoving;
        [FormerlySerializedAs("isAu8tomated")] [SerializeField] private bool _isAutomated = true, _isDelayed;

        private void Update() {
            if (_isAutomated) {
                if (!_isMoving & !_isDelayed) {
                    StartCoroutine(FiveSecondCallElevator());
                }
            }  else  {
                if (_playerInElevator) {
                    if (Keyboard.current.eKey.wasPressedThisFrame) {
                        CallElevator();
                    }
                }
            }
        }

        private IEnumerator FiveSecondCallElevator()
        {
            _isMoving = true;
            _isDelayed = true;
            yield return new WaitForSeconds(_timeDelay);
            CallElevator();
            _isDelayed = false;
        }

        void FixedUpdate() {
            if (_isMoving & !_isDelayed) {
                if (_goingDown) {
                    transform.position =
                        Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _targetB.position) < 0.1f) {
                        _isMoving = false;
                        AudioManager.Instance.StopElevator();
                    }
                }  else {
                    transform.position =
                        Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _targetA.position) < 0.1f) {
                        _isMoving = false;
                        AudioManager.Instance.StopElevator();
                    }
                }
            }
        }

        public void CallElevator() {
            // know the current location of elevator
            _goingDown = !_goingDown;
            _isMoving = true;
            AudioManager.Instance.PlayElevator();
        }

        private void OnTriggerEnter(Collider other) {
            if (!_isAutomated) {
                if (other.CompareTag("Player")) {
                    other.transform.parent = this.transform;
                    _playerInElevator = true;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!_isAutomated) {
                if (other.CompareTag("Player")) {
                    other.transform.parent = null;
                    _playerInElevator = true;
                }
            }
        }
    }
}