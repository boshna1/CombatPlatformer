using UnityEngine;

public class _PlayerBaseClass : MonoBehaviour
{
    //[Header("Player General Combo Variables")]
    public enum AttackState
    {
        Idle,
        Attacking
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*pPointer = GetComponentInChildren<PlayerPointer>();
        xDirMod = -1;
        yDirMod = -1;
        pi = GetComponent<PlayerInput>();
        //attatches other components
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
