using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        public string Description;
        public Sprite Picture;
        public string PictureURL;
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
        [HideInInspector]
        // Make json booth list available to other objects
        public static Booth[] BoothList;

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

    public static class BoothPictureHelper
    {
        public static IEnumerator GetBoothPicture(Transform parent, GameObject boothTemplate)
        {
            Debug.Log("Getting pictures from the JSON file");
            for (int i = 0; i < JsonHelper.BoothList.Length; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(JsonHelper.BoothList[i].PictureURL);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Setting picture of booth i: " + i);
                    Texture2D tx = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                    Sprite newSprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(tx.width / 2, tx.height / 2));
                    JsonHelper.BoothList[i].Picture = newSprite;
                }
            }
            // ListContainer.DrawUI();
            ListContainer.DrawUI(parent, boothTemplate);
        }
    }
}
