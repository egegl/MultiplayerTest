using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float cooldown;
    private Transform _barrel;
    private Animator _anim;
    private bool _attackCD = true;
    private Rocket _rocket;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _barrel = transform.Find("Barrel");
        _rocket = rocketPrefab.GetComponent<Rocket>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _attackCD)
        {
            AttackOnce();
        }
    }
    private void AttackOnce()
    {
        // PLAY WEAPON ROCKET SOUND (NOT IMPLEMENTED YET SO COMMENTED OUT)
        // AudioManager.instance.Play("rocket");

        CameraShake.Instance.Shake(2.5f, .2f);
        SendBullet();
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        _attackCD = false;
        yield return new WaitForSeconds(cooldown);
        _attackCD = true;
    }
    private void SendBullet()
    {
        Instantiate(rocketPrefab, _barrel.position, _barrel.rotation);
        _anim.SetTrigger("Shoot");
    }
}
