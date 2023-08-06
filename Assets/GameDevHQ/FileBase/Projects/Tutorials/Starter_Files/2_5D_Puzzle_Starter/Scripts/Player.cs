using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace GameDevHQ_25D
{
    public class Player : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        private CharacterController _controller;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private float _gravity = 1.0f;
        [SerializeField] private float _jumpHeight = 15.0f;
        private float _yVelocity;
        private bool _canDoubleJump;
        [SerializeField] private int _coins;
        public int Coins { get { return _coins; } }

        private UIManager _uiManager;
        [SerializeField] private int _lives = 3;
        private Vector3 _direction, _velocity;
        private bool _canWallJump;
        private Vector3 _wallSurfaceNormal;
        [SerializeField]
        private float _boxPushPower = 1.0f;

        void Start()
        {
            _playerInputActions = new  PlayerInputActions();
            _playerInputActions.Player.Enable();
            _controller = GetComponent<CharacterController>();
            _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if (_uiManager == null)
            {
                Debug.LogError("The UI Manager is NULL.");
            }

            _uiManager.UpdateLivesDisplay(_lives);
        }

        void Update()
        {
            float horizontalInput = _playerInputActions.Player.Movement.ReadValue<float>();
            if (_controller.isGrounded )
            {
                _canWallJump = false;
                _direction = new Vector3(horizontalInput, 0, 0);
                _velocity = _direction * _speed;
                if ( Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = true;
                }
            }
            else
            { 
                if (Keyboard.current.spaceKey.wasPressedThisFrame && !_canWallJump)
                {
                    if (_canDoubleJump == true)
                    {
                        _yVelocity += _jumpHeight;
                        _canDoubleJump = false;
                    }
                }

                if (Keyboard.current.spaceKey.wasPressedThisFrame && _canWallJump)
                {
                    _yVelocity = _jumpHeight;
                    _velocity = _wallSurfaceNormal * _speed;
                }

                _yVelocity -= _gravity;
            }

            _velocity.y = _yVelocity;
            if (_controller.enabled == true)
            {
                _controller.Move(_velocity * Time.deltaTime);
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.transform.CompareTag("Wall") && !_controller.isGrounded)
            {
                _wallSurfaceNormal = hit.normal;
                _canWallJump = true;
            }
            if(hit.transform.CompareTag("Moveable Box")) {
                // confirm RigidBody
                Rigidbody moveableBox = hit.collider.attachedRigidbody;
                if (moveableBox == null || moveableBox.isKinematic){
                    return;
                }
                // push power - declare global variable
                // calculate move direction
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
                // push ( using movingBox velocity)
                moveableBox.velocity = pushDirection * _boxPushPower;
            }
        }

        public void AddCoins()
        {
            _coins++;

            _uiManager.UpdateCoinDisplay(_coins);
        }

        public void Damage()
        {
            _lives--;

            _uiManager.UpdateLivesDisplay(_lives);

            if (_lives < 1)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}