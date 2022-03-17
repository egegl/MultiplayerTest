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
    public static Scoreboard Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        UpdateScores();
    }

    public override void OnDisable()
    {
        text.text = "";
    }

    public void UpdateScores()
    {
        text.text = "";
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            text.text = text.text + player.NickName.ToString() + "  :  " + player.CustomProperties["Kills"] + " K / " + player.CustomProperties["Deaths"] + " D" + "\n";
        }
    }
}
