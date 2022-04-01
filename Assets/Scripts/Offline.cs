using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Offline : MonoBehaviour
{
    private void Awake()
    {
        // OFFLINE MODE
        PhotonNetwork.OfflineMode = true;
        RoomOptions options = new RoomOptions();
        PhotonNetwork.CreateRoom("Development", options);
    }
}
