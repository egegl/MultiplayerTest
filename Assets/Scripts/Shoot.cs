using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

// PUT THIS SCRIPT ON WEAPON GAME OBJECTS SO THEY SHOOT 

public class Shoot : MonoBehaviourPunCallbacks
{
    public Weapon weapon;
    public GameObject bulletPrefab;
    private Transform _barrel;
    private Animator _anim;
    private bool attackCD = true;
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
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        _barrel = transform.Find("Barrel");
        _anim.SetFloat("animSpeed", 0.5f / weapon.fireRate);
    }
    void Update()
    {
        if (_isMine)
        {
            if (!weapon.fullAuto)
            {
                if (Input.GetButtonDown("Fire1") && attackCD)
                {
                    AttackOnce();
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && attackCD)
                {
                    StartCoroutine(Spray());
                }
            }
        }
    }

    // ATTACK COOLDOWN HAS TO BE OVER BEFORE THE ATTACK METHOD CAN BE CALLED
    IEnumerator AttackCooldown()
    {
        attackCD = false;
        yield return new WaitForSeconds(weapon.fireRate);
        attackCD = true;
    }

    private void AttackOnce()
    {
        _anim.SetTrigger("Shoot");
        photonView.RPC("RPC_SendBullet", RpcTarget.All, photonView.OwnerActorNr);
        StartCoroutine(AttackCooldown());
    }
    private IEnumerator Spray()
    {
        attackCD = false;
        _anim.SetTrigger("Shoot");
        photonView.RPC("RPC_SendBullet", RpcTarget.All, photonView.OwnerActorNr);
        yield return new WaitForSeconds(weapon.fireRate);
        attackCD = true;
    }

    [PunRPC]
    public void RPC_SendBullet(int senderNumber)
    {
        AudioManager.instance.Play("gunshot");
        Instantiate(bulletPrefab, _barrel.position, _barrel.rotation);
        bulletPrefab.GetComponent<Bullet>()._creator = senderNumber;
    }
}
