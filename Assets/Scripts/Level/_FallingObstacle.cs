using System.Collections;
using UnityEngine;

[RequireComponent (typeof(_CustomGravity))]
public class _FallingObstacle : _BaseTrigger
{
    [SerializeField] protected float _fallingScale;

    [SerializeField] protected float _timeToFall;
    [SerializeField] protected float _timeToDestroy;
    protected Vector3 _defaultPos;
    protected Vector3 _defaultAngles;

    protected Vector2 _normal;

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
        foreach (ContactPoint contact in collision.contacts)
        {
            _normal = contact.normal;
        }
        if (collision.transform.CompareTag("Player") && _normal.y == -1)
        {
            StartCoroutine(WaitRespawn());
        }
        if (collision.transform.CompareTag("Ground"))
        {
            StartCoroutine(HitGround());
        }
    }

    public override IEnumerator WaitRespawn()
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

    public override IEnumerator HitGround()
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
