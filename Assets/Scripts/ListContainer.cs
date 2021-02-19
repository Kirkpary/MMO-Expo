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
        //Debug.Log("Drawing search UI");
        //DrawUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void DrawUI(Transform parent, GameObject boothTemplate)
    {
        //GameObject boothTemplate = transform.GetChild(0).gameObject;
        //GameObject boothTemplate = GameObject.Find("BoothTemplate");
        //Debug.Log("The name of the parent of the boothTemplate is: " + boothTemplate.transform.parent.gameObject.name);
        GameObject g;

        //Debug.Log("The name of boothTemplate is: " + boothTemplate.name);
        //Debug.Log("The name of this.transform is: " + this.transform.name);

        for (int i = 0; i < JsonHelper.BoothList.Length; i++)
        {
            g = Instantiate(boothTemplate, parent);
            g.transform.GetChild(0).GetComponent<Text>().text = JsonHelper.BoothList[i].BoothName;
            g.transform.GetChild(1).GetComponent<Text>().text = JsonHelper.BoothList[i].Description;
            g.transform.GetChild(2).GetComponent<Image>().sprite = JsonHelper.BoothList[i].Picture;

            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
        SearchScript.boothTemplate.SetActive(false);
    }

    static void ItemClicked(int itemIndex)
    {
        Debug.Log("Teleporting player model to the booth: " + JsonHelper.BoothList[itemIndex].BoothName);

        GameObject destinationBooth = GameObject.Find(JsonHelper.BoothList[itemIndex].BoothName);
        GameManager.PlayerModel.transform.position = destinationBooth.transform.position + destinationBooth.transform.forward * (-4);
    }
}
