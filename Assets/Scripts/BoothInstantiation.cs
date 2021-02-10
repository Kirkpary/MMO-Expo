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
        public GameObject myPrefab;
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
            if (SpawnpointListParent != null && myPrefab != null && Ground != null)
            {
                if (PhotonNetwork.InRoom)
                {
                    JsonPath = Application.streamingAssetsPath + "/" + PhotonNetwork.CurrentRoom.Name + ".json";
                    StartCoroutine(JsonHelper.JsonUrlToObject<Room>(JsonPath, true, InstantiateBooth));
                }
            }
            else
            {
                Debug.LogWarning("<Color=yellow><a>Missing</a></Color> myPrefab, SpawnpointListParent, or Terrain Reference. Please set it up in GameObject 'BoothInstantiation'", this);
            }
        }

        public void InstantiateBooth(Room roomObj)
        {
            _BoothList = roomObj.Items;
            GameObject[] temp;
            if (_BoothList.Length < spawnList.Length - 1) {
                temp = new GameObject[_BoothList.Length + 1];
                // Add Information Booth object
                temp[_BoothList.Length] = myPrefab;
            }
            else
            {
                temp = new GameObject[spawnList.Length];
                // Add Information Booth object
                temp[spawnList.Length - 1] = myPrefab;
            }

            Mesh prefabMesh = myPrefab.GetComponent<MeshFilter>().sharedMesh;
            Transform[] spawnList = SpawnpointListParent.GetComponentsInChildren<Transform>();  // List of spawn points including parent. perent is skipped later

            if (_BoothList.Length > spawnList.Length - 1)
            {
                Debug.LogWarning("Maximum number of booths exceeded. Add more spawn points. There are " + _BoothList.Length + " booths and " + (spawnList.Length - 1) + " booth spawn points.", this);
            }

            // Instantiate booths
            for (int i = 0; i < temp.Length - 1; i++)
            {
                // Get position and rotation of spawn point
                int RaycastOffset = 1;
                Vector3 position = spawnList[i + 1].position;
                position.y += RaycastOffset;
                Quaternion rotation = spawnList[i + 1].transform.rotation;

                // Get slope of terrain
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
                // Adjust y to account for height of mesh
                position.y += prefabMesh.bounds.extents.y - prefabMesh.bounds.center.y;

                // Create booth
                temp[i] = Instantiate(myPrefab, new Vector3(position.x, position.y, position.z), rotation);
                temp[i].name = _BoothList[i].BoothName;
            }

            ChatGui.FindBoothsForChat(ref temp);
        }
    }
}