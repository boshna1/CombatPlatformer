using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;
using Unity.Cinemachine;
using Unity.VisualScripting;
public class _PlayerMovement : MonoBehaviour
{
    
    //Movement enum
    public enum MovementState
    {
        WallDraging,
        Grounded,

        OnWall,
        Airborne
    }

    public enum WalkingState
    {
        Idle,
        Walking,
        Sprinting, 
        Dashing

    }

    //player input direction enums
    public enum DirX
    {
        Left,
        Right,
        None,
    }
    public enum DirY
    {
        Up,
        Down,
        None
    }

    [Header("Player General Movement Variables")]
    [Header("Current State")]
    public MovementState movementState;
    public WalkingState walkingState;

    public Rigidbody rb;
    public float moveSpeed;

    [SerializeField] float _sprintSpeed;
    [SerializeField] float _defaultSpeed;
    public Vector2 _moveDirection;

    [SerializeField] float _timeToStopSprint;

    [Header("Player Dash Variables")]
    public bool canDash;
    public float dashForceX;
    public float dashVelocityX;
    public float dashFallOffDuration;
    public float residueSpeedX;
    public float residueSpeedY;
    public float dashDuration;
    public float dashTime;
    public float dashFalloff;
    public float dashDistance;

    public float baseLungeDist;

    public float dashCooldown;

    [Header("Player Knockback Variables")]
    public bool isKnockback;
    public Vector2 knockbackVelocity;
    public float knockbackX;
    public float knockbackTime;
    public float knockbackFalloff;
    public float knockbackFallOffDuration;
    public float knockbackDuration;
    public bool localKnockback;

    [Header("Player Condition Variables")]
    public bool isGrounded;
    public bool isDashing;
    public bool isLunging;
    public bool isSprinting;
    Coroutine _isSprint;

    public bool isWallJumping;
    bool enableDoubleJump = true;

    bool isMoveAnim;

    [Header("Player Jump Variables")]
    public int jumpCount = 0;
    public int maxJump = 2;
    public float jumpForce;
    public float airBufferTime;

    [Header("Player Hop Variables")]
    public float hopModifierX;
    public float hopModifierY;
    public float knockbackFallOff;

    

    [Header("Player Wall Jump Variables")]
    public float wallJumpEffectTime;
    public float wallJumpCurrentTime;
    public Vector2 wallJumpForce;
    public Vector2 defaultWallJumpForce;
    public float movementDamper;
    public float movementDampTime;
    public float defaultMovementDamper;
    public float movementDampDuration;

    [Header("Coyote Time Variables")]

    public float coyoteTime;

    [Header("Input Actions")]

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference dash;
    [SerializeField] InputActionReference sprint;

    //indicates respective player
    [SerializeField] PlayerInput pi;

    //current controller
    Gamepad currentGamepad;

    //ground normal
    Vector2 normal;
    _PlayerMovement pm;
    _PlayerPointer pPointer;
    Animator _playerAnim;


    [Header("Sound")]
    //velocity threshold to play landing sound
    [SerializeField] float landSoundThreshold = 3;

    [Header("Directions")]
    //current directions
    public DirX dirX;

    public DirY dirY;

    _AudioManager am;

    _CustomGravity cg;

    [Header("Camera")]

    [SerializeField] CinemachineCamera _defaultCam;
    [SerializeField] CinemachineCamera _sprintCam;

    [Header("Effects")]
    [SerializeField] _EffectHandler EH;

    [Header("HUD")]
    [SerializeField] _HUDManager HUD;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerAnim = GetComponentInChildren<Animator>();
        cg = GetComponent<_CustomGravity>();
        pPointer = GetComponentInChildren<_PlayerPointer>();
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        isKnockback = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        
        if (residueSpeedX != 0)
        {
            PassEnableDash(false);
            residueSpeedX = Mathf.MoveTowards(residueSpeedX, 0, 1);
            rb.linearVelocity = new Vector2(residueSpeedX, 0) + new Vector2(_moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }
        if (isWallJumping)
        {
            WallJump();
        }
        else if (isDashing)
        {
            DashingFunction();
        }
        else if (isKnockback)
        {
            KnockBackFunction();
        }
        else
        {
            rb.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }
        if (rb.linearVelocity.y < 0 && movementState == MovementState.WallDraging)
        {
            rb.linearDamping = 5;
        }
        else
        {
            rb.linearDamping = 0;
        }
        if (rb.linearVelocity.y > 3 && movementState == MovementState.Airborne)
        {
            _playerAnim.SetTrigger("Fall");
        }
        if (_moveDirection.x != 0)
            _playerAnim.SetBool("Moving", true);
        else
        {
            _playerAnim.SetBool("Moving", false);
        }
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        dash.action.Enable();
        sprint.action.Enable();
        jump.action.started += Jump;
        dash.action.started += Dash;
        sprint.action.performed += OnSprint;
        InputSystem.onDeviceChange += OnDeviceChange;
        pi.onControlsChanged += OnControlsChanged;
       
    }

    private void OnDisable()
    {

        move.action.Disable();
        jump.action.Disable();
        dash.action.Disable();
        sprint.action.Disable();
        jump.action.started -= Jump;
        dash.action.started -= Dash;
        sprint.action.performed -= OnSprint;
        InputSystem.onDeviceChange -= OnDeviceChange;
        pi.onControlsChanged -= OnControlsChanged;
    }

    public void OnMove(InputValue value)
    {
        CalculateDirections();
        EH.Transform(dirX);

        if (_isSprint != null)
        {
            StopCoroutine(_isSprint);
        }
        _moveDirection = value.Get<Vector2>();
        
        if (value.Get<Vector2>() != Vector2.zero)
        {    
            pPointer.transform.position = _moveDirection + new Vector2(transform.position.x, transform.position.y);
            transform.localScale = new Vector3(_moveDirection.normalized.x, transform.localScale.y, transform.localScale.z);
        }
        else if (value.Get<Vector2>() == Vector2.zero)
        {
            if (_isSprint != null)
            {
                _isSprint = StartCoroutine(DisableSprint());
            }
            
            _playerAnim.SetTrigger("ResetToIdle");
        }
        
    }

    public void OnSprint(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            CalculateDirections();
            EH.Effect(true, dirX);
            moveSpeed = _sprintSpeed;
        }
    }

    public void OnControlsChanged(PlayerInput currentInput)
    {
        if (currentInput.currentControlScheme == "Controller")
        {
            pi.SwitchCurrentControlScheme("Controller", Gamepad.current);
        }
        else if (currentInput.currentControlScheme == "Keyboard")
        {
            pi.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
        }
        Debug.Log("Changed input to" + currentInput.currentControlScheme);
    }

    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    Debug.Log("Added Controller");
                    pi.SwitchCurrentControlScheme("Controller", Gamepad.current);
                    break;
                case InputDeviceChange.Removed:
                    Debug.Log("Removed Controller");
                    break;
            }
        }
        else if (device is Keyboard)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    Debug.Log("Added Keyboard");
                    pi.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
                    break;
                case InputDeviceChange.Removed:
                    Debug.Log("Removed Keyboard");
                    break;
            }
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        cg.isGravityEffected = true;
        if (movementState == MovementState.WallDraging || movementState == MovementState.OnWall)
        {
            isWallJumping = true;
            _playerAnim.SetTrigger("Jump");
        }
        else if (isGrounded || jumpCount < maxJump)
        {
            
            if (jumpCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.85f);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            jumpCount++;
            isGrounded = false;
            _playerAnim.SetBool("Grounded", false);
            _playerAnim.SetTrigger("Jump");
        }
        
    }

    private void Dash(InputAction.CallbackContext obj)
    {
        if (!isDashing && canDash)
        {
            HUD.TriggerButton("buttonDash", dashCooldown);
            cg.isGravityEffected = false;
            _AudioManager.Instance.PlaySoundAmbientPitch("Whoosh", 2.5f, 0.8f);
            PassEnableDash(true);
            dashVelocityX = _moveDirection.x * dashForceX;
            dashTime = 0;
            isDashing = true;
            dashFallOffDuration = 2;
            dashFalloff = 10;
            dashDuration = 0.25f;
            StartCoroutine(WaitCooldownDash(dashCooldown));
        }
    }

    public bool ReturnIsDashing()
    {
        return isDashing;
    }

    public bool ReturnIsLunging()
    {
        return isLunging;
    }

    public bool ReturnIsGrounded()
    {
        return isGrounded;
    }

    public void Hop(float y)
    {
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + y);
        isGrounded = false;
        _playerAnim.SetBool("Grounded", false);
    }

    public Vector2 ReturnMoveDir()
    {
        return _moveDirection;
    }

    public void DashingFunction()
    {
        dashTime += Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(dashVelocityX, 0);
        if (dashForceX > dashFalloff)
        {
            dashVelocityX = Mathf.Lerp(dashVelocityX, 0, dashTime / dashFallOffDuration);
        }
        if (dashTime >= dashDuration)
        {
            dashForceX = 35;
            isDashing = false;
            cg.isGravityEffected = true;
            if (isLunging)
            {
                isLunging = false;
            }
            residueSpeedX = dashVelocityX;
            _isSprint = StartCoroutine(DisableSprint());
        }
    }

    public void Lunge(float modifier)
    {
        dashVelocityX = (_moveDirection.x + baseLungeDist) * modifier;
        dashTime = 0;
        isDashing = true;
        isLunging = true;
        dashFallOffDuration = 0.25f;
        dashFalloff = 0.0005f;
        dashDuration = 0.0005f;
    }

    public void KnockBackFunction()
    {
        knockbackTime += Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(knockbackVelocity.x, rb.linearVelocity.y);
        if (knockbackX > knockbackFalloff)
        {
            knockbackVelocity.x = Mathf.Lerp(knockbackVelocity.x, 0, knockbackTime / knockbackFallOffDuration);
        }
        if (knockbackTime >= knockbackDuration)
        {
            knockbackX = 0;
            isKnockback = false;
        }
        residueSpeedX = knockbackVelocity.x;
    }

    public void WallJump()
    {
        wallJumpCurrentTime += Time.fixedDeltaTime;
        movementDampTime += Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(normal.x * wallJumpForce.x + _moveDirection.x * moveSpeed * 1 / movementDamper, wallJumpForce.y);
        wallJumpForce.x = Mathf.Lerp(wallJumpForce.x, 8, wallJumpCurrentTime / wallJumpEffectTime);
        movementDamper = Mathf.Lerp(movementDamper, 1, movementDampTime / movementDampDuration);
        if (wallJumpCurrentTime >= wallJumpEffectTime)
        {
            isWallJumping = false;
            wallJumpCurrentTime = 0;
            movementDamper = defaultMovementDamper;
            movementDampTime = 0;
            movementDamper = 10;
            wallJumpForce = defaultWallJumpForce;
        }
    }

    public void EnableKnockBack(Vector2 knockbackVelocity, float knockbackX, float knockbackFalloff, float knockbackFallOffDuration, float knockbackDuration)
    {
        knockbackTime = 0;
        localKnockback = false;
        isKnockback = true;
        this.knockbackVelocity = knockbackVelocity;
        this.knockbackX = knockbackX;
        this.knockbackFalloff = knockbackFalloff;
        this.knockbackFallOffDuration = knockbackFallOffDuration;
        this.knockbackDuration = knockbackDuration;
    }

    public void PassEnableDash(bool condition)
    {
        // pass to attack script
    }

    public void CalculateDirections()
    {
        if (Mathf.Abs(_moveDirection.x) > 0.2f)
        {
            if (_moveDirection.x > 0)
            {
                dirX = DirX.Right;
            }
            else if (_moveDirection.x < 0)
            {
                dirX = DirX.Left;
            }
            else
            {
                dirX = DirX.None;
            }
        }
        if (Mathf.Abs(_moveDirection.y) > 0.2f)
        {
            if (_moveDirection.y < 0)
            {
                dirY = DirY.Down;
            }
            else if (_moveDirection.y > 0)
            {
                dirY = DirY.Up;
            }

        }
        else
        {
            dirY = DirY.None;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                normal = contact.normal;
            }
            Debug.Log(normal);
            if (normal.x != 0 && rb.linearVelocity.y >= 0)
            {
                movementState = MovementState.OnWall;
                _playerAnim.SetBool("Airborne", false);
            }
            else if (normal.x != 0 && rb.linearVelocity.y < 0)
            {
                movementState = MovementState.WallDraging;
                if (rb.linearVelocity.y < landSoundThreshold)
                {
                    _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", 2.5f, 0.3f);
                }
            }
            else if (normal.y != -1 && movementState == MovementState.Airborne)
            {
                _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", _AudioManager.Instance.defaultPitch, 0.3f);
                movementState = MovementState.Grounded;
                isGrounded = true;
                _playerAnim.SetBool("Grounded", true);
                _playerAnim.SetBool("Airborne", false);
                jumpCount = 0;
                _playerAnim.SetTrigger("ResetToIdle");
            }
        }
        _playerAnim.ResetTrigger("Jump");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if (rb.linearVelocity.y != 0)
            {
                if (movementState != MovementState.WallDraging)
                {
                    _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", 2.5f, 0.3f);
                }
                else
                {
                    _AudioManager.Instance.PlaySoundAmbientPitch("Wall_Jump", 2.5f, 0.3f);
                }
                movementState = MovementState.Airborne;
                _playerAnim.SetBool("Airborne",true);
                isGrounded = false;
                _playerAnim.SetBool("Grounded", false);



            }
            if (normal.y == 1 && rb.linearVelocity.y <= 0)
            {
                cg.isGravityEffected = false;
                StartCoroutine(CoyoteTime());
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (movementState == MovementState.OnWall && rb.linearVelocity.y < -1 && collision.transform.CompareTag("Ground"))
        {
            movementState = MovementState.WallDraging;
        }
    }


    IEnumerator WaitCooldownDash(float time)
    {
        canDash = false;
        yield return new WaitForSeconds(time);
        canDash = true;
    }
    
    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        cg.isGravityEffected = true;
    }

    IEnumerator DisableSprint()
    {
        moveSpeed = _sprintSpeed;
        ChangeCam(_sprintCam, 50);
        yield return new WaitForSeconds(_timeToStopSprint);
        EH.Effect(false);
        ChangeCam(_sprintCam, -1);
        moveSpeed = _defaultSpeed;
    }

    void ChangeCam(CinemachineCamera cam, int value) // 0 = default | -1 = sprint
    {
        cam.Priority.Value = value;
    }
}
