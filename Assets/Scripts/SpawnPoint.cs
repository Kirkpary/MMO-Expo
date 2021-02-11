using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Oregonstate.MMOExpo
{
    public class SpawnPoint : MonoBehaviour
    {
        [Tooltip("Mesh filters are gathered from all the children and rendered as a gizmo wireframe.")]
        public GameObject PrefabMesh;

        private void OnDrawGizmos()
        {
            if (PrefabMesh != null)
            {
                MeshFilter[] meshes = PrefabMesh.GetComponentsInChildren<MeshFilter>();

                for (int i = 0; i < meshes.Length; i++)
                {
                    // Change gizmo to local coordinates so the whole wirefram rotates with the spawn point
                    Gizmos.matrix = this.transform.localToWorldMatrix;
                    Transform t = meshes[i].gameObject.GetComponent<Transform>();
                    Gizmos.DrawWireMesh(meshes[i].sharedMesh, -1, t.position, t.rotation, t.localScale);
                }
            }
            else
            {
                Debug.LogWarning("<Color=yellow><a>Missing</a></Color> PrefabMesh Reference. Please set it up in GameObject '" + this.name + "'", this);
            }
        }
    }
}
