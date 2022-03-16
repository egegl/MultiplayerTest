using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/* 
THIS SCRIPT IS FOR THE BULLET PREFAB TO GO FORWARD WHEN INSTANTIATED AND THEN DESTROY ON IMPACT
ALSO DAMAGE ANY COLLIDED ENEMIES BY ACCESING THEIR <Enemy> SCRIPT
*/

public class Bullet : MonoBehaviourPun
{
    [HideInInspector] public int speed;
    [HideInInspector] public int damage;
    public ParticleSystem impactPS;
    private Rigidbody2D _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // MOVING THE BULLET'S RIGIDBODY2D FORWARD ON START
        _rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy _enemy = collision.GetComponent<Enemy>();
            _enemy.Damage(damage, Color.yellow);
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerHealth _playerHealth = collision.GetComponent<PlayerHealth>();
            _playerHealth.TakeDamage(damage, Color.yellow);

            if (_playerHealth._currHP <= 0)
            {
                // Create variable equal to stored hashtable variable, increment, set
                int totalKills = (int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"];
                totalKills++;
                ExitGames.Client.Photon.Hashtable setPlayerKills = new ExitGames.Client.Photon.Hashtable() { { "Kills", totalKills } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(setPlayerKills);
            }
        }
        DestroyBullet();
        // AudioManager.instance.Play("projectilehit"); 
    }

    private void DestroyBullet()
    {
        Instantiate(impactPS, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
