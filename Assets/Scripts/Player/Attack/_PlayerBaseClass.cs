using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class _PlayerBaseClass : MonoBehaviour
{
    //[Header("Player General Combo Variables")]
    public enum AttackState
    {
        Idle,
        Attacking
    }

    [Header("Components")]
    _PlayerPointer pPointer;
    PlayerInput pi;
    _PlayerMovement pm;
    Rigidbody rb;
    Animator _playerAnimator;

    [Header("Input Actions")]
    [SerializeField] InputActionReference attack;


    [Header("Conditions")]

    bool canAttack;

    [Header("Animation Variables")]
    string animationTrigger;
    string trimmedString;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerAnimator = GetComponentInChildren<Animator>();
        pPointer = GetComponentInChildren<_PlayerPointer>();
        pi = GetComponent<PlayerInput>();
        //attatches other components
        pm = GetComponent<_PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        attack.action.Enable();
        attack.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attack.action.Disable();
        attack.action.performed -= OnAttack;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack(InputAction.CallbackContext obj)
    {
        if (canAttack)
        {
            SetTrigger();
        }
        
    }

    string Trim(string convert)
    {
        if (convert.Equals("None"))
        {
            trimmedString = "";
        }
        if (convert.Equals("Left") || convert.Equals("Right"))
        {
            trimmedString = "Horizontal";
        }
        if (convert.Equals("Up") || convert.Equals("Down"))
        {
            trimmedString = "Vertical";
        }
        return trimmedString;
    }

    void SetTrigger()
    {
        canAttack = false;
        AnimatorClipInfo[] clipTime = _playerAnimator.GetCurrentAnimatorClipInfo(0);
        StartCoroutine(WaitAnimationCooldown(clipTime.Length));
    }

    IEnumerator WaitAnimationCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
