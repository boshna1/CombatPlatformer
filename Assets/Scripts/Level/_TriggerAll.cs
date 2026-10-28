using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class _TriggerAll : MonoBehaviour
{
    [SerializeField] List<_BaseTrigger> _triggers;

    GameObject player;

    [SerializeField] float _distanceThreshold;

    private void Start()
    {
        player = _GlobalValues.player;
    }

    public void TriggerAll()
    {
        foreach (var _trigger in _triggers)
        {
            _trigger.GetComponent<_BaseTrigger>().Trigger();
        }
    }

    public void TriggerCondition()
    {
        foreach (var _trigger in _triggers)
        {
            if ((_trigger.transform.position - player.transform.position).magnitude < _distanceThreshold)
            {
                _trigger.GetComponent<_BaseTrigger>().Trigger();
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            TriggerAll();
        }
    }
}
