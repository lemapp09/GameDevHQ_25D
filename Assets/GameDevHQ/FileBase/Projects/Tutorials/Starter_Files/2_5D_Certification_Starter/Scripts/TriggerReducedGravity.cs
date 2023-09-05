using System.Collections;
using GameDevHQ_25dCert;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class TriggerReducedGravity : MonoBehaviour
    {
        [SerializeField] private GameObject _reduceGravityBox;
        [SerializeField] private GameObject _ledgeChecker;

        private void OnTriggerEnter(Collider other)
        {
            _reduceGravityBox.SetActive(true);
            _ledgeChecker.SetActive(true);
            StartCoroutine(TurnOffBox());
        }

        private IEnumerator TurnOffBox()
        {
            yield return  new WaitForSeconds(20f);
            _reduceGravityBox.SetActive(false);
            _ledgeChecker.SetActive(false);
        }
    }
}