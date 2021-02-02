using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchScript : MonoBehaviour
{
    public GameObject popUpContainer;
 
    // Start is called before the first frame update
    void Start()
    {
        popUpContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSearchContainer()
    {
        Debug.Log("Button");
        popUpContainer.SetActive(true);
    }

    public void CloseSearchContainer()
    {
        popUpContainer.SetActive(false);
    }
}
