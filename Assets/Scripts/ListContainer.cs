using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListContainer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject boothTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        Debug.Log("The name of boothTemplate is: " + boothTemplate.name);
        Debug.Log("The name of this.transform is: " + this.transform.name);

        for (int i = 0; i < 10; i++)
        {
            g = Instantiate(boothTemplate, this.transform);
        }
        Destroy(boothTemplate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
