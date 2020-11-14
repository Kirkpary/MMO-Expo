using System.Collections;
using UnityEngine;

using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields
        [SerializeField]
        private float directionDampTime = 0.25f;
        #endregion

        #region MonoBehaviour Callbacks
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayAnimatorManager is Missing Animaotr Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!animator)
            {
                return;
            }
            // deal with jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("Jump");
                }
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
        #endregion
    }
}