using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
namespace Com.Oregonstate.MMOExpo
{
    public class BoothImageScript : MonoBehaviour
    {

        public GameObject BoothUI;
        public GameObject BoothImage;
        public GameObject Booth;
        public GameObject BoothPrompt;
        private static GameObject[] Booths = null;
        public bool display = false;
        private static GameObject agent;

        // Update is called once per frame
        void Update()
        {
            if (Booths != null && agent != null)
            {
                int i = 0;
                foreach (GameObject booth in Booths)
                {
                    float distance = (booth.transform.position - agent.transform.position).sqrMagnitude;
    /*                if (distance < 10)
                    {
                        BoothPrompt.SetActive(true);
                    }
                    else
                    {
                        BoothPrompt.SetActive(false);
                    } */
                    if (distance < 10 && Input.GetKeyDown("space") && display == false)
                    {
                        Debug.Log(booth.name);
                        string url = JsonHelper.BoothList[i].PictureURL;
                        StartCoroutine(GetTexture(url));
                        display = true;
                    //    BoothImage.SetActive(display);
                        LeanTween.moveLocalY(BoothImage, 0, 2);
                        break;
                    }
                    else if (Input.GetKeyDown("space") && display == true)
                    {
                        display = false;
                       // BoothImage.SetActive(display);
                        LeanTween.moveLocalY(BoothImage, -500, 2);
                        break;
                    }
                    i++;
                }
            }

        }

        private void Awake()
        {
            BoothUI = this.transform.GetChild(0).gameObject;
            BoothPrompt = this.transform.GetChild(1).gameObject;
            BoothImage = BoothUI.gameObject;
            BoothPrompt.SetActive(false);
            LeanTween.moveLocalY(BoothImage, -500, 0);
        }


        public void OpenBooth()
        {
            if (BoothUI == null)
                Debug.Log("NULL");
            BoothImage.SetActive(true);
            Debug.Log("Clicked");
        }

        IEnumerator GetTexture(string url)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            BoothUI.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

        public static void FindBooths()
        {
            Booths = GameObject.FindGameObjectsWithTag("Booth");
            foreach (GameObject booth in Booths)
                Debug.Log(booth.name);
        }

        public static void FindPlayer()
        {
            agent = GameObject.FindGameObjectWithTag("Player");
        }

    }
}
