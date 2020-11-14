using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace Com.Oregonstate.MMOExpo
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;

        private PlayerManager target;

        [Tooltip("Pixel offset form the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

        //float characterControllerHeight = 0f;
        float boxColliderHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;
        #endregion

        #region MonoBehavior Callbacks
        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            // Destroy ifself if the target is null,, It's a fail safe when Photon is detroying Instances of a Player over the network
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        private void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            // #Critical
            // Follow the Target GameObject on sceen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                //targetPosition.y += characterControllerHeight;
                targetPosition.y += boxColliderHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }
        #endregion

        #region Public Methods
        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            //TODO May need to change to character controller
            //CharacterController characterController = _target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            /*if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }*/
            BoxCollider boxCollider = _target.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxColliderHeight = boxCollider.size.y;
            }
        }
        #endregion
    }
}
