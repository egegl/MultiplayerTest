using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Text.RegularExpressions;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    private float _delay = 0f;
    private List<string> _messages = new List<string>();
    public InputField chatInput;
    public TextMeshProUGUI chatContent;

    private void Start()
    {
        chatInput.characterLimit = 80;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(PlayerJoinedRoom(newPlayer));
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        photonView.RPC("RPC_AddMessage", RpcTarget.All, player.NickName + " left");
    }

    private IEnumerator PlayerJoinedRoom(Player newPlayer)
    {
        yield return new WaitForSeconds(0.5f);
        if (newPlayer.NickName != "")
        {
            photonView.RPC("RPC_AddMessage", RpcTarget.All, newPlayer.NickName + " joined");
        }
    }

    [PunRPC]
    void RPC_AddMessage(string msg)
    {
        _messages.Add(msg);
        AudioManager.instance.Play("notification");
    }

    public void SendChat2(string msg)
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
        SendChat2(chatInput.text);
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

        if (Input.GetKeyDown(KeyCode.Return) && chatInput.text != "")
        {
            SendChat();
            DeselectChatInput();
            
        }
        else if(Input.GetKeyDown(KeyCode.Return) && chatInput.text == "")
        {
            if (!chatInput.isFocused)
            {
                SelectChatInput();
            }
            else
            {
                DeselectChatInput();
            }
        }
    }

    private void SelectChatInput()
    {
        chatInput.Select();
    }

    private void DeselectChatInput()
    {
        chatInput.DeactivateInputField();
    }
}
