using UnityEngine;
using System.Collections;

public class _FallingGround : _FallingObstacle
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override IEnumerator WaitRespawn()
    {
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
