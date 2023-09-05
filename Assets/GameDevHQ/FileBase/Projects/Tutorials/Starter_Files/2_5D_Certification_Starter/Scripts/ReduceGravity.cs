using Unity.VisualScripting;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class ReduceGravity : MonoBehaviour {
    private Player _player;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _player = other.GetComponent<Player>();
                Vector3 animVelocity;
                Vector3 CCVelocity;
                Animator anim = other.GetComponent<Animator>();
                animVelocity = anim != null ? anim.velocity : Vector3.zero;

                CharacterController controller = other.GetComponent<CharacterController>();
                CCVelocity = controller != null ? controller.velocity : Vector3.zero;
                
                // get Player direction of movement. Is he going right or left?
                Vector3 playerDirection = other.transform.forward;
                var dotProduct = Vector3.Dot(Vector3.forward,playerDirection);

                if (dotProduct > 0) {
                    if (animVelocity.z > 0 || CCVelocity.z > 0) {
                        if(_player != null)
                            _player.ReduceGravity(true);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                if(_player != null)
                    _player.ReduceGravity(false);
            }
        }
    }
}
