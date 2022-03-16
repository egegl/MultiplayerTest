using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed, dashCooldown;
    private bool dashCD;
    private PlayerController _controller;
    private TrailRenderer _trail;

    // Start is called before the first frame update
    void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !dashCD)
        {
            StartCoroutine(Dash());
        }
    }
    private IEnumerator DashCooldown()
    {
        dashCD = true;
        yield return new WaitForSeconds(dashCooldown);
        dashCD = false;
    }
    private IEnumerator Dash()
    {
        _trail.enabled = true;
        // AudioManager.instance.Play("dash");
        StartCoroutine(DashCooldown());
        float currSpeed = _controller.moveSpeed;
        _controller.moveSpeed *= dashSpeed;
        yield return new WaitForSeconds(0.15f);
        _controller.moveSpeed = currSpeed;
        _trail.enabled = false;
    }   
}
