using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;

namespace Com.Oregonstate.MMOExpo
{
    /// <summary>
    /// Player name input field. Let the user input his name, will appear above the player in the game.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "Playername";
        #endregion

        #region MonoBehavior Callbacks
        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                }
            }
            _inputField.text = defaultName;
            SetPlayerName(defaultName);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Inportant
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogWarning("Player Name is null or empty");
                // If value is empty create a new username
                value = "user";
                // 4 = 10^5 = 100,000
                for (int i = 0; i < 5; i++)
                {
                    value += (char)Random.Range('0', '9');
                }
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}
