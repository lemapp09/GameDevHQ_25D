using GameDevHQ_25dCert;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class TriggerReducedGravity : MonoBehaviour
    {
        [SerializeField] private GameObject _reduceGravityBox;

        private void OnTriggerEnter(Collider other)
        {
            _reduceGravityBox.SetActive(true);
        }
    }
}