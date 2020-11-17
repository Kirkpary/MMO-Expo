using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WorldSelector : MonoBehaviourPunCallbacks
{

    public GameObject CS_Button = null;
    public GameObject ECE_Button = null;
    public GameObject CS_ECE_Button = null;


    // Start is called before the first frame update
    private void Start()
    {
        CS_Button.SetActive(true);
        ECE_Button.SetActive(true);
        CS_ECE_Button.SetActive(true);
    }


    public void CS_Button_Click()
    {
        PhotonNetwork.LoadLevel("CS_Room");
    }


    public void ECE_Button_Click()
    {
        PhotonNetwork.LoadLevel("ECE_Room");
    }


    public void CS_ECE_Button_Click()
    {
        PhotonNetwork.LoadLevel("CS_ECE_Room");
    }
}
