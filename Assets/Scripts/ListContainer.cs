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

        for (int i = 0; i < 5; i++)
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
