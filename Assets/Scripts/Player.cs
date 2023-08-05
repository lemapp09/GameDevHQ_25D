using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    private PlayerInputActions _playerInputActions;
    private CharacterController _controller;
    private float _horizontal;
    private bool _canDoubleJump;
    private bool _isGrounded;
    private Vector3 playerMovement;
    [SerializeField]
    private float _jumpHeight = 3.0f;

    private float _yVelocity = 0f;
    [SerializeField] 
    private float _gravity = 1.0f;
    [SerializeField]
    private float _playerSpeed = 5f;

    private int _coins;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    private void Start()
    {
        if (GetComponent<CharacterController>() != null) {
            _controller = GetComponent<CharacterController>(); 
        }  else {
            Debug.Log("There is no Character Controller attached to Player");
        }
    }

    private void Update() {
        Vector3 _effectGravity = Vector3.zero;
        _isGrounded = _controller.isGrounded;
        _horizontal = _playerInputActions.Player.Movement.ReadValue<Vector2>().x;
        playerMovement = _playerSpeed * Time.deltaTime * _horizontal * Vector3.right;
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
        playerMovement.y = _yVelocity;
        _controller.Move(playerMovement);
    }

    public void CollectCoin() {
        _coins++;
        UIManager.Instance.DisplayCoinTotal(_coins);
    }
    
}
