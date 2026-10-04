using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class _PlayerMovement : MonoBehaviour
{
    
    //Movement enum
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        WallDraging,
        Grounded,

        OnWall,
        Airborne
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

    public Rigidbody rb;
    public float moveSpeed;

    [SerializeField] float _sprintSpeed;
    [SerializeField] float _defaultSpeed;
    public Vector2 _moveDirection;

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

    public bool isWallJumping;
    bool enableDoubleJump = true;

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


    [Header("Sound")]
    //velocity threshold to play landing sound
    [SerializeField] float landSoundThreshold = 3;

    [Header("Directions")]
    //current directions
    public DirX dirX;

    public DirY dirY;

    _AudioManager am;

    _CustomGravity cg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        _moveDirection = value.Get<Vector2>();
        if (_moveDirection != Vector2.zero)
        {
            pPointer.transform.position = _moveDirection + new Vector2(transform.position.x, transform.position.y);
        }
        else if (_moveDirection.x == 0)
        {
            moveSpeed = _defaultSpeed;
        }
    }

    public void OnSprint(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            moveSpeed = _sprintSpeed;
        }
        else
        {
            moveSpeed = _defaultSpeed;
        }
    }

    public void OnControlsChanged(PlayerInput currentInput)
    {
        if (currentInput.currentControlScheme == "Gamepad")
        {
            pi.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
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
                    Debug.Log("Added device");
                    break;
                case InputDeviceChange.Removed:
                    Debug.Log("Removed device");
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
        }
        else if (isGrounded || jumpCount < maxJump)
        {
            jumpCount++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void Dash(InputAction.CallbackContext obj)
    {
        if (!isDashing && canDash)
        {
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
            if (_moveDirection.x < 0)
            {
                dirX = DirX.Left;
                baseLungeDist = -baseLungeDist;
            }
            else if (_moveDirection.x > 0)
            {
                dirX = DirX.Right;
                baseLungeDist = 0.6f;
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
            }
            else if (normal.x != 0 && rb.linearVelocity.y < 0)
            {
                movementState = MovementState.WallDraging;
                if (rb.linearVelocity.y < landSoundThreshold)
                {
                    _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", 2.5f, 0.3f);
                }
            }
            else if (normal.y != -1)
            {
                _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", _AudioManager.Instance.defaultPitch, 0.3f);
                movementState = MovementState.Grounded;
                isGrounded = true;
                jumpCount = 0;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if (rb.linearVelocity.y != 0)
            {
                movementState = MovementState.Airborne;
                isGrounded = false;
                _AudioManager.Instance.PlaySoundAmbientPitch("GroundLand", 2.5f, 0.3f);
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
}
