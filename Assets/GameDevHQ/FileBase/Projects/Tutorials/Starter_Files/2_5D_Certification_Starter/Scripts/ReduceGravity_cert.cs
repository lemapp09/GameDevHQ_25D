using System;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class ReduceGravity_cert : MonoBehaviour {
        [SerializeField] private Player_cert _player;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _player.ReduceGravity(true);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                _player.ReduceGravity(false);
            }
        }
    }
}
