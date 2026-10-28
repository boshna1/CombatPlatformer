using System.Collections;
using UnityEngine;

[RequireComponent (typeof(_CustomGravity))]
public class _FallingObstacle : _BaseTrigger
{
    [SerializeField] float _fallingScale;

    [SerializeField] float _timeToFall;
    [SerializeField] float _timeToDestroy;
    Vector3 _defaultPos;
    Vector3 _defaultAngles;

    private void Start()
    {
        GetComponent<_CustomGravity>().isGravityEffected = false;
        GetComponent<_CustomGravity>().gravityScale = _fallingScale;
        _defaultPos = transform.position;
        _defaultAngles = transform.eulerAngles;
    }

    public override void Trigger()
    {
        StartCoroutine(WaitBuffer());
        
    }

    IEnumerator WaitBuffer()
    {
        yield return new WaitForSeconds(_timeToFall);
        GetComponent<_CustomGravity>().isGravityEffected = true;
        GetComponent<Rigidbody>().freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(WaitRespawn());
        }
        if (collision.transform.CompareTag("Ground"))
        {
            StartCoroutine(HitGround());
        }
    }

    IEnumerator WaitRespawn()
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.position = _defaultPos;
        
        GetComponent<_CustomGravity>().isGravityEffected = false;
        yield return new WaitForSeconds(_timeToDestroy);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.eulerAngles = _defaultAngles;

    }

    IEnumerator HitGround()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        GetComponent<MeshRenderer>().enabled = true;
        transform.position = _defaultPos;

        GetComponent<_CustomGravity>().isGravityEffected = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.eulerAngles = _defaultAngles;

    }
}
