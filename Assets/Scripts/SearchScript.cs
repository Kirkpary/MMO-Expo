using Com.Oregonstate.MMOExpo;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SearchScript : MonoBehaviour
{
    public GameObject popUpPanel;
    public InputField searchBoothInputField;
    public GameObject scrollViewContent;
    
    public Transform searchScrollViewContent;
    public static GameObject boothTemplate;

    private List<GameObject> boothList = new List<GameObject>();

    private static bool searchPanelEnabled = false;
    public static bool isSearchPanelEnabled { 
        get { return searchPanelEnabled; } 
    }

    private bool firstTimeOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        searchScrollViewContent = GameObject.FindGameObjectWithTag("ScrollViewContent").transform;
        boothTemplate = GameObject.Find("BoothTemplate");
        

        popUpPanel.SetActive(false);
        searchPanelEnabled = false;

        //Transform[] booths = scrollViewContent.GetComponentsInChildren<Transform>();
        //foreach (Transform booth in booths)
        //{
        //    if (booth.gameObject.name == "BoothTemplate(Clone)")
        //    {
        //        boothList.Add(booth.gameObject);
        //        Debug.Log("Adding booth to boothList wtih name: " + booth.gameObject.name);
        //    }
        //}

        // Initialize input field
        searchBoothInputField.onValueChanged.AddListener(delegate { SearchPanelOnValueChanged(); });
    }

    public void OpenSearchContainer()
    {
        if (firstTimeOpen)
        {
            boothTemplate.SetActive(true);
            Transform[] booths = scrollViewContent.GetComponentsInChildren<Transform>();
            foreach (Transform booth in booths)
            {
                if (booth.gameObject.name == "BoothTemplate(Clone)")
                {
                    boothList.Add(booth.gameObject);
                    Debug.Log("Adding booth to boothList wtih name: " + booth.gameObject.name);
                }
            }
            firstTimeOpen = false;
            StartCoroutine(BoothPictureHelper.GetBoothPicture(searchScrollViewContent, boothTemplate));
        }

        Debug.Log("Search button clicked");
        searchPanelEnabled = true;
        popUpPanel.SetActive(true);
        Debug.Log("The size of boothList is: " + boothList.Count);
    }

    public void CloseSearchContainer()
    {
        popUpPanel.SetActive(false);
        searchPanelEnabled = false;

    }

    public void SearchPanelOnValueChanged()
    {
        // All the text search is performed in lower case

        Debug.Log("SeachPanel text input value changed: " + searchBoothInputField.text);
        string searchText = searchBoothInputField.text.ToLower();

        //
        // GameObject boothNameText = boothList[0].transform.GetChild(0).gameObject;
        // Text boothNameText = boothList[0].GetComponentInChildren<Text>();
        // Debug.Log("The name of boothNameText is: " + boothNameText.text);


        // Restart the filter list
        for (int i = 0; i < boothList.Count; i++)
        {
            boothList[i].SetActive(true);
        }


        // Check that the input field string is not empty
        if (!String.IsNullOrEmpty(searchText))
        {
            // Do booth search filtering
            Debug.Log("Doing booth search with the following string: " + searchText);
            for (int i = 0; i < boothList.Count; i++)
            {
                // Get the text string value from the Text object name of the booth
                Text boothNameText = boothList[i].GetComponentInChildren<Text>();
                string boothName = boothNameText.text.ToLower();

                if (!boothName.Contains(searchText))
                {
                    boothList[i].SetActive(false);
                }
            }
        }
        else
        {
            // Do nothing
            Debug.Log("SearchBoothInputField is empty");
        }
    }

}
