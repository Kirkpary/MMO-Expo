using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.Oregonstate.MMOExpo
{
    public class PortalTeleporterScript : MonoBehaviour
    {
        public Launcher launcher;

        void OnTriggerEnter (Collider other) 
        {
            if (SceneManager.GetActiveScene().name == "CS_Room")
            {
                launcher.Connect("ECE_Room");
            }
            else
            {
                launcher.Connect("CS_Room");
            }
        }
    }
}   
