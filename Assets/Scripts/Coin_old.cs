using UnityEngine;

public class Coin_old : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Player_old.Instance.CollectCoin();
            Destroy(gameObject);
        }
    }
}
