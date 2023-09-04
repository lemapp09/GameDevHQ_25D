using UnityEngine;

namespace GameDevHQ_25dCert
{

    public class AutomatedGate : MonoBehaviour
    {
        [SerializeField] private BoxCollider _liftWall;
        private bool _isMoving;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isMoving)
            {
                if (other.CompareTag("Player"))
                {
                    Debug.Log("Opening Gate");
                    StartCoroutine(OpenGate());
                }
            }
        }


        private System.Collections.IEnumerator OpenGate()
        {
            _isMoving = true;
            var bounds = this.GetComponent<Collider>().bounds;
            float moveAmounts = bounds.extents.y * 0.04f;
            _liftWall.enabled = false;
            var i = 25;
            while (i > 0)
            {
                this.transform.position += new Vector3(0, -moveAmounts, 0);
                new WaitForSeconds(0.05f);
                i--;
            }

            yield return new WaitForSeconds(15);
            i = 25;
            while (i > 0)
            {
                this.transform.position += new Vector3(0, moveAmounts, 0);
                new WaitForSeconds(0.05f);
                i--;
            }

            _liftWall.enabled = true;
            _isMoving = false;
        }
    }
}