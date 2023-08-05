using UnityEngine;

public class Coin : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Player.Instance.CollectCoin();
            Destroy(gameObject);
        }
    }
}