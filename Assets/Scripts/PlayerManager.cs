using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
        CharacterController characterController;
        NavMeshAgent agent;
        GraphicRaycaster graphicRaycaster;
        PointerEventData pointerEventData;
        EventSystem eventSystem;
        #endregion

        #region Public Fields
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;
        [Tooltip("Enable if testing without connection to photon")]
        public bool OfflineDebugging = false;
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
            if (photonView.IsMine || OfflineDebugging)
            {
                // Get the Raycaster from the Canvas
                graphicRaycaster = GameObject.FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>();
                // Get the Event System form the Scene
                eventSystem = GetComponent<EventSystem>();

                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
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
                Debug.LogWarning("<Color=Yellow><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        void Update()
        {
            if (photonView.IsMine || OfflineDebugging)
            {
                ProcessInputs();
            }
        }

        void FixedUpdate()
        {
            if (photonView.IsMine && ChatGui.chatManager.IsChatConnected)
            {
                ChatGui.chatManager.SubscirbeToClosestBooth(transform.position);
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
            // Get the Raycaster from the Canvas
            graphicRaycaster = GameObject.FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>();
            // Get the Event System form the Scene
            eventSystem = GetComponent<EventSystem>();

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
            if (agent != null)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if (h == 0 && v == 0)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        // Check for UI click
                        pointerEventData = new PointerEventData(eventSystem);
                        pointerEventData.position = Input.mousePosition;

                        List<RaycastResult> results = new List<RaycastResult>();

                        graphicRaycaster.Raycast(pointerEventData, results);

                        // Only set destination if the ui is not clicked on
                        if (results.Count <= 0)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                            {
                                agent.destination = hit.point;
                            }
                        }
                    }
                }
                else
                {
                    if (!ChatGui.isChatEnabled || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                    {
                        agent.isStopped = true;
                        agent.ResetPath();
                        transform.Rotate(0, h * (agent.angularSpeed * Time.deltaTime), 0, Space.Self);
                        agent.Move(transform.forward * v * agent.speed * Time.deltaTime);
                    }
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> NavMeshAgent Component on playerPrefab.", this);
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