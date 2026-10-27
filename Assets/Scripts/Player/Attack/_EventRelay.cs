using UnityEngine;

public class _EventRelay : MonoBehaviour
{
    _PlayerBaseClass pbc;

    private void Start()
    {
        pbc = GetComponentInParent<_PlayerBaseClass>();
    }
    public void Relay()
    {
        pbc.EnableAttack();
    }
}
