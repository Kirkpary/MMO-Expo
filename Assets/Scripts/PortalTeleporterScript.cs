using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Oregonstate.MMOExpo
{
    public class PortalTeleporterScript : MonoBehaviour
    {

        void OnTriggerEnter (Collider other) 
        {
            if (SceneManager.GetActiveScene().name == "CS_Room")
            {
                Application.LoadLevel("ECE_Room");
            }
            else
            {
                Application.LoadLevel("CS_Room");
            }
        }
    }
}   
