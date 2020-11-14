using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Oregonstate.MMOExpo
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [Header("Make sure there is a scene with the same name")]
        [Tooltip("The name of the room/scene to load")]
        [SerializeField]
        private string roomName;
        #endregion

        #region Private Fields
        /// <summary>
        /// This client's version number. Users are separated form each other by gameVersion (which allows you to make breaking changes).
        /// Format: Major.Minor.Patch
        /// accessible anywhere with: 'Launcher.gameVersion'
        /// </summary>
        static string gameVersion = "1.0.0";
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronus and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;
        #endregion

        #region MonoBehavior Callbacks
        /// <summary>
        /// MonoBehavior method called on GameObject by Unity during early initialization phase.
        /// </summary>
        // Start is called before the first frame update
        void Awake()
        {
            // #Critical
            // if true, this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = false;   //TODO Delete. Probably not needed for MMO Expo
        }

        
        /// <summary>
        /// MonoBehavior method called on GameObject by Unity during initilization phase.
        /// </summary>
        private void Start()
        {
            //Connect();    // Replaced with UI play button
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are, else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                // keep track of the will to join a roomm, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential exisitng room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRoom(roomName);
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRoomFailed() was called by PUN. No room called '" + roomName + "' available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            Debug.Log("Load '" + PhotonNetwork.CurrentRoom.Name + "'");
            // #Critical
            // Load the Room level with the same name as the current room
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
        }
        #endregion
    }
}
