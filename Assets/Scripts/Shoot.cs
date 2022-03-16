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
    private Bullet _bullet;
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
        _bullet = bulletPrefab.GetComponent<Bullet>();
        Sync();
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
        // PLAY WEAPON SHOT SOUND (NOT IMPLEMENTED YET SO COMMENTED OUT)
        // AudioManager.instance.Play("weapon.attackSound");
        _anim.SetTrigger("Shoot");
        photonView.RPC("RPC_SendBullet", RpcTarget.All);
        StartCoroutine(AttackCooldown());
    }
    private IEnumerator Spray()
    {
        attackCD = false;
        _anim.SetTrigger("Shoot");
        photonView.RPC("RPC_SendBullet", RpcTarget.All);
        yield return new WaitForSeconds(weapon.fireRate);
        attackCD = true;
    }

    [PunRPC]
    void RPC_SendBullet()
    {
        PhotonNetwork.Instantiate("Bullet", _barrel.position, _barrel.rotation);
    }

    public void Sync()
    {
        _anim.SetFloat("animSpeed", 0.5f/weapon.fireRate);
        transform.localScale = new Vector3(weapon.width, weapon.height, 1);
        _bullet.speed = weapon.bulletSpeed;
        _bullet.damage = weapon.bulletDamage;
    }
}
