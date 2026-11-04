using System.Collections;
using UnityEditor;
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

    [SerializeField] bool canAttack;

    [SerializeField] bool isResetting;

    [Header("Animation Variables")]
    string animationTrigger;
    string trimmedString;
    int comboCountGrounded;
    [SerializeField] float timeToIdle;
    [SerializeField] int maxComboCountGrounded;

    Coroutine resetReference;

    _AnimationHandler ah;
    [SerializeField] _AnimationHandler.ClipType _class;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ah = GetComponent<_AnimationHandler>();
        canAttack = true;
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
        if (resetReference != null)
        {
            isResetting = true;
        }
        else
        {
            isResetting = false;
        }
    }

    void OnAttack(InputAction.CallbackContext obj)
    {
        if (canAttack && obj.performed)
        {
            SetTrigger();
        }
        
    }

    string Trim(string convert)
    {
        Debug.Log(convert);
        if (convert.Equals("None"))
        {
            trimmedString = "";
        }
        else if (convert.Equals("Left") || convert.Equals("Right"))
        {
            trimmedString = "Horizontal";
        }
        else if (convert.Equals("Up") || convert.Equals("Down"))
        {
            trimmedString = "Vertical";
        }
        else if (!convert.Equals("Grounded") || !convert.Equals("Airborne"))
        {
            trimmedString = "";
        }
        Debug.Log(trimmedString);
        return trimmedString;
    }

    void SetTrigger()
    {
        canAttack = false;
        //string WalkingState = pm.walkingState.ToString();
        Debug.Log(Trim(pm.dirX.ToString()));
        string FacingX = Trim(pm.dirX.ToString());
        string FacingY = Trim(pm.dirY.ToString());
        string MovementState = pm.movementState.ToString();
        ah.LookUp(_class, FacingX + FacingY + MovementState + comboCountGrounded.ToString());
        comboCountGrounded++;
        if (comboCountGrounded > maxComboCountGrounded)
        {
            comboCountGrounded = 0;
        }
        
    }

    public void EnableAttack()
    {
        canAttack = true;

    }

}
