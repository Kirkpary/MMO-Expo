using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields
        NavMeshAgent agent;
        #endregion

        #region Public Fields
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;
        [SerializeField]
        public CharacterController characterController;
        [SerializeField]
        public float moveSpeed = 0.01f;
        [SerializeField]
        public float rotationSpeed = 0.4f;
        #endregion

        #region Private Methods
#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
        #endregion

        #region MonoBehavior Callbacks
        /// <summary>
        /// MonoBehavior method called on GameObject by Unity during early initilization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchrononized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);     //TODO Delete. Probably not needed for MMO
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }

            if (PlayerUiPrefab != null)
            {
                // Only show the floating nametag on other players
                if (!photonView.IsMine)
                {
                    GameObject _uiGo = Instantiate(PlayerUiPrefab);
                    _uiGo.GetComponent<PlayerUI>().SetTarget(this);
                }
            }
            else
            {
                Debug.LogWarning("<Color=red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
        }

#if !UNITY_5_4_OR_NEWER
        void OnLevelWasLoaded(int level) 
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            _uiGo.GetComponent<PlayerUI>().SetTarget(this);
        }

#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif
        #endregion

        #region Custom
        /// <summary>
        /// Processes the inputs.
        /// </summary>
        void ProcessInputs()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h == 0 && v == 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        agent.destination = hit.point;
                    }
                }
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
                if (characterController != null) {
                    characterController.Move(transform.TransformDirection(Vector3.forward) * v * (moveSpeed * Time.deltaTime));
                    transform.Rotate(0, h * (rotationSpeed * Time.deltaTime), 0, Space.Self);
                }
                else
                {
                    Debug.LogError("<Color=Red><a>Missing</a></Color> CharacterController Component on playerPrefab.", this);
                }
            }
        }
        #endregion

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // Send and recieve custom variables
        }
#endregion
    }
}