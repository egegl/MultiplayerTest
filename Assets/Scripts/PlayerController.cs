using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

// PUT THIS SCRIPT ON PLAYER GAME OBJECT SO IT CAN MOVE

public class PlayerController : MonoBehaviourPunCallbacks
{
    public TextMeshPro username;
    public GameObject playerCam;
    public float moveSpeed;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Camera _cam;
    private bool _isMine;
    private Vector2 _movement;
    public float dashSpeed, dashCooldown;
    private bool dashCD;
    private TrailRenderer _trail;

    void Awake()
    {
        if (photonView.IsMine)
        {
            _isMine = true;
            playerCam.SetActive(true);
            username.text = PhotonNetwork.NickName  ;
            username.color = Color.cyan;
        }
        else
        {
            _isMine = false;
            username.text = photonView.Owner.NickName;
        }
    }
    void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _cam = Camera.main;
    }

    void Update()
    {
        if (_isMine)
        {
            Movement();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);
                PhotonNetwork.LeaveRoom();
                SceneLoader.instance.Load("Lobby");
            }
        } 
    }

    private void Movement()
    {
        Vector3 diff = (_cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        _rb.velocity = _movement * moveSpeed;

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _anim.SetFloat("MoveSpeed", _movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !dashCD)
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
        float currSpeed = moveSpeed;
        moveSpeed *= dashSpeed;
        yield return new WaitForSeconds(0.1f);
        moveSpeed = currSpeed;
        _trail.enabled = false;
    }   
}