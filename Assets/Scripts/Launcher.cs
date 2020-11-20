using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Oregonstate.MMOExpo
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        //[Header("Make sure there is a scene with the same name. Not needed for leave game button.")]
        //[Tooltip("The name of the room/scene to load")]
        //[SerializeField]
        private string roomName;
        [Header("Optional - Shown while connecting.")]
        [Tooltip("The UI Label to inform the user that the connection is in progress.")]
        [SerializeField]
        private GameObject progressLabel;
        [Header("Optional - Hidden while connecting.")]
        [Tooltip("The UI Panel to let the user enter name, connect and play.")]
        [SerializeField]
        private GameObject controlPanel;
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
        /// <summary>
        /// Keep track of whether the user is trying to return to the menu or just switching rooms.
        /// </summary>
        bool switchRoom;
        bool leaveGame;
        #endregion

        #region MonoBehavior Callbacks
        /// <summary>
        /// MonoBehavior method called on GameObject by Unity during initilization phase.
        /// </summary>
        private void Start()
        {
            switchRoom = false;
            leaveGame = false;
            if (progressLabel != null)
            {
                progressLabel.SetActive(false);
            }
            if (controlPanel != null)
            {
                controlPanel.SetActive(true);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining the specified room
        ///     - If already in a room, leave room first
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect(string roomName)
        {
            this.roomName = roomName;
            if (roomName == "")
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> roomName string. Please set it up in call to Launcher.Connect()");
            }
            if (progressLabel != null)
            {
                progressLabel.SetActive(false);
            }
            if (controlPanel != null)
            {
                controlPanel.SetActive(true);
            }
            // we check if we are connected or not, we join if we are, else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // Join a room if not currently in one otherwise leave
                if(PhotonNetwork.CurrentRoom == null)
                {
                    // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                    PhotonNetwork.JoinRoom(roomName);
                }
                else
                {
                    Debug.Log("MMO Expo/Launcher: Connect() Already connected to Room. Leaving room...");
                    switchRoom = true;
                    PhotonNetwork.LeaveRoom();
                }
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                // keep track of the will to join a roomm, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        /// <summary>
        /// Return to the main menu.
        /// </summary>
        public void LeaveRoom()
        {
            leaveGame = true;
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("MMO Expo/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting || switchRoom)
            {
                // #Critical: The first we try to do is to join a potential exisitng room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRoom(roomName);
                isConnecting = false;
                switchRoom = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            switchRoom = false;
            if (progressLabel != null)
            {
                progressLabel.SetActive(false);
            }
            if (controlPanel != null)
            {
                controlPanel.SetActive(true);
            }
            Debug.LogWarningFormat("MMO Expo/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("MMO Expo/Launcher: OnJoinRoomFailed() was called by PUN. No room called '" + roomName + "' available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join the specified room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("MMO Expo/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            Debug.Log("Load '" + roomName + "'");
            // #Critical
            // Load the level with the same name as the current room
            PhotonNetwork.LoadLevel(roomName);
        }

        /// <summary>
        /// Called when the local player left the room.
        /// </summary>
        public override void OnLeftRoom()
        {
            Debug.Log("MMO Expo/Launcher: OnLeftRoom() called by PUN.");
            if (leaveGame)
            {
                // Return to menu
                SceneManager.LoadScene(0);
            }
           
            leaveGame = false;
        }
        #endregion
    }
}
