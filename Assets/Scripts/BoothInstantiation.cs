using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    public class BoothInstantiation : MonoBehaviour 
    {
        [Tooltip("Prefab to instantiate.")]
        public GameObject BoothPrefab;
        [Tooltip("Gameobject from scene that is the parent of all spawnpionts.")]
        public GameObject SpawnpointListParent;
        [Tooltip("Collider for Object that the prefab needs to be leveled on.")]
        public Collider Ground;
        private string JsonPath;
        [HideInInspector]
        // Make json booth list available to other objects
        public static Booth[] BoothList
        {
            get
            {
                return _BoothList;
            }
        }
        private static Booth[] _BoothList;

        void Start()
        {
            // Reading the JSON file for this room
            if (SpawnpointListParent != null && BoothPrefab != null)
            {
                if (PhotonNetwork.InRoom)
                {
                    JsonPath = Application.streamingAssetsPath + "/" + PhotonNetwork.CurrentRoom.Name + ".json";
                    StartCoroutine(JsonHelper.JsonUrlToObject<Room>(JsonPath, true, InstantiateBooth));
                }
            }
            else
            {
                Debug.LogWarning("<Color=yellow><a>Missing</a></Color> myPrefab or SpawnpointListParent Reference. Please set it up in GameObject '" + this.name + "'", this);
            }
        }

        public void InstantiateBooth(Room roomObj)
        {
            _BoothList = roomObj.Items;
            // Get the transforms for the spawn points. List of spawn points including parent. perent is skipped later
            Transform[] spawnList = SpawnpointListParent.GetComponentsInChildren<Transform>();

            if (_BoothList.Length > spawnList.Length - 1)
            {
                Debug.LogWarning("Maximum number of booths exceeded. Add more spawn points. There are " + _BoothList.Length + " booths and " + (spawnList.Length - 1) + " booth spawn points.", this);
            }

            // Instantiate booths
            for (int i = 0; i < _BoothList.Length && i < spawnList.Length - 1; i++)
            {
                // Get position and rotation of spawn point
                Vector3 position = spawnList[i + 1].position;
                Quaternion rotation = spawnList[i + 1].transform.rotation;

                // Get slope of terrain
                if (Ground != null)
                {
                    // Offset raycast incase spawn point origin is slightly below the ground
                    int RaycastOffset = 1;
                    position.y += RaycastOffset;
                    RaycastHit hit;
                    Ray ray = new Ray(position, Vector3.down);
                    if (Ground.Raycast(ray, out hit, 1000))
                    {
                        // Move prefab center to the ground
                        position.y -= hit.distance;
                        // Level prefab with ground
                        rotation = Quaternion.FromToRotation(spawnList[i + 1].transform.up, hit.normal) * rotation;
                    }
                    else
                    {
                        position.y -= RaycastOffset;
                    }
                }
                else
                {
                    Debug.LogWarning("<Color=yellow><a>Missing</a></Color> Ground Collider Reference. Booths will not be auto leveled. Please set it up in GameObject '" + this.name + "'", this);
                }

                // Create booth
                GameObject temp = Instantiate(BoothPrefab, new Vector3(position.x, position.y, position.z), rotation);
                temp.name = _BoothList[i].BoothName;                
            }

            ChatGui.FindBoothsForChat();
        }
    }
}