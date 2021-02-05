using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchScript : MonoBehaviour
{
    public GameObject popUpPanel;
    private List<GameObject> boothList = new List<GameObject>();
    private int firstTimeOpen = 0;
 
    // Start is called before the first frame update
    void Start()
    {
        popUpPanel.SetActive(false);
        
        // Get the booth template
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSearchContainer()
    {
        // Add all the booths into a list if it is the first time
        // That the user opens the search
        if (firstTimeOpen == 0)
        {
            var booths = GameObject.FindGameObjectsWithTag("Booth");
            foreach (GameObject booth in booths)
            {
                boothList.Add(booth);
            }
            firstTimeOpen++;
        }

        Debug.Log("Button");
        popUpPanel.SetActive(true);
        Debug.Log("The number of booths is: " + boothList.Count);
    }

    public void CloseSearchContainer()
    {
        popUpPanel.SetActive(false);
    }

    void displayList(List<GameObject> boothList)
    {

    }
}
