using UnityEngine;
using System.Collections.Generic;

public class _CheckpointManager : MonoBehaviour
{
    public _Checkpoint currentCheckpoint;
    _Checkpoint lastCheckpoint;
    [SerializeField] GameObject player;
    [SerializeField] public List<_Checkpoint> checkpointList;

    void Awake()
    {
        _GlobalValues.SetCM(this);
    }

    public void SetCheckpoint(_Checkpoint value)
    {
        currentCheckpoint = value;
    }

    public void ToLastCheckpoint()
    {
        Debug.Log("ToLastTP");
        player.transform.position = currentCheckpoint.transform.position;
    }

    public void RevertCheckpoint()
    {
        currentCheckpoint = lastCheckpoint;
        currentCheckpoint = null;
    }
}
