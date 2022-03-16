using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    public Text ping;
    public GameObject tabPanel;
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnPlayer();

        ExitGames.Client.Photon.Hashtable setPlayerKills = new ExitGames.Client.Photon.Hashtable() {{ "Kills", 0 }};
        PhotonNetwork.LocalPlayer.SetCustomProperties(setPlayerKills);

        ExitGames.Client.Photon.Hashtable setPlayerDeaths = new ExitGames.Client.Photon.Hashtable() { { "Deaths", 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(setPlayerDeaths);
    }

    void Update()
    {
        ping.text = "Ping: " + PhotonNetwork.GetPing().ToString() + "ms";

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            tabPanel.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            return;
        }
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}
