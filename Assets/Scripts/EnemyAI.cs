using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVERYTHING ABOUT ENEMY MOVEMENT IS IN THIS SCRIPT

public class EnemyAI : MonoBehaviour
{
    public Transform[] moveSpots;
    public float roamMoveSpeed;
    public float agroMoveSpeed;
    public int agroRange;
    private float _moveSpeed;
    private Transform _player;
    private Vector3 _startPos;
    private Vector3 _roamPos;
    private Vector3 _targetPos;
    private Rigidbody2D _rb;
    private Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startPos = transform.position;
        _roamPos = GetRoamPos();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, _player.position) < agroRange)
        {
            _moveSpeed = agroMoveSpeed;
            _targetPos = _player.position;
        }
        else
        {
            _moveSpeed = roamMoveSpeed;
            _targetPos = _roamPos;

            if (Vector2.Distance(transform.position, _roamPos) < 0.2f)
            {
                _roamPos = GetRoamPos();
            }
        }
        Vector3 newPos = Vector2.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);
        Vector3 diff = (_targetPos - transform.position).normalized;
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(newPos, Quaternion.Euler(0, 0, rotationZ));
    }

    private Vector3 GetRoamPos()
    {
        int randIndex = Random.Range(0, moveSpots.Length);
        return moveSpots[randIndex].position;
        // return _startPos + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * Random.Range(3, 8);
    }
}
