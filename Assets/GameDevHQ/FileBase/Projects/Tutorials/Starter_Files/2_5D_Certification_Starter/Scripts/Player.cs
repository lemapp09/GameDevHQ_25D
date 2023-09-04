using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace GameDevHQ_25dCert
{

    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        private static Player _instance;

        public static Player Instance
        {
            get { return _instance; }
        }

        [SerializeField] private PlayerAnim _playerAnim;
        private PlayerInputActions _playerInputActions;
        private CharacterController _controller;
        [SerializeField] private GameObject _playerBody;

        [SerializeField] private float _horizontal, _yVelocity = 0f, _gravity,  speed = 500f;
        private int _coins = 0;
        private bool _isDead, _canDoubleJump, _ledgeIsGrabbed;
        private Vector3 _playerMovement;
        [SerializeField] private float _jumpHeight = 3.0f, _gravityMax = 9.8f, _playerSpeed = 5f;
        [SerializeField] private bool _isGrounded, _walkingForward = true, _canRotate = true;
        [SerializeField] private int _lives = 3;
        [SerializeField] private Transform _ledgeGrabPoint;
        public Vector3 target;
        public float arcHeight = 1;
        private LedgeGrab _currentLedge;

        Vector3 _startPosition;
        float _stepScale;
        float _progress;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else  {
                _instance = this;
            }
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Climb.performed += ctx => ClimbUp();
            _gravity = _gravityMax;
        }

        private void Start() {
            if (GetComponent<CharacterController>() != null) {
                _controller = GetComponent<CharacterController>();
            } else {
                Debug.Log("There is no Character Controller attached to Player");
            }
            UIManager.Instance.DisplayLivesTotal(_lives);
            UIManager.Instance.DisplayCoinTotal(_coins);
        }

        private void Update() {
            Movement();
        }

        private void Movement()
        {
            _isGrounded = _controller.isGrounded;
            _horizontal = _playerInputActions.Player.Movement.ReadValue<Vector2>().x;
            // Rotate Player when the game player switches direction
            if (_canRotate) {
                if (_horizontal > 0 && !_walkingForward) {
                    StartCoroutine(RotatePlayer(1));
                }
                else if (_horizontal < 0 && _walkingForward) {
                    StartCoroutine(RotatePlayer(-1));
                }
            }

            _playerMovement = _playerSpeed * Time.deltaTime * _horizontal * Vector3.forward;
            if (_isGrounded) {
                if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                    _playerAnim.Jump();
                    _yVelocity = _jumpHeight; 
                    _canDoubleJump = true;
                    _playerAnim.IsGrounded();
                }
            } else {
                if (Keyboard.current.spaceKey.wasPressedThisFrame && _canDoubleJump) {
                    _playerAnim.Jump();
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                } else {
                    _yVelocity = -_gravity * Time.deltaTime;
                }
            }

            _playerMovement += _yVelocity * Vector3.up;
            if (!_isDead) {
                if (_ledgeIsGrabbed) {
                    _playerMovement = Vector3.zero;
                }
                _controller.Move(_playerMovement);
                _playerAnim.SetSpeed(Mathf.Abs(_controller.velocity.z));
            }
        }

        private IEnumerator RotatePlayer(int i)
        {
            if (_ledgeIsGrabbed == false) {
                _canRotate = false;
                Quaternion toRotation = Quaternion.identity;
                switch (i) {
                    case 1:
                        toRotation.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case -1:
                        toRotation.eulerAngles = new Vector3(0, 180, 0);
                        break;
                }
                Quaternion fromRotation = _playerBody.transform.rotation;
                float count = 0f;
                while (count <= 12) {
                    _playerBody.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, count * 0.08333f);
                    count++;
                    yield return new WaitForSeconds(0.01f);
                }

                switch (i)
                {
                    case 1:
                        _playerBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case -1:   
                        _playerBody.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                }
                _walkingForward = !_walkingForward;
                _canRotate = true;
            }
        }

        public void CollectCoin()
        {
            _coins++;
            UIManager_old.Instance.DisplayCoinTotal(_coins);
        }

        public void Damage() {
            Debug.Log("Damage Reached");
            _lives--;
            UIManager_old.Instance.DisplayLivesTotal(_lives);
            if (_lives < 1) {
                StartCoroutine(EndGameDelay());
            }
        }

        public IEnumerator EndGameDelay() {
            _isDead = true;
            UIManager_old.Instance.DisplayGameOver();
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(0);
        }

        public void LedgeGrab(LedgeGrab currentLedge)
        {
            _playerAnim.LedgeGrab();
            _ledgeIsGrabbed = true;
            _currentLedge = currentLedge;
        }

        public void ClimbUp() {
            if (_ledgeIsGrabbed) {
                _playerAnim.ClimbUp();
            }
        }

        public void ClimbComplete() {
            transform.position = _currentLedge.GetStandPos();
            _ledgeIsGrabbed = false;
            _playerAnim.ClimbEnd();
            _controller.enabled = true;
        }

        public void ReduceGravity(bool ToReduceGravity) {
            if (ToReduceGravity) {
                StartCoroutine(MovePlayerToLedge());
            } else {
                _gravity = _gravityMax;
            }
        }

        private IEnumerator MovePlayerToLedge()
        {
            _ledgeIsGrabbed = true;
            _gravity = 2f;
            _playerAnim.LedgeGrab();
            float timeInCycle = 0f;
            float timeToJumpAgain = 0.9f;
            Vector3 startingPosition = transform.position;
            float startingDistance = Mathf.Abs( transform.position.z  - _ledgeGrabPoint.position.z);
            float distanceY = _ledgeGrabPoint.position.y - startingPosition.y;
            float distanceZ = _ledgeGrabPoint.position.z - startingPosition.z;
            // move in 60 frames
            int i = 0;
            while (i <= 60 )
            {
                timeInCycle += Time.deltaTime;
                float yOffset = startingPosition.y +  Mathf.Sin((i / 60 ) * Mathf.PI * 1.5f) + ((distanceY * i) / 60);
                float zOffset = startingPosition.z + ((distanceZ * i) / 60);
                transform.position = new Vector3(0, yOffset, zOffset);
                yield return new WaitForSeconds(0.01f);
                _playerAnim.LedgeGrab();
                i++;
            }
            transform.position = _ledgeGrabPoint.position;
            _playerAnim.EndLedgeGrabJump();
            yield return new WaitForSeconds(0.01f);
            _gravity = _gravityMax;
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
            _playerInputActions.Player.Climb.performed -= ctx => ClimbUp();
        }
    }
}
