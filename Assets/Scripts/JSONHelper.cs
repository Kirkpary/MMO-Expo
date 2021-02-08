using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.Oregonstate.MMOExpo
{
    // Helper class to process Json arrays
    public static class JsonHelper
    {
        public static IEnumerator JsonUrlToObject<T>(string JsonPath, Action<T> callback)
        {
            Debug.Log("Json download started.");
            UnityWebRequest www = new UnityWebRequest(JsonPath);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Json download succeded.");
                string JsonString = www.downloadHandler.text; // Show results as text
                callback(JsonUtility.FromJson<T>(JsonString)); // List of all booths
            }
        }
    }
}
