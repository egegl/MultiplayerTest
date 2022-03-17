using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public ParticleSystem hurtPS;
    public int maxHP;
    [HideInInspector] public int _currHP;
    private Animator _anim;
    public GameObject damagePopup;
    private TextMesh _popupTM;
    private bool _isMine;

    void Awake()
    {
        if (photonView.IsMine)
        {
            _isMine = true;
        }
        else
        {
            _isMine = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _popupTM = damagePopup.GetComponent<TextMesh>();
    }

    public override void OnEnable()
    {
        if (_isMine)
        {
            _currHP = maxHP;
            HealthBar.Instance.SetMaxHealth(_currHP);
            GameManager.instance.DeathGUIClose();
        }
    }

    public void TakeDamage(int damage, int damagerID)
    {
        if (_isMine)
        {
            _currHP -= damage;
            photonView.RPC("RPC_DamagePopup", RpcTarget.All, damage);
            HealthBar.Instance.SetHealth(_currHP);
            if (_currHP <= 0)
            {
                Player damager = null;
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.ActorNumber == damagerID)
                    {
                        damager = player;
                    }
                }
                photonView.RPC("RPC_Die", RpcTarget.All, damager);
                return;
            }
            CameraShake.Instance.Shake(1.5f, .2f);
            _anim.SetTrigger("Hurt");
        }
    }

    [PunRPC]
    void RPC_DamagePopup(int damage)
    {
        _popupTM.text = damage.ToString();
        Instantiate(damagePopup, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(-16, 16)));
    }

    [PunRPC]
    void RPC_Die(Player killer)
    {
        // INSTANTIATE PARTICLE SYSTEM
        Instantiate(hurtPS, transform.position, transform.rotation);
        // PLAY DEATH SOUND
        // AudioManager.instance.Play("playerdeath");

        // Make killer's kills equal to stored hashtable variable, increment, set
        int totalKills = (int)killer.CustomProperties["Kills"];
        totalKills++;
        ExitGames.Client.Photon.Hashtable setPlayerKills = new ExitGames.Client.Photon.Hashtable() { { "Kills", totalKills } };
        killer.SetCustomProperties(setPlayerKills);

        // Make client's deaths equal to stored hashtable variable, increment, set
        int totalDeaths = (int)photonView.Owner.CustomProperties["Deaths"];
        totalDeaths++;
        ExitGames.Client.Photon.Hashtable setPlayerDeaths = new ExitGames.Client.Photon.Hashtable() { { "Deaths", totalDeaths } };
        photonView.Owner.SetCustomProperties(setPlayerDeaths);

        GameManager.instance.PlayerDied(gameObject);

        if(_isMine)
        {
            GameManager.instance.DeathGUIOpen();
        }

        gameObject.SetActive(false);
    }

}
