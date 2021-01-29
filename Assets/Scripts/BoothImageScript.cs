using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoothImageScript : MonoBehaviour
{

    public GameObject BoothUI;
    public GameObject BoothHover;
    // Start is called before the first frame update
    void Start()
    {
        BoothUI.SetActive(false);
        BoothHover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
       if(Input.GetMouseButtonDown(0) & BoothUI.active == false)
        {
            BoothUI.SetActive(true);
        }

       else
        {
            BoothUI.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (BoothHover.active == false)
        {
            BoothHover.SetActive(true);
            Debug.Log("Mouse is over GameObject.");
        }
    }

    private void OnMouseExit()
    {
        if (BoothHover.active == true)
        {
            BoothHover.SetActive(false);
        }
    } 
}
