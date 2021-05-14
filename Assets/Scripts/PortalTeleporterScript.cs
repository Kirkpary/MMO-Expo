using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.Oregonstate.MMOExpo
{
    public class PortalTeleporterScript : MonoBehaviour
    {
        public GameObject PortalPrefab;
        private string DefaultWorld = "PortalLobby";
        private static Launcher launcher;

        public static void passLauncher(Launcher roomLauncher) 
        {
            launcher = roomLauncher;
        }

        void OnTriggerEnter (Collider other) 
        {
            if (launcher != null)
            {
                if (PortalPrefab.name != "")
                {
                    launcher.Connect(PortalPrefab.name);
                }
                else
                {
                    Debug.Log("No RoomName specified. Defaulting to " + DefaultWorld + ".", this);
                    launcher.Connect(DefaultWorld);
                }
            }
        }
    }
}   
