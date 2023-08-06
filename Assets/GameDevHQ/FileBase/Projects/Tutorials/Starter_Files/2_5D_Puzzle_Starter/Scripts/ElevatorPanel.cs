using UnityEngine;
using UnityEngine.InputSystem;


namespace GameDevHQ_25D
{
    public class ElevatorPanel : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _callbutton;
        [SerializeField] private Elevator _elevator;
        private int _coins = 0;
        [SerializeField] private int _coinsNeedToCall = 3;
        private bool _playerIsInTheBox;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerIsInTheBox = true;
                _coins = other.GetComponent<Player>().Coins;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerIsInTheBox = false;
                _coins = 0;
            }
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && _playerIsInTheBox
                                                          && _coins >= _coinsNeedToCall)
            {
                _callbutton.material.color = Color.green;
                _elevator.CallElevator();
            }
        }
    }
}