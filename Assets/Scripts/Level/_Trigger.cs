using UnityEngine;

public class _Trigger : MonoBehaviour
{
    [SerializeField] _BaseTrigger _trigger;
    
    public void Trigger()
    {
        _trigger.Trigger();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Trigger();
        }
    }
}
