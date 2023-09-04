using UnityEngine;

namespace GameDevHQ_25dCert
{

    [RequireComponent(typeof(Collider))]
    public class LedgeGrab : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField] private Transform _standPos;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }     

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<Player>().LedgeGrab(this);
            }
            if (other.CompareTag("Ledge_Checker")) {
                Player player = other.transform.parent.GetComponent<Player>();
                if (player != null) {
                    player.LedgeGrab(this);
                }
            }
        }

        public Vector3 GetStandPos() {
            return _standPos.position;
        }
    }
}