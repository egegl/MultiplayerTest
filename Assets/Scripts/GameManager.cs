using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    public GameObject deathPanel;
    public GameObject hpPanel;
    public Text ping;
    public Text fps;
    public GameObject tabPanel;
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public static GameManager instance;

    private int frameCount;
    private float time;

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

        time += Time.deltaTime;
        frameCount++;
        if (time >= 1)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fps.text = frameRate.ToString() + " fps";
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            tabPanel.SetActive(false);
        }
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }

    // FIX
    public void PlayerDied(GameObject player)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        StartCoroutine(ReactivatePlayer(player, spawnPoint));
    }

    private IEnumerator ReactivatePlayer(GameObject player, Transform spawnPoint)
    {
        player.transform.position = spawnPoint.position;
        yield return new WaitForSeconds(3.7f);
        player.SetActive(true);
    }

    public void DeathGUIOpen()
    {
        hpPanel.SetActive(false);
        deathPanel.SetActive(true);
    }

    public void DeathGUIClose()
    {
        hpPanel.SetActive(true);
        deathPanel.SetActive(false);
    }
}
