using UnityEngine;

namespace GameDevHQ_25D {
    public class PressurePad : MonoBehaviour {
        [SerializeField] private MeshRenderer _display;
        private bool _boxIsInPlace;

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Moveable Box")) {
                if (!_boxIsInPlace) {
                    float distance = Vector3.Distance(transform.position, other.transform.position);
                    if (distance < 0.05f) {
                        _display.material.color = Color.blue;
                        Rigidbody moveableBox = other.GetComponent<Rigidbody>();
                        if (moveableBox != null) {
                            moveableBox.isKinematic = true;
                            _boxIsInPlace = true;
                            Destroy(this);
                        }
                    }
                }
            }
        }
    }
}