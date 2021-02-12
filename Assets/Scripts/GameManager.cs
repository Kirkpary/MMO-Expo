using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Oregonstate.MMOExpo
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public static GameManager Instance;
        [Tooltip("Optional: Default is (0, 1, 0)")]
        public GameObject PlayerSpawn;
        public static GameObject PlayerModel;
        #endregion

        #region MonoBehavior Callbacks
        private void Start()
        {   
            Instance = this;
            
            if (PlayerPrefs.GetString("selectedCharacter", "") == "")
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> playerPrefab Reference. Please set it up in Scene 'CharacterSelector' in GameObject 'Characters'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("Printing PlayerPrefs: {0}", PlayerPrefs.GetString("selectedCharacter"));

                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    Vector3 position = new Vector3(0f, 1f, 0f);
                    Quaternion rotation = Quaternion.identity;
                    if (PlayerSpawn != null)
                    {
                        position = PlayerSpawn.transform.position;
                        rotation = PlayerSpawn.transform.rotation;
                    }
                    else
                    {
                        Debug.LogWarning("<Color=yellow><a>Missing</a></Color> PlayerSpawn Reference. Defaulting to spawn at world coord (0, 1, 0). Please set it up in GameObject 'GameManager'", this);
                    }
                    PlayerModel = PhotonNetwork.Instantiate("Avatars/" + PlayerPrefs.GetString("selectedCharacter"), position, rotation, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        #endregion

        #region Photon Callbacks
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        }
        #endregion
    }
}