using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private bool _xOrY;
        [SerializeField] private float _speed = 2f;
        private Vector3 _startingPosition;
        private Vector3 _endingPosition;
        [SerializeField] private float _DistanceToTravel = 5f;
        private bool _movingForward = true;
        private Vector3 _nextPosition;

        private void Start()
        {
            _startingPosition = transform.position;
            if (_xOrY) {
                _endingPosition = transform.position + new Vector3(_DistanceToTravel, 0, 0);
            }  else {
                _endingPosition = transform.position + new Vector3(0, _DistanceToTravel, 0);
            }
            _nextPosition = _endingPosition;
        }

        void FixedUpdate()
        {
            MovePlatform();
        }

        private void MovePlatform()
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextPosition, step);

            if (Vector3.Distance(transform.position, _nextPosition) < 0.01f)
            {
                _movingForward = !_movingForward;
                if (_movingForward)
                {
                    _nextPosition = _endingPosition;
                }
                else
                {
                    _nextPosition = _startingPosition;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = this.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = null;
            }
        }
    }
}