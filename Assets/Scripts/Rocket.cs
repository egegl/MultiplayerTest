using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
THIS SCRIPT IS FOR THE BULLET PREFAB TO GO FORWARD WHEN INSTANTIATED AND THEN DESTROY ON IMPACT
ALSO DAMAGE ANY COLLIDED ENEMIES BY ACCESING THEIR <Enemy> SCRIPT
*/

public class Rocket : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _playerRb;
    public ParticleSystem explosionPS;
    private Rigidbody2D _rb;

    void Start()
    {
        _player = transform.root.gameObject;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * 13;
        // Knockback();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2.5f);
        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position)/1.5f;
                collider.GetComponent<Enemy>().Damage(Mathf.RoundToInt(200/distance), Color.red);
            }
        }
        // INSTANTIATE THE IMPACT PARTICLE SYSTEM ON IMPACT POSITION
        Instantiate(explosionPS, transform.position, transform.rotation);
        CameraShake.Instance.Shake(2.5f, .2f);
        Destroy(gameObject);
    }
}
