using UnityEngine;

namespace GameDevHQ_25dCert
{

    [RequireComponent(typeof(Collider))]
    public class LedgeGrab_cert : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Ledge_Checker")) {
                Player_cert player = other.transform.parent.GetComponent<Player_cert>();
                if (player != null)
                {
                    player.LedgeGrab();
                }
            }
        }
    }
}