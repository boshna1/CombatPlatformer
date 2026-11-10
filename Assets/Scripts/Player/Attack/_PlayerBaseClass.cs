using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static _PlayerMovement;

public class _PlayerBaseClass : MonoBehaviour
{
    //[Header("Player General Combo Variables")]
    public enum AttackState
    {
        Idle,
        Attacking
    }

    public enum Combo
    {
        HorizontalGrounded,
        HorizontalAirborne,
        UpGrounded,
        DownGrounded,
        UpAirborne,
        DownAirborne
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

    [SerializeField] int currentComboStep;
    [SerializeField] int currentMaxCombo;
    [SerializeField] Combo currentCombo;

    Dictionary<Combo, int> _comboDictionary = new Dictionary<Combo, int>();

    Coroutine resetReference;

    _AnimationHandler ah;
    _ComboValues cv;
    [SerializeField] _AnimationHandler.ClipType _class;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cv = GetComponent<_ComboValues>();
        _comboDictionary = cv._comboDictrionary;
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
            return convert;
        }
        else if (!convert.Equals("Grounded") || !convert.Equals("Airborne"))
        {
            trimmedString = "";
        }

        return trimmedString;
    }

    void CheckString(string trimmedString)
    {
        if (trimmedString != currentCombo.ToString())
        {
            currentComboStep = 0;
            currentCombo = Enum.Parse<Combo>(trimmedString);
            Debug.Log(currentCombo);
            currentMaxCombo = _comboDictionary[currentCombo];
            ExecuteAttack(trimmedString);
        }
        else
        {
            ExecuteAttack(trimmedString);
            currentComboStep++;
            if (currentComboStep > currentMaxCombo)
            {
                currentComboStep = 0;
            }

        }

    }

    void SetTrigger()
    {
        canAttack = false;
        //string WalkingState = pm.walkingState.ToString();
        string FacingX = Trim(pm.dirX.ToString());
        string FacingY = Trim(pm.dirY.ToString());
        string MovementState = pm.movementState.ToString();
        CheckString(FacingX + FacingY + MovementState);


    }

    void ExecuteAttack(string trimmedString)
    {
     if (!ah.LookUp(_class, trimmedString + currentComboStep.ToString()))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }


    public void EnableAttack()
    {
        canAttack = true;

    }

}
