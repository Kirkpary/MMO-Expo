using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Oregonstate.MMOExpo
{
    public class SpawnPoint : MonoBehaviour
    {
        public MeshFilter PrefabMesh;
        public Transform PrefabTransform;

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            if (PrefabMesh != null && PrefabTransform != null)
            {
                position.y += PrefabMesh.sharedMesh.bounds.extents.y;
                Gizmos.DrawWireMesh(PrefabMesh.sharedMesh, -1, position, Quaternion.identity, PrefabTransform.localScale);
            }
        }
    }
}
