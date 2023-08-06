using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player_old : MonoBehaviour
{
    private static Player_old _instance;
    public static Player_old Instance { get { return _instance; } }
    private PlayerInputActions _playerInputActions;
    private CharacterController _controller;
    private float _horizontal;
    private bool _canDoubleJump;
    private bool _isGrounded;
    private bool _isDead;
    private Vector3 _playerMovement;
    [SerializeField]
    private float _jumpHeight = 3.0f;

    private float _yVelocity = 0f;
    [SerializeField] 
    private float _gravity = 1.0f;
    [SerializeField]
    private float _playerSpeed = 5f;

    private int _coins = 0;
    [SerializeField]
    private int _lives = 3;
    public int Lives { get { return _lives; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
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
        UIManager_old.Instance.DisplayLivesTotal(_lives);
        UIManager_old.Instance.DisplayCoinTotal(_coins);
    }

    private void Update() {
        Movement();
    }

    private void Movement() {
        _isGrounded = _controller.isGrounded;
        _horizontal = _playerInputActions.Player.Movement.ReadValue<Vector2>().x;
        _playerMovement = _playerSpeed * Time.deltaTime * _horizontal * Vector3.right;
        if (_isGrounded) {
            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        } else {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && _canDoubleJump) {
                _yVelocity += _jumpHeight; 
                _canDoubleJump = false;
            }  else {
                _yVelocity = -_gravity * Time.deltaTime;
            }
        }
        _playerMovement += _yVelocity * Vector3.up;
        if (!_isDead) {
            _controller.Move(_playerMovement);
        }
    }

    public void CollectCoin() {
        _coins++;
        UIManager_old.Instance.DisplayCoinTotal(_coins);
    }

    public void Damage() {
        _lives--;
        UIManager_old.Instance.DisplayLivesTotal(_lives); 
        if (_lives < 1) {
            StartCoroutine(EndGameDelay());
        }
    }

    public IEnumerator EndGameDelay()
    {
        _isDead = true;
        UIManager_old.Instance.DisplayGameOver();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("SampleScene");
    }
}
