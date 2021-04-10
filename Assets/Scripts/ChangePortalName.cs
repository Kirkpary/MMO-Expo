using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Com.Oregonstate.MMOExpo
{
    public class ChangePortalName : MonoBehaviour
    {
        public GameObject PortalPrefab;

        void Start()
        {
            TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
            textmeshPro.SetText(PortalPrefab.name);
        }
    }
}