using System.Collections;
using UnityEngine;

[RequireComponent (typeof(_CustomGravity))]
public class _FallingObstacle : _BaseTrigger
{
    [SerializeField] float _fallingScale;

    [SerializeField] float _timeToFall;
    [SerializeField] float _timeToDestroy;

    private void Start()
    {
        GetComponent<_CustomGravity>().isGravityEffected = false;
        GetComponent<_CustomGravity>().gravityScale = _fallingScale;
    }

    public override void Trigger()
    {
        StartCoroutine(WaitBuffer());
        
    }

    IEnumerator WaitBuffer()
    {
        yield return new WaitForSeconds(_timeToFall);
        GetComponent<_CustomGravity>().isGravityEffected = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, _timeToDestroy);
        }
    }
}
