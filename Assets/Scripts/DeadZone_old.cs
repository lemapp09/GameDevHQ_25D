using System.Collections;
using UnityEngine;

public class DeadZone_old : MonoBehaviour
{
    [SerializeField]
    private GameObject _respawnPosition;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") ) {
            if (other.GetComponent<Player_old>() != null) {
                EnvironmentPlayer player = other.GetComponent<EnvironmentPlayer>();
                player.Damage();
                if (other.GetComponent<CharacterController>()) {
                    CharacterController cc = other.GetComponent<CharacterController>();
                    cc.enabled = false;
                    StartCoroutine(EnableCharacterController(cc));
                }
                other.transform.position = _respawnPosition.transform.position;
            }
        }
    }

    private IEnumerator EnableCharacterController(CharacterController cc)
    {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}
