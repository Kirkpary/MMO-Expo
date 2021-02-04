using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    // Class object that has matching fields with the Json file nested array
    [System.Serializable]
    public class Booth
    {
        public string BoothName;
    }

    public class BoothInstantiation : MonoBehaviour 
    {
        [Tooltip("Prefab to instantiate.")]
        public GameObject myPrefab;
        [Tooltip("Gameobject from scene that is the parent of all spawnpionts.")]
        public GameObject SpawnpointListParent;
        [Tooltip("Collider for Object that the prefab needs to be leveled on.")]
        public Collider Ground;
        private string JsonPath;
        private string JsonString;

        void Start()
        {
            // Reading the JSON file for this room
            if (SpawnpointListParent != null && myPrefab != null && Ground != null)
            {
                if (PhotonNetwork.InRoom)
                {
                    JsonPath = Application.streamingAssetsPath + "/" + PhotonNetwork.CurrentRoom.Name + ".json";
                    StartCoroutine(InstantiateBooth(JsonPath));
                }
                else
                {
                    JsonPath = Application.streamingAssetsPath + "/test.json";
                    StartCoroutine(InstantiateBooth(JsonPath));
                }
            }
            else
            {
                Debug.LogWarning("<Color=yellow><a>Missing</a></Color> myPrefab, SpawnpointListParent, or Terrain Reference. Please set it up in GameObject 'BoothInstantiation'", this);
            }
        }

        IEnumerator InstantiateBooth(string JsonPath) {
            UnityWebRequest www = new UnityWebRequest(JsonPath);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
    
            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                string JsonString = www.downloadHandler.text; // Show results as text
                Booth[] Booth_List = JsonHelper.FromJson<Booth>(JsonString); // List of all booths

                Mesh prefabMesh = myPrefab.GetComponent<MeshFilter>().sharedMesh;
                Transform[] spawnList = SpawnpointListParent.GetComponentsInChildren<Transform>();  // List of spawn points including parent. perent is skipped later

                if (Booth_List.Length > spawnList.Length-1)
                {
                    Debug.LogWarning("Maximum number of booths exceeded. Add more spawn points. There are " + Booth_List.Length + " booths and " + (spawnList.Length-1) + " booth spawn points.", this);
                }

                // Instantiate booths
                for (int i = 0; i < Booth_List.Length && i < spawnList.Length-1; i++)
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
                    GameObject temp = Instantiate(myPrefab, new Vector3(position.x, position.y, position.z), rotation);
                    temp.name = Booth_List[i].BoothName;
                }

                ChatGui.FindBoothsForChat();
            }
        }
    }
}