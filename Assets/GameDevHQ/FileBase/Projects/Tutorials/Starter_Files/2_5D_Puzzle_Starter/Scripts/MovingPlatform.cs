using UnityEngine;

namespace GameDevHQ_25D
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _targetA, _targetB;
        [SerializeField] private float _speed = 3.0f;
        private bool _switching = false;


        // Update is called once per frame
        void FixedUpdate()
        {
            if (_switching == false)
            {

                transform.position =
                    Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
            }
            else if (_switching == true)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
            }

            if (transform.position == _targetB.position)
            {
                _switching = true;
            }
            else if (transform.position == _targetA.position)
            {
                _switching = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.transform.parent = this.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                other.transform.parent = null;
            }
        }
    }
}
