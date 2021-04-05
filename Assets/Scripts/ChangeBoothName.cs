using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Com.Oregonstate.MMOExpo
{
    public class ChangeBoothName : MonoBehaviour
    {
        public GameObject BoothPrefab;

        void Start()
        {
            TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
            textmeshPro.SetText(BoothPrefab.name);
        }
    }
}