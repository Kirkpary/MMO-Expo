using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Com.Oregonstate.MMOExpo;

public class ListContainer : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        GameObject boothTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        Debug.Log("The name of boothTemplate is: " + boothTemplate.name);
        Debug.Log("The name of this.transform is: " + this.transform.name);

        for (int i = 0; i < BoothInstantiation.BoothList.Length; i++)
        {
            g = Instantiate(boothTemplate, this.transform);
            g.transform.GetChild(0).GetComponent<Text>().text = BoothInstantiation.BoothList[i].BoothName;
            g.transform.GetChild(1).GetComponent<Text>().text = BoothInstantiation.BoothList[i].Description;
        }
        Destroy(boothTemplate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
