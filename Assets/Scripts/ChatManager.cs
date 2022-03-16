using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Text.RegularExpressions;

public class ChatManager : MonoBehaviourPunCallbacks
{
    private float _delay = 0f;
    private List<string> _messages = new List<string>();
    public InputField chatInput;
    public TextMeshProUGUI chatContent;


    [PunRPC]
    void RPC_AddMessage(string msg)
    {
        _messages.Add(msg);
    }

    public void SendChat(string msg)
    {
        string NewMessage = PhotonNetwork.NickName + ": " + msg;
        photonView.RPC("RPC_AddMessage", RpcTarget.All, NewMessage);
    }

    public void SendChat()
    {
        string blankCheck = chatInput.text;
        blankCheck = Regex.Replace(blankCheck, @"\s", "");
        if (blankCheck == "")
        {
            chatInput.text = "";
            return;
        }
        SendChat(chatInput.text);
        chatInput.text = "";
    }
    
    public void BuildChat()
    {
        string NewContent = "";
        foreach (string msg in _messages)
        {
            NewContent += msg + "\n";
        }
        chatContent.text = NewContent;
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            chatContent.maxVisibleLines = 8;

            if (_messages.Count > 8)
            {
                _messages.RemoveAt(0);
            }
            if (_delay < Time.time)
            {
                BuildChat();
                _delay = Time.time + 0.25f;
            }
            
        }
        else if (_messages.Count > 0)
        {
            _messages.Clear();
            chatContent.text = "";
        }

        if (Input.GetKey(KeyCode.Return))
        {
            SendChat();
        }
    }
}
