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
    private SpriteRenderer _renderer;
    public GameObject damagePopup;
    private TextMesh _popupTM;
    private bool _isMine;
    private Animator _anim;

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
        }
    }

    [PunRPC]
    public void RPC_Die()
    {
        Instantiate(hurtPS, transform.position, transform.rotation);
    }

    [PunRPC]
    public void RPC_DamagePopup()
    {
        Instantiate(damagePopup, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(-16, 16)));
    }

    public void TakeDamage(int damage, int damagerID)
    {
        if (_isMine)
        {
            if (damagerID == photonView.OwnerActorNr) return;

            _currHP -= damage;
            _popupTM.text = damage.ToString();
            photonView.RPC("RPC_DamagePopup", RpcTarget.All);
            _anim.SetTrigger("Hurt");
            HealthBar.Instance.SetHealth(_currHP);
            CameraShake.Instance.Shake(2f, .2f);

            if (_currHP <= 0)
            {
                Player killer = PhotonNetwork.CurrentRoom.GetPlayer(damagerID);
                photonView.RPC("RPC_Die", RpcTarget.All);
                AudioManager.instance.Play("death");
                    
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
            }
        }
    }
}
