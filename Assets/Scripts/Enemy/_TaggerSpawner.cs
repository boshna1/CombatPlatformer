using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class _TaggerSpawner : _EnemySpawn
{
    [SerializeField] public List<Transform> _patrolPositions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Trigger()
    {
        base.Trigger();
    }
}
