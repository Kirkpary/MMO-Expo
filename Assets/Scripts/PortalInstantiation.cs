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

        private string JsonPath;

        void Start()
        {
            JsonPath = Application.streamingAssetsPath + "/RoomList.json";
            StartCoroutine(JsonHelper.JsonUrlToObject<RoomList>(JsonPath, false, InstantiateObjects));

            PortalTeleporterScript.passLauncher(launcher);
        }

        void InstantiateObjects(RoomList roomList)
        {
            if (roomList != null)
            {
                int x = 0, y = 2, z = 5; // might change to something more elegant in the future
                foreach (string roomName in roomList.RoomNames)
                {
                    GameObject temp = Instantiate(PortalPrefab, new Vector3(x, y, z), Quaternion.identity);
                    temp.name = roomName;

                    x += 6;
                }
            }
        }
    }
}
