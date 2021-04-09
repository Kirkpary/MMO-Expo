using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.Oregonstate.MMOExpo
{
    public class PortalTeleporterScript : MonoBehaviour
    {
        public Launcher launcher;
<<<<<<< HEAD
        public GameObject PortalPrefab;

        void OnTriggerEnter (Collider other) 
        {
            launcher.Connect(PortalPrefab.name);
=======
        public string RoomName = "";
        private string DefaultWorld = "WorldLobby";

        void OnTriggerEnter (Collider other) 
        {
            if (RoomName != "")
            {
                launcher.Connect(RoomName);
            }
            else
            {
                Debug.Log("No RoomName specified. Defaulting to " + DefaultWorld + ".", this);
                launcher.Connect(DefaultWorld);
            }
>>>>>>> origin/main
        }
    }
}   
