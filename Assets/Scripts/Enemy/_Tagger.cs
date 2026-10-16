using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class _Tagger : _EnemyBase
{ 
    [SerializeField] List<Transform> _patrolPositions;
    int listIndex;
    Rigidbody rb;
    int _moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _patrolPositions = transform.parent.GetComponent<_TaggerSpawner>()._patrolPositions;
        GetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_moveDirection * base._moveSpeed, rb.linearVelocity.y);
    }

    void GetDirection()
    {
        _moveDirection = (int)(_patrolPositions[listIndex].transform.position - transform.position).magnitude;
        listIndex++;
        if (listIndex > _patrolPositions.Count - 1)
        {
            listIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("TaggerPatrol"))
        {
            GetDirection();
        }
        if (collision.transform.CompareTag("Player"))
        {
            //damage(base.damage);
        }
    }
}
