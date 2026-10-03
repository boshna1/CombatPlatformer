using UnityEngine;

public class _CustomGravity : MonoBehaviour
{
    [SerializeField] float gravityScale;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale,ForceMode.Acceleration);
    }
}
