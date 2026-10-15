using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class _EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] List<_EnemySpawn> _enemySpawners;

    [SerializeField] float _distanceThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAll()
    {
        foreach (var _enemy in _enemySpawners)
        {
            //_enemy.Spawn();
        }
    }

    public void TriggerCondition()
    {
        foreach (var _enemy in _enemySpawners)
        {
            //if ((enemy.transform.position - player.position).magnitude < _distanceThreshold)
            {
                //_enemy.Spawn();
            }

        }
    }
}
