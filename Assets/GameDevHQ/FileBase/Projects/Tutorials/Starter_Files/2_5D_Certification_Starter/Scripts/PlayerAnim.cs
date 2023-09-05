using System.Collections;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnim : MonoBehaviour {
        [SerializeField] private Animator _anim;
        private bool _notSeen, _isJumping;
        [SerializeField] private bool _is_Stumbling;
        private int _speedAnimId, _notseenAnimId, _jumpAnimId, _ledgeGrabAnimId, _deathAnimId, _climbAnimId, _rollAnimId,
            _stumbleAnimId;

        protected virtual void Awake() {
            if (_speedAnimId == 0) {
                _speedAnimId = Animator.StringToHash("Speed"); }
            if (_notseenAnimId == 0) {
                _notseenAnimId = Animator.StringToHash("NotSeen"); }
            if (_jumpAnimId == 0) {
                _jumpAnimId = Animator.StringToHash("Jump"); }
            if (_ledgeGrabAnimId == 0) {
                _ledgeGrabAnimId = Animator.StringToHash("LedgeGrab"); }
            if (_deathAnimId == 0) {
                _deathAnimId = Animator.StringToHash("Death"); }
            if (_climbAnimId == 0) {
                _climbAnimId = Animator.StringToHash("Climb"); }
            if (_rollAnimId == 0) {
                _rollAnimId = Animator.StringToHash("Roll"); }
            if (_stumbleAnimId == 0) {
                _stumbleAnimId = Animator.StringToHash("Stumble"); }
        }

        private void Start() {
            if( _anim == null && GetComponent<Animator>() != null) {
                _anim = GetComponent<Animator>();
            } 
            if(_anim == null)
                    Debug.LogError("Animator is null");
            //     public static readonly int MyBool = Animator.StringToHash("MyBool");
            StartCoroutine(Stumbling());
        }
        
        public void SetSpeed(float speed) {
            _anim.SetFloat(_speedAnimId, speed);
        }

        public void IsGrounded()
        {
            if (_isJumping)
            {
                _isJumping = false;
                _anim.SetBool(_jumpAnimId, false); 
            }
        }

        public void NotSeen() {
            if (!_notSeen) {
                _anim.SetBool(_notseenAnimId, true);
            } else {
                _anim.SetBool(_notseenAnimId, false);
            }
            _notSeen = !_notSeen;
        }

        public void Death() {
            _anim.SetTrigger(_deathAnimId);
        }
        
        public void Jump() {
            if (!_isJumping) {
                StartCoroutine(JumpCoroutine());
            }
        }

        private IEnumerator JumpCoroutine() {
            _isJumping = true;
            _anim.SetBool(_jumpAnimId, true); 
            yield return new WaitForSeconds(0.90f);
            _anim.SetBool(_jumpAnimId, false); 
            _isJumping = false;
        }

        public void LedgeGrab() {
            _anim.SetBool(_ledgeGrabAnimId, true);
        }

        public void EndLedgeGrabJump() {
            _anim.SetFloat(_speedAnimId, 0.0f);
            _anim.SetBool(_jumpAnimId, false);
        }

        public void ClimbUp()
        {
            AudioManager.Instance.PlayManGrunting();
            _anim.SetBool(_climbAnimId, true);
        }
        public void ClimbEnd() {
            _anim.SetBool(_ledgeGrabAnimId, false);
        }

        public void Roll() {
            _anim.SetBool(_rollAnimId, true);
            AudioManager.Instance.PlayManMoan();
            StartCoroutine(RollDelay());
        }

        private IEnumerator RollDelay() {
            yield return new WaitForSeconds(2.25f);
            _anim.SetBool(_rollAnimId, false);
        }

        private IEnumerator Stumbling() {
            while (_is_Stumbling) {
                if (!_anim.GetBool(_jumpAnimId) && !_anim.GetBool(_ledgeGrabAnimId) &&
                    !_anim.GetBool(_climbAnimId) && !_anim.GetBool(_rollAnimId) && !_anim.GetBool(_stumbleAnimId))
                {
                    Debug.Log("Stumbling");
                    _anim.SetBool(_stumbleAnimId, true);
                    yield return new WaitForSeconds(1.0f);
                    _anim.SetBool(_stumbleAnimId, false);
                    yield return new WaitForSeconds(Random.Range(9.0f, 30.0f));
                }
            }
        }
    }
}