using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class Coin_cert : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player_old.Instance.CollectCoin();
                Destroy(gameObject);
            }
        }
    }
}