using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Com.Oregonstate.MMOExpo;

public static class BoothButtonExtension
{
    public static void AddEventListener<T>(this Button button, T p, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(p);
        });
    }
}

public class ListContainer : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        //JsonHelper.GetBoothPicture();
        Debug.Log("Drawing search UI");
        //DrawUI();
        Invoke("DrawUI", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawUI()
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
            g.transform.GetChild(2).GetComponent<Image>().sprite = BoothInstantiation.BoothList[i].Picture;

            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
        Destroy(boothTemplate);
    }

    void ItemClicked(int itemIndex)
    {
        Debug.Log("Teleporting player model to the booth: " + BoothInstantiation.BoothList[itemIndex].BoothName);

        GameObject destinationBooth = GameObject.Find(BoothInstantiation.BoothList[itemIndex].BoothName);
        GameManager.PlayerModel.transform.position = destinationBooth.transform.position + destinationBooth.transform.forward * (-4);
    }
}
