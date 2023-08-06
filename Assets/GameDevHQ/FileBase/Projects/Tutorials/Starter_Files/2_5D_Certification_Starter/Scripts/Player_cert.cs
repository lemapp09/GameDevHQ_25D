using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameDevHQ_25dCert
{

    [RequireComponent(typeof(CharacterController))]
    public class Player_cert : MonoBehaviour
    {
        private static Player_cert _instance;

        public static Player_cert Instance
        {
            get { return _instance; }
        }

        private PlayerInputActions _playerInputActions;
        private CharacterController _controller;
        private float _horizontal;
        private bool _canDoubleJump;
        [SerializeField] private bool _isGrounded;
        private bool _isDead;
        private Vector3 _playerMovement;
        [SerializeField] private float _jumpHeight = 3.0f;

        private float _yVelocity = 0f;
        [SerializeField] private float _gravity = 1.0f;
        [SerializeField] private float _playerSpeed = 5f;

        [SerializeField] private GameObject _playerBody;
        [SerializeField] private bool _walkingForward = true, _canRotate = true;

        private int _coins = 0;
        [SerializeField] private int _lives = 3;

        [SerializeField] private Animator _anim;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else  {
                _instance = this;
            }
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        private void Start() {
            if (GetComponent<CharacterController>() != null) {
                _controller = GetComponent<CharacterController>();
            }  else {
                Debug.Log("There is no Character Controller attached to Player");
            }
            UIManager_cert.Instance.DisplayLivesTotal(_lives);
            UIManager_cert.Instance.DisplayCoinTotal(_coins);
        }

        private void Update() {
            Movement();
        }

        private void Movement() {
            _isGrounded = _controller.isGrounded;
            _horizontal = _playerInputActions.Player.Movement.ReadValue<Vector2>().x;
            // Rotate Player when the game player switches direction
            if (_canRotate)
            {
                if (_horizontal > 0 && !_walkingForward)
                {
                    StartCoroutine(RotatePlayer(1));
                }
                else if (_horizontal < 0 && _walkingForward)
                {
                    StartCoroutine(RotatePlayer(-1));
                }
            }

            _playerMovement = _playerSpeed * Time.deltaTime * _horizontal * Vector3.forward;
            if (_isGrounded) {
                if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = true;
                }
            } else  {
                if (Keyboard.current.spaceKey.wasPressedThisFrame && _canDoubleJump) {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                } else {
                    _yVelocity = -_gravity * Time.deltaTime;
                }
            }

            _playerMovement += _yVelocity * Vector3.up;
            if (!_isDead) {
                _controller.Move(_playerMovement);
                _anim.SetFloat("Speed", Mathf.Abs(_controller.velocity.z));
            }
        }

        private IEnumerator RotatePlayer(int i)
        {
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
            _walkingForward = !_walkingForward;
            _canRotate  = true;
        }

        public void CollectCoin() {
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
    }
}