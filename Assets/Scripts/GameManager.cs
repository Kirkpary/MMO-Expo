using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Oregonstate.MMOExpo
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public static GameManager Instance;
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
                    PhotonNetwork.Instantiate("Avatars/" + PlayerPrefs.GetString("selectedCharacter"), new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
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