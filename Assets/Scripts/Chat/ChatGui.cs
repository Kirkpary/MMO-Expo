// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatGui.cs" company="Exit Games GmbH">
//   Part of: PhotonChat demo,
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

/// <summary>
/// This simple Chat UI demonstrate basics usages of the Chat Api
/// </summary>
/// <remarks>
/// The ChatClient basically lets you create any number of channels.
///
/// some friends are already set in the Chat demo "DemoChat-Scene", 'Joe', 'Jane' and 'Bob', simply log with them so that you can see the status changes in the Interface
///
/// Workflow:
/// Create ChatClient, Connect to a server with your AppID, Authenticate the user (apply a unique name,)
/// and subscribe to some channels.
/// Subscribe a channel before you publish to that channel!
///
///
/// Note:
/// Don't forget to call ChatClient.Service() on Update to keep the Chatclient operational.
/// </remarks>
namespace Com.Oregonstate.MMOExpo
{
	public class ChatGui : MonoBehaviour, IChatClientListener
	{

		public List<String> ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

		public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

		private string selectedChannelName; // mainly used for GUI/input

		private static bool chatEnabled = false;
		public static bool isChatEnabled { 
			get { return chatEnabled; }
		}

		public ChatClient chatClient;

		#if !PHOTON_UNITY_NETWORKING
		[SerializeField]
		#endif
		protected internal ChatAppSettings chatAppSettings;

		[Tooltip("Range of the porximity booth chat.")]
		[Range(0, Mathf.Infinity)]
		public float boothChatRange = 5;
		[Tooltip("Main chat panel that holds all the chat UI")]
		public RectTransform ChatPanel;     // set in inspector (to enable/disable panel)
		[Tooltip("Chat input box")]
		public InputField InputFieldChat;   // set in inspector
		[Tooltip("Chat Output Text Object")]
		public Text CurrentChannelText;     // set in inspector
		[Tooltip("Channel Toggle from Channel Panel")]
		public GameObject ChannelToggleToInstantiate; // set in inspector

		private readonly Dictionary<string, GameObject> channelToggles = new Dictionary<string, GameObject>();

		[Tooltip("Text object to display the current user ID. Not currently part of the chat panel.")]
		public Text UserIdText; // set in inspector

		[Tooltip("Text object that tells the user how to open the chat.")]
		public Text OpenChatPrompt;

		private static GameObject[] booths = null;
		private string boothChannel;

		public static ChatGui chatManager;
		private bool isChatConnected = false;
		public bool IsChatConnected
		{
			get { return isChatConnected; }
		}

		// private static string WelcomeText = "Welcome to chat. Type /help to list commands.";
		private static string HelpText = "\n    -- HELP --\n" +
			"To subscribe to channel(s) (channelnames are case sensitive) :  \n" +
				"\t<color=#DC4405>/subscribe</color> <color=green><list of channelnames></color>\n" +
				"\tor\n" +
				"\t<color=#DC4405>/s</color> <color=green><list of channelnames></color>\n" +
				"\n" +
				"To leave channel(s):\n" +
				"\t<color=#DC4405>/unsubscribe</color> <color=green><list of channelnames></color>\n" +
				"\tor\n" +
				"\t<color=#DC4405>/u</color> <color=green><list of channelnames></color>\n" +
				"\n" +
				"To switch the active channel\n" +
				"\t<color=#DC4405>/join</color> <color=green><channelname></color>\n" +
				"\tor\n" +
				"\t<color=#DC4405>/j</color> <color=green><channelname></color>\n" +
				"\n" +
				"To send a private message: (username are case sensitive)\n" +
				"\t<color=#DC4405>/msg</color> <color=green><username></color> <color=green><message></color>\n" +
				"\n" +
				"To clear the current chat tab (private chats get closed):\n" +
				"\t<color=#DC4405>/clear</color>";


		public void Start()
		{
			//TODO DELETE
			//DontDestroyOnLoad(this.gameObject);

			chatManager = this;

			if (UserIdText != null)
			{
				this.UserIdText.text = "";
				this.UserIdText.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError("<Color=red><a>Missing</a></Color> userIdText Reference. Please set it up in GameObject 'Chat Panel'", this);
			}
			if (ChatPanel != null) {
				this.ChatPanel.gameObject.SetActive(false);
			}
			else
			{
				Debug.LogError("<Color=red><a>Missing</a></Color> userIdText Reference. Please set it up in GameObject 'Chat Panel'", this);
			}

			chatEnabled = false;
			isChatConnected = false;

			#if PHOTON_UNITY_NETWORKING
			this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
			#endif

			bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppId);

			if (!appIdPresent)
			{
				Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
			}
			else
			{
				// Add the chat for the current room to the join on connect list
				ChannelsToJoinOnConnect.Add(PhotonNetwork.CurrentRoom.Name);
				Connect();
			}
		}

		public void Connect()
		{
			this.chatClient = new ChatClient(this);
			#if !UNITY_WEBGL
			this.chatClient.UseBackgroundWorkerForSending = true;
			#endif
			this.chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);
			this.chatClient.ConnectUsingSettings(this.chatAppSettings);

			this.ChannelToggleToInstantiate.gameObject.SetActive(false);
			Debug.Log("Connecting as: " + PhotonNetwork.NickName);
		}

		/// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnDestroy.</summary>
		public void OnDestroy()
		{
			if (this.chatClient != null)
			{
				this.chatClient.Disconnect();
			}
		}

		/// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
		public void OnApplicationQuit()
		{
			if (this.chatClient != null)
			{
				this.chatClient.Disconnect();
			}
		}

		public void Update()
		{
			if (this.chatClient != null)
			{
				this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
			}

			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !chatEnabled)
			{
				ShowChat();
			}
		}

		public void OnEnterSend()
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				this.SendChatMessage(this.InputFieldChat.text);
				this.InputFieldChat.text = "";
				InputFieldChat.Select();
				InputFieldChat.ActivateInputField();
			}
		}

		public void OnClickSend()
		{
			if (this.InputFieldChat != null)
			{
				this.SendChatMessage(this.InputFieldChat.text);
				this.InputFieldChat.text = "";
				InputFieldChat.Select();
				InputFieldChat.ActivateInputField();
			}
		}


		public int TestLength = 2048;
		private byte[] testBytes = new byte[2048];

		private void SendChatMessage(string inputLine)
		{
			if (string.IsNullOrEmpty(inputLine))
			{
				return;
			}
			//TODO Delete. For testing private messages with one client
			/*if ("test".Equals(inputLine))
			{
				if (this.TestLength != this.testBytes.Length)
				{
					this.testBytes = new byte[this.TestLength];
				}

				this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
			}*/


			bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
			string privateChatTarget = string.Empty;
			if (doingPrivateChat)
			{
				// the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
				// so the remote ID is simple to figure out

				string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
				privateChatTarget = splitNames[1];
			}
			//UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);


			if (inputLine[0].Equals('/'))
			{
				string[] tokens = inputLine.Split(new char[] {' '}, 2);
				if (tokens[0].Equals("/help"))
				{
					this.PostHelpToCurrentChannel();
				}
				if ((tokens[0].Equals("/subscribe") || tokens[0].Equals("/s")) && !string.IsNullOrEmpty(tokens[1]))
				{
					this.chatClient.Subscribe(tokens[1].Split(new char[] {' ', ','}));
				}
				else if ((tokens[0].Equals("/unsubscribe") || tokens[0].Equals("/u")) && !string.IsNullOrEmpty(tokens[1]))
				{
					this.chatClient.Unsubscribe(tokens[1].Split(new char[] {' ', ','}));
				}
				else if (tokens[0].Equals("/clear"))
				{
					if (doingPrivateChat)
					{
						if (this.channelToggles.ContainsKey(selectedChannelName))
						{
							GameObject t = this.channelToggles[selectedChannelName];
							Destroy(t.gameObject);

							this.channelToggles.Remove(selectedChannelName);

							Debug.Log("Removed private channel toggle '" + selectedChannelName + "'.");
						}
						this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
						if (this.channelToggles.Count > 0)
						{
							List<String> keys = new List<String>(this.channelToggles.Keys);
							this.ShowChannel(keys[0]);
							this.selectedChannelName = keys[0];
						}
						else
						{
							this.CurrentChannelText.text = "";
						}
					}
					else
					{
						ChatChannel channel;
						if (this.chatClient.TryGetChannel(this.selectedChannelName, doingPrivateChat, out channel))
						{
							channel.ClearMessages();
							this.CurrentChannelText.text = "";
						}
					}
				}
				else if (tokens[0].Equals("/msg") && !string.IsNullOrEmpty(tokens[1]))
				{
					string[] subtokens = tokens[1].Split(new char[] {' ', ','}, 2);
					if (subtokens.Length < 2) return;

					string targetUser = subtokens[0];
					string message = subtokens[1];
					this.chatClient.SendPrivateMessage(targetUser, message);
				}
				else if ((tokens[0].Equals("/join") || tokens[0].Equals("/j")) && !string.IsNullOrEmpty(tokens[1]))
				{
					string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);

					// If we are already subscribed to the channel we directly switch to it, otherwise we subscribe to it first and then switch to it implicitly
					if (this.channelToggles.ContainsKey(subtokens[0]))
					{
						this.ShowChannel(subtokens[0]);
					}
					else
					{
						this.chatClient.Subscribe(new string[] { subtokens[0] });
					}
				}
				else
				{
					Debug.Log("The command '" + tokens[0] + "' is invalid.");
				}
			}
			else
			{
				if (doingPrivateChat)
				{
					this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
				}
				else
				{
					this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
				}
			}
		}

		public void PostHelpToCurrentChannel()
		{
			this.CurrentChannelText.text += HelpText;
		}

		public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
		{
			if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
			{
				Debug.LogError(message);
			}
			else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
			{
				Debug.LogWarning(message);
			}
			else
			{
				Debug.Log(message);
			}
		}

		public void OnConnected()
		{
			if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Count > 0)
			{
				this.chatClient.Subscribe(this.ChannelsToJoinOnConnect.ToArray(), this.HistoryLengthToFetch);
			}

			if (this.channelToggles.ContainsKey(PhotonNetwork.CurrentRoom.Name))
			{
				this.ShowChannel(PhotonNetwork.CurrentRoom.Name);
			}

			this.isChatConnected = true;

			this.UserIdText.text = PhotonNetwork.NickName;

			this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).

			this.OpenChatPrompt.enabled = true;

		}

		public void OnDisconnected()
		{
			//TODO May need this later
			//this.UnSubscribeCurrent();

			this.isChatConnected = false;

			this.OpenChatPrompt.enabled = false;
		}

		public void OnChatStateChange(ChatState state)
		{
			// use OnConnected() and OnDisconnected()
			// this method might become more useful in the future, when more complex states are being used.
		}

		public void OnSubscribed(string[] channels, bool[] results)
		{
			foreach (string channel in channels)
			{
				//? Welcome message
				//this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.

				if (this.ChannelToggleToInstantiate != null)
				{
					this.InstantiateChannelButton(channel);

				}
			}

			Debug.Log("OnSubscribed: " + string.Join(", ", channels));

			/*
			// select first subscribed channel in alphabetical order
			if (this.chatClient.PublicChannels.Count > 0)
			{
				var l = new List<string>(this.chatClient.PublicChannels.Keys);
				l.Sort();
				string selected = l[0];
				if (this.channelToggles.ContainsKey(selected))
				{
					ShowChannel(selected);
					foreach (var c in this.channelToggles)
					{
						c.Value.isOn = false;
					}
					this.channelToggles[selected].isOn = true;
					AddMessageToSelectedChannel(WelcomeText);
				}
			}
			*/

			// Switch to the first newly created channel
			this.ShowChannel(channels[0]);
		}

		/// <inheritdoc />
		public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
		}

		private void InstantiateChannelButton(string channelName)
		{
			if (this.channelToggles.ContainsKey(channelName))
			{
				Debug.Log("Skipping creation for an existing channel toggle.");
				return;
			}

			GameObject cbtn = Instantiate(this.ChannelToggleToInstantiate);
			cbtn.gameObject.SetActive(true);
			cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
			cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

			this.channelToggles.Add(channelName, cbtn);
		}

		public void OnUnsubscribed(string[] channels)
		{
			foreach (string channelName in channels)
			{
				if (this.channelToggles.ContainsKey(channelName))
				{
					GameObject t = this.channelToggles[channelName];
					Destroy(t.gameObject);

					this.channelToggles.Remove(channelName);

					Debug.Log("Unsubscribed from channel '" + channelName + "'.");

					// Showing another channel if the active channel is the one we unsubscribed from before
					if (channelName == this.selectedChannelName && this.channelToggles.Count > 0)
					{
						IEnumerator<KeyValuePair<string, GameObject>> firstEntry = this.channelToggles.GetEnumerator();
						firstEntry.MoveNext();

						this.ShowChannel(firstEntry.Current.Key);

						firstEntry.Current.Value.GetComponentInChildren<Toggle>().isOn = true;
					}
				}
				else
				{
					Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
				}
			}
		}

		public void OnGetMessages(string channelName, string[] senders, object[] messages)
		{
			if (channelName.Equals(this.selectedChannelName))
			{
				// update text
				this.ShowChannel(this.selectedChannelName);
			}
		}

		public void OnPrivateMessage(string sender, object message, string channelName)
		{
			// as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
			// you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
			this.InstantiateChannelButton(channelName);

			byte[] msgBytes = message as byte[];
			if (msgBytes != null)
			{
				Debug.Log("Message with byte[].Length: "+ msgBytes.Length);
			}
			if (this.selectedChannelName.Equals(channelName))
			{
				this.ShowChannel(channelName);
			}
		}

		/// <summary>
		/// New status of another user (you get updates for users set in your friends list).
		/// </summary>
		/// <param name="user">Name of the user.</param>
		/// <param name="status">New status of that user.</param>
		/// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a
		/// message (keep any you have).</param>
		/// <param name="message">Message that user set.</param>
		public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
		{
			Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));
		}

		public void OnUserSubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		public void OnUserUnsubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		/// <inheritdoc />
		public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, Extensions.ToStringFull(properties));
		}

		public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
		{
			Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, Extensions.ToStringFull(properties));
		}

		/// <inheritdoc />
		public void OnErrorInfo(string channel, string error, object data)
		{
			Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
		}

		public void AddMessageToSelectedChannel(string msg)
		{
			ChatChannel channel = null;
			bool found = this.chatClient.TryGetChannel(this.selectedChannelName, out channel);
			if (!found)
			{
				Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelName);
				return;
			}

			if (channel != null)
			{
				channel.Add("Bot", msg,0); //TODO: how to use msgID?
			}
		}



		public void ShowChannel(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				return;
			}

			ChatChannel channel = null;
			bool found = this.chatClient.TryGetChannel(channelName, out channel);
			if (!found)
			{
				Debug.Log("ShowChannel failed to find channel: " + channelName);
				return;
			}

			this.selectedChannelName = channelName;
			this.CurrentChannelText.text = channel.ToStringMessages();
			Debug.Log("ShowChannel: " + this.selectedChannelName);

			foreach (KeyValuePair<string, GameObject> pair in this.channelToggles)
			{
				pair.Value.GetComponentInChildren<Toggle>().isOn = pair.Key == channelName ? true : false;
			}
		}

		public void OpenDashboard()
		{
			Application.OpenURL("https://dashboard.photonengine.com");
		}

		public void ShowChat()
		{
			this.ChatPanel.gameObject.SetActive(true);
			this.OpenChatPrompt.enabled = false;
			chatEnabled = true;
			InputFieldChat.Select();
			InputFieldChat.ActivateInputField();
		}

		public void HideChat()
		{
			this.ChatPanel.gameObject.SetActive(false);
			this.OpenChatPrompt.enabled = true;
			chatEnabled = false;
		}

		/// <summary>
		/// Unsubscribes the user from the current room chat and current booth chat
		/// </summary>
		private void UnSubscribeCurrent()
		{
			List<String> currentChats = new List<string>();
			currentChats.Add(PhotonNetwork.CurrentRoom.Name);
			if (boothChannel != null)
			{
				currentChats.Add(boothChannel);
			}

			this.chatClient.Unsubscribe(currentChats.ToArray());
		}

		public void OnClickUnsubscribe(Text channelName)
		{
			if (channelName != null)
			{
				string[] currentScenes = { channelName.text };
				this.chatClient.Unsubscribe(currentScenes);
			}
			else
			{
				Debug.LogError("<Color=red><a>Missing</a></Color> Label Reference. Please set it up in GameObject Channel Toggle 'Close Button'");
			}
		}

		public static void FindBoothsForChat()
		{
            booths = GameObject.FindGameObjectsWithTag("Booth");
		}

		/// <summary>
		/// Subscribe to the booth channel that is closest and unsubscribe from the previous booth. Does lots of distance calculations use sparingly.
		/// </summary>
		/// <param name="playerPosition"></param>
		public void SubscirbeToClosestBooth(Vector3 playerPosition)
		{
			if (booths == null || booths[0] == null)
				return;

			String closestBoothName = null;
			float smallestDistance = boothChatRange * boothChatRange;
			foreach (GameObject booth in booths)
			{
				float distance = (booth.transform.position - playerPosition).sqrMagnitude;
				if (distance < smallestDistance)
				{
					closestBoothName = booth.name;
					smallestDistance = distance;
				}
			}

			//TODO change to booth title
			if (closestBoothName != boothChannel)
			{
				// Unsubscribe from current booth channel
				string[] channels = { boothChannel };
				if (channels[0] != null)
				{
					this.chatClient.Unsubscribe(channels);
				}

				// Subsribe to closest booth channel unless the user manually subscribed.
				// This also prevents the booth from being unsubscribed when the user manually subscribed to it.
				if (closestBoothName != null && !this.channelToggles.ContainsKey(closestBoothName))
				{
					boothChannel = closestBoothName;
					channels[0] = boothChannel;
					this.chatClient.Subscribe(channels);
				}
				else
				{
					boothChannel = null;
				}
			}
		}
	}
}