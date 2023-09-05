using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class Coin : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Player>().CollectCoin();
                Destroy(gameObject);
            }
        }
    }
}