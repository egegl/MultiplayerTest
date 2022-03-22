using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/* 
THIS SCRIPT IS FOR THE BULLET PREFAB TO GO FORWARD WHEN INSTANTIATED AND THEN DESTROY ON IMPACT
ALSO DAMAGE ANY COLLIDED ENEMIES BY ACCESING THEIR <Enemy> SCRIPT
*/

public class Bullet : MonoBehaviourPun
{
    public ParticleSystem impactPS;
    private Rigidbody2D _rb;
    [HideInInspector] public int _creator;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // MOVING THE BULLET'S RIGIDBODY2D FORWARD ON START
        _rb.velocity = transform.right * 25;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            BulletHit();
            return;
        }
        else if (collision.gameObject.GetPhotonView().Owner.ActorNumber == _creator) return;
        
        PlayerHit(collision.gameObject);
    }

    private void BulletHit()
    {
        Instantiate(impactPS, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void PlayerHit(GameObject collision)
    {
        collision.GetComponent<PlayerHealth>().TakeDamage(18, _creator);
        AudioManager.instance.Play("hit");
        BulletHit();
    }
}
