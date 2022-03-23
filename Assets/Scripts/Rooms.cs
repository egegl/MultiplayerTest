using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Rooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput;
    public TMP_InputField joinRoomInput;
    public TMP_InputField usernameInput;

    private void Start()
    {
        usernameInput.characterLimit = 10;
        createRoomInput.characterLimit = 10;
    }

    public void CreateRoom()
    {
        CheckConnection();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(createRoomInput.text, options);
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
