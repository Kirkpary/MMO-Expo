using UnityEngine;
using System.Collections;
using System.IO;

namespace Com.Oregonstate.MMOExpo
{
    // Class object that has matching fields with the Json file nested array
    [System.Serializable]
    public class Booth
    {
        public string BoothName;
        public int CoordX;
        public int CoordY;
        public int CoordZ;
    }

    // Helper class to process Json arrays
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    public class BoothInstantiation : MonoBehaviour 
    {
        public GameObject myPrefab;
        private string JsonPath;
        private string JsonString;

        void Start()
        {
            // Reading the JSON file for this room
            JsonPath = Application.streamingAssetsPath + "/CS_Room.json";
            JsonString = File.ReadAllText(JsonPath);
            Booth[] CS_Room = JsonHelper.FromJson<Booth>(JsonString); // List of all booths

            // Instantiate booths
            for (int i=0; i<CS_Room.Length; i++) {
                Instantiate(myPrefab, new Vector3(CS_Room[i].CoordX, CS_Room[i].CoordY, CS_Room[i].CoordZ), Quaternion.identity);
            }

            ChatGui.FindBoothsForChat();
        }
    }
}