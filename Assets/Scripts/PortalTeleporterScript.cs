using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.Oregonstate.MMOExpo
{
    public class PortalTeleporterScript : MonoBehaviour
    {
        public Launcher launcher;
        public GameObject PortalPrefab;

        void OnTriggerEnter (Collider other) 
        {
            launcher.Connect(PortalPrefab.name);
        }
    }
}   
