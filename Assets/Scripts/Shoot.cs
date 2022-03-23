using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

// PUT THIS SCRIPT ON WEAPON GAME OBJECTS SO THEY SHOOT 

public class Shoot : MonoBehaviourPunCallbacks
{
    public Transform firePoint;
    public ParticleSystem impactPS;
    public LineRenderer lineRenderer;
    public int damage;
    public float fireRate;

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
        _anim.SetFloat("animSpeed", 0.5f / fireRate);
    }
    void Update()
    {
        if (_isMine)
        {
            if (!attackCD) return;

            if (Input.GetButton("Fire1"))
            {
                StartCoroutine(Spray());
            }
        }
    }

    // ATTACK COOLDOWN HAS TO BE OVER BEFORE THE ATTACK METHOD CAN BE CALLED
    IEnumerator AttackCooldown()
    {
        attackCD = false;
        yield return new WaitForSeconds(fireRate);
        attackCD = true;
    }

    private IEnumerator Spray()
    {
        attackCD = false;
        photonView.RPC("RPC_SendRaycast", RpcTarget.All);
        yield return new WaitForSeconds(fireRate);
        attackCD = true;
    }

    [PunRPC]
    private void RPC_SendRaycast()
    {
        _anim.SetTrigger("Shoot");
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        AudioManager.instance.Play("gunshot");

        if (hitInfo)
        {
            PlayerHealth player = hitInfo.transform.GetComponent<PlayerHealth>();
            if (player != null)
            {
                AudioManager.instance.Play("hit");
                player.TakeDamage(damage, photonView.OwnerActorNr);
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            StartCoroutine(SendLine());
            Instantiate(impactPS, hitInfo.point, Quaternion.identity);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 90);
            StartCoroutine(SendLine());
        }
    }

    private IEnumerator SendLine()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }
}
