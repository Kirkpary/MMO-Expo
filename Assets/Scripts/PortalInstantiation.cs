using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.IO;
using UnityEngine.Networking;
using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    public class PortalInstantiation : MonoBehaviour
    {
        public Launcher launcher;
        public GameObject PortalPrefab;
        // public GameObject buttonToInstantiate;

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
                int x = 0, y = 2, z = 0;
                foreach (string roomName in roomList.RoomNames)
                {
                    x += 6;
                    // Create "portal" buttons - may be changed to the 3d portals later
                    // GameObject button = Instantiate(buttonToInstantiate);
                    //button.GetComponentInChildren<Text>().text = roomName;
                    //button.GetComponent<Button>().onClick.AddListener(delegate { launcher.Connect(roomName); });
                   // button.transform.SetParent(buttonToInstantiate.transform.parent);
                   // button.SetActive(true); 
                    Debug.Log(roomName);
                    //GameObject temp = Instantiate(PortalPrefab, new Vector3(x, y, z), 0);
                    GameObject temp = Instantiate(PortalPrefab, new Vector3(x, y, z), Quaternion.identity);
                    temp.name = roomName;
                }
            }
        }
    }
}
