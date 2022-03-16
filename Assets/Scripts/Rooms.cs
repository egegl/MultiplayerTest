using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Rooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput;
    public TMP_InputField joinRoomInput;
    public TMP_InputField usernameInput;

    public void CreateRoom()
    {
        CheckConnection();
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }

    public void JoinRoom()
    {
        CheckConnection();
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        if (usernameInput.text != "")
        {
            PhotonNetwork.NickName = usernameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        }
    }

    public void CheckConnection()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneLoader.instance.Load("Loading");
            return;
        }
    }
}
