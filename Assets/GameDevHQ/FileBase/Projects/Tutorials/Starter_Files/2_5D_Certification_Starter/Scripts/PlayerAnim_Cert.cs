using System.Collections;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnim_Cert : MonoBehaviour {
        [SerializeField] private Animator _anim;
        private bool _notSeen , _isJumping;

        private void Start() {
            if( _anim == null && GetComponent<Animator>() != null) {
                _anim = GetComponent<Animator>();
            } 
            if(_anim == null)
                    Debug.LogError("Animator is null");
        }

        public void SetSpeed(float speed) {
            _anim.SetFloat("Speed", speed);
        }

        public void IsGrounded()
        {
            if (_isJumping)
            {
                _isJumping = false;
                _anim.SetBool("Jump", false); 
            }
        }

        public void NotSeen() {
            if (!_notSeen) {
                _anim.SetBool("NotSeen", true);
            } else {
                _anim.SetBool("NotSeen", false);
            }
            _notSeen = !_notSeen;
        }

        public void Death() {
            _anim.SetTrigger("Death");
        }
        
        public void Jump() {
            if (!_isJumping) {
                StartCoroutine(JumpCoroutine());
            }
        }

        private IEnumerator JumpCoroutine() {
            _isJumping = true;
            _anim.SetBool("Jump", true); 
            yield return new WaitForSeconds(0.90f);
            _anim.SetBool("Jump", false); 
            _isJumping = false;
        }

        public void LedgeGrab()
        {
            _anim.SetTrigger("LedgeGrab");
        }
    }
}