using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Oregonstate.MMOExpo
{
    public class PortalInstantiation : MonoBehaviour
    {
        public Launcher launcher;
        public GameObject buttonToInstantiate;

        private string JsonPath;

        void Start()
        {
            JsonPath = Application.streamingAssetsPath + "/RoomList.json";
            StartCoroutine(JsonHelper.JsonUrlToObject<RoomList>(JsonPath, false, InstantiateObjects));
        }

        void InstantiateObjects(RoomList roomList)
        {
            if (roomList != null)
            {
                foreach (string roomName in roomList.RoomNames)
                {
                    // Create "portal" buttons - may be changed to the 3d portals later
                    GameObject button = Instantiate(buttonToInstantiate);
                    button.GetComponentInChildren<Text>().text = roomName;
                    button.GetComponent<Button>().onClick.AddListener(delegate { launcher.Connect(roomName); });
                    button.transform.SetParent(buttonToInstantiate.transform.parent);
                    button.SetActive(true);
                }
            }
        }
    }
}
