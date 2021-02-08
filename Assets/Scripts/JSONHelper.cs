using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.Oregonstate.MMOExpo
{
    #region RoomJson
    [Serializable]
    public class Room
    {
        public string SceneName;
        public Booth[] Items;
    }

    [Serializable]
    public class Booth
    {
        public string BoothName;
    }
    #endregion

    #region RoomListJson
    [Serializable]
    public class RoomList
    {
        public string[] RoomNames;
    }
    #endregion

    /// <summary>
    /// Helper class to process Json arrays
    /// </summary>
   
    public static class JsonHelper
    {
        /// <summary>
        /// Function to read/download json with passed in path.
        /// </summary>
        /// <typeparam name="T">Pass in root JSON class object.</typeparam>
        /// <param name="JsonPath">Path to JSON file in project/website.</param>
        /// <param name="checkCache">If true check if the file exists on the local machine and read the file if it exists.</param>
        /// <param name="callback">Callback function to call when the local file is read or the json finishes downloading.</param>
        /// <example>
        /// This shows how to call the <see cref="JsonUrlToObject"/> method.
        /// <code>
        /// class TestClass
        /// {
        ///     void Start() {
        ///         string JsonPath = Application.streamingAssetsPath + "/test.json";
        ///         StartCoroutine(JsonHelper.JsonUrlToObject<Room>(JsonPath, true, TestCallback));
        ///     }
        ///     
        ///     void TestCallback(TestJsonClass test) {
        ///         // Do stuff with json data
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IEnumerator JsonUrlToObject<T>(string JsonPath, bool checkCache, Action<T> callback)
        {
            if (checkCache && File.Exists(JsonPath))
            {
                Debug.Log("Json file already exists. Using existing file.");
                StreamReader reader = new StreamReader(JsonPath);
                string JsonString = reader.ReadToEnd();
                reader.Close();
                callback(JsonUtility.FromJson<T>(JsonString));
            }
            else
            {
                Debug.Log("Json file missing. Downloading from server.");
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
}
