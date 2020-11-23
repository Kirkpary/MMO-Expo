using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace Com.Oregonstate.MMOExpo
{
    [RequireComponent(typeof(PlayerManager))]
    public class EnablePlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            GetComponent<PlayerManager>().enabled = true;
            GetComponent<CameraWork>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
