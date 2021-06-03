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
        public GameObject BoothButton;
        public GameObject Booth;
        public GameObject BoothPrompt; //Prompt to tell the user the controls for the UI
        private static GameObject[] Booths = null; //gameobject array of all booths
        public bool display = false; //Bool to turn off and on the UI
        private static GameObject agent; //Gameobject for the player character
        public string ZoomURL; //Holds the zoom link

        // Update is called once per frame
        void Update()
        {
            //Check to make sure booths and player have loaded in
            if (Booths != null && agent != null)
            {
                int i = 0;
                //Loops over all booths and checks distance from player to that booth
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
                        string zoomURL = JsonHelper.BoothList[i].ZoomLink;
                        StartCoroutine(GetTexture(url));
                        display = true;
                        LeanTween.moveLocalY(BoothImage, 0, 2);
                        LeanTween.moveLocalY(BoothButton, -185, 2);
                        break;
                    }
                    else if (Input.GetKeyDown("space") && display == true)
                    {
                        display = false;
                        LeanTween.moveLocalY(BoothImage, -500, 2);
                        LeanTween.moveLocalY(BoothButton, -500, 2);
                        break;
                    }
                    i++;
                }
            }

        }

        private void Awake()
        {
            BoothUI = this.transform.GetChild(0).gameObject; //Get the booth UI image from the parent
            BoothPrompt = this.transform.GetChild(1).gameObject; //Get the booth prompt from the parent
            BoothButton = this.transform.GetChild(2).gameObject;
            BoothImage = BoothUI.gameObject; //Get the canvas parent element as a game object
            BoothPrompt.SetActive(false); //disable the booth prompt
            LeanTween.moveLocalY(BoothImage, -500, 0); //Moves the booth image offscreeen
            LeanTween.moveLocalY(BoothButton, -500, 0); //Moves the booth button offscreeen
        }

        //Loads in texture from the given website that is pulled from the json file
        IEnumerator GetTexture(string url)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            BoothUI.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

        public static void BoothButtonClicked()
        {
            Application.OpenURL("https://google.com/");
        }

        //Gets all booths in a scene, called in BoothInstantiation.cs
        public static void FindBooths()
        {
            Booths = GameObject.FindGameObjectsWithTag("Booth");
            foreach (GameObject booth in Booths)
                Debug.Log(booth.name);
        }

        //Gets a reference to the player character, called in PlayerManager.cs
        public static void FindPlayer()
        {
            agent = GameObject.FindGameObjectWithTag("Player");
        }

    }
}
