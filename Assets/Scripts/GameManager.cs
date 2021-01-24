using UnityEngine;
using System.Collections;
using System.IO;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Oregonstate.MMOExpo
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public static GameManager Instance;
        #endregion

        #region Private Fields
        string JsonPath;
        string JsonString;
        #endregion

        #region MonoBehavior Callbacks
        private void Start()
        {   
            Instance = this;

            // Reading JSON file for this room (CS_Room)
            JsonPath = Application.streamingAssetsPath + "/CS_Room.json";
            JsonString = File.ReadAllText(JsonPath);
            Room CS_Room = JsonUtility.FromJson<Room>(JsonString);
            Debug.Log(CS_Room.NumBooths);
            
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
                    PhotonNetwork.Instantiate("Avatars/" + PlayerPrefs.GetString("selectedCharacter"), new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
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

    [System.Serializable]
    public class Room 
    {
        public int NumBooths;
    }
}