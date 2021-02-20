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

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Function to populate the scroll view list that is inside the search panel.
    /// It will take the information available from the JSONHelper BoothList.
    /// </summary>
    public static void DrawUI(Transform scrollViewContent, GameObject boothTemplate)
    {
        GameObject g;

        for (int i = 0; i < JsonHelper.BoothList.Length; i++)
        {
            g = Instantiate(boothTemplate, scrollViewContent);
            g.transform.GetChild(0).GetComponent<Text>().text = JsonHelper.BoothList[i].BoothName;
            g.transform.GetChild(1).GetComponent<Text>().text = JsonHelper.BoothList[i].Description;

            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
        Destroy(boothTemplate);
    }

    /// <summary>
    /// Takes the Sprite Picture stored in the JSONHelper Boothlist
    /// to populate the icon that is in each element of the scroll view list
    /// in the search panel.
    /// </summary>
    public static void DrawUIPicture()
    {
        GameObject g;
        for (int i = 0; i < JsonHelper.BoothList.Length; i++)
        {
            g = SearchScript.boothList[i];
            if (g != null)
            {
                g.transform.GetChild(2).GetComponent<Image>().sprite = JsonHelper.BoothList[i].Picture;
            }
        }
    }

    /// <summary>
    /// Teleports the player to the correct booth.
    /// </summary>
    static void ItemClicked(int itemIndex)
    {
        Debug.Log("Teleporting player model to the booth: " + JsonHelper.BoothList[itemIndex].BoothName);

        GameObject destinationBooth = GameObject.Find(JsonHelper.BoothList[itemIndex].BoothName);
        GameManager.PlayerModel.transform.position = destinationBooth.transform.position + destinationBooth.transform.forward * (-4);
    }
}
