using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Scoreboard : MonoBehaviourPunCallbacks 
{
    public TextMeshProUGUI text;
    [HideInInspector] public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public override void OnEnable()
    {
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            int a = 1;
            text.text = a + ") " + player.NickName.ToString() + "  :  " + player.CustomProperties["Kills"] + " K / " + player.CustomProperties["Deaths"] + " D" + "\n";
            a++;
        }
    }

    public override void OnDisable()
    {
        text.text = "";
    }
}
