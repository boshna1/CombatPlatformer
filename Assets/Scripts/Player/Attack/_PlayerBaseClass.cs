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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (attack != null && attack.action != null)
        attack.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attack.action.Disable();
        if (attack != null && attack.action != null)
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
        _playerAnimator.ResetTrigger("ResetToIdle");
        if (resetReference != null)
        {
            StopCoroutine(resetReference);
            resetReference = null;
        }  
        canAttack = false;
        //string WalkingState = pm.walkingState.ToString();
        Debug.Log(Trim(pm.dirX.ToString()));
        string FacingX = Trim(pm.dirX.ToString());
        string FacingY = Trim(pm.dirY.ToString());
        string MovementState = pm.movementState.ToString();
        _playerAnimator.SetTrigger(FacingX + FacingY + MovementState + comboCountGrounded.ToString());
        comboCountGrounded++;
        if (comboCountGrounded > maxComboCountGrounded)
        {
            comboCountGrounded = 0;
        }
        AnimatorClipInfo[] clipTime = _playerAnimator.GetCurrentAnimatorClipInfo(0);
        _playerAnimator.ResetTrigger(FacingX + FacingY + MovementState + comboCountGrounded.ToString());
        
    }

    public void EnableAttack()
    {
        canAttack = true;
        resetReference = StartCoroutine(ResetToIdle());
    }

    IEnumerator ResetToIdle()
    {
        yield return new WaitForSeconds(timeToIdle);
        _playerAnimator.SetTrigger("ResetToIdle");
        comboCountGrounded = 0;
        
        resetReference = null;
        StopCoroutine(resetReference);
        

    }
}
