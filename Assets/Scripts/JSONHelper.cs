using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.Oregonstate.MMOExpo
{
    [Serializable]
    public class Room
    {
        public string SceneName;
        public Booth[] Items;
    }

    // Class object that has matching fields with the Json file nested array
    [Serializable]
    public class Booth
    {
        public string BoothName;
    }

    // Helper class to process Json arrays
    public static class JsonHelper
    {
        public static IEnumerator JsonUrlToObject<T>(string JsonPath, Action<T> callback)
        {
            UnityWebRequest www = new UnityWebRequest(JsonPath);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string JsonString = www.downloadHandler.text; // Show results as text
                callback(JsonUtility.FromJson<T>(JsonString)); // List of all booths
            }
        }
    }
}
