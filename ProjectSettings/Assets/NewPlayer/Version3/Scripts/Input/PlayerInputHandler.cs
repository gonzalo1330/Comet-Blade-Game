using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public bool AttackInputStop { get; private set; }
    public bool MenuInput { get; private set; }
    public bool MenuInputStop { get; private set; }
    public bool PreviousLevel { get; private set; }
    public bool PreviousLevelStop { get; private set; }
    public bool ResetLevel { get; private set; }
    public bool ResetLevelStop { get; private set; }
    public bool LoadMainInput { get; private set; }
    public bool LoadMainInputStop { get; private set; }
    public bool QuitInput { get; private set; }
    public bool QuitInputStop { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float MenuInputStartTime;
    public float AttackInputStartTime { get; private set; }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckMenuInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        if(Mathf.Abs(RawMovementInput.x) > 0.5f)
        {
            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            NormInputX = 0;
        }
        
        if(Mathf.Abs(RawMovementInput.y) > 0.5f)
        {
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            NormInputY = 0;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            AttackInputStop = false;
            AttackInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            AttackInputStop = true;
        }
    }

    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MenuInput = true;
            MenuInputStop = false;
            MenuInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            MenuInputStop = true;
        }
    }

    
    public void OnPreviousLevelInput(InputAction.CallbackContext context) {
        if (context.started)
        {
            PreviousLevel = true;
            PreviousLevelStop = false;
        }
        else if (context.canceled)
        {
            PreviousLevelStop = true;
        }
    }

    public void OnResetLevelInput(InputAction.CallbackContext context) {
        if (context.started)
        {
            ResetLevel = true;
            ResetLevelStop = false;
        }
        else if (context.canceled)
        {
            ResetLevelStop = true;
        }
    }

    public void OnLoadMenuInput(InputAction.CallbackContext context) {
        if (context.started)
        {
            LoadMainInput = true;
            LoadMainInputStop = false;
        }
        else if (context.canceled)
        {
            LoadMainInputStop = true;
        }
    }

    public void OnQuitInput(InputAction.CallbackContext context) {
        if (context.started)
        {
            QuitInput = true;
            QuitInputStop = false;
        }
        else if (context.canceled)
        {
            QuitInputStop = true;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseMenuInput() => MenuInput = false;

    public void UseAttackInput() => AttackInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    private void CheckAttackInputHoldTime() {
        if(Time.time >= AttackInputStartTime + inputHoldTime)
        {
            AttackInput = false;
        }
    }

    private void CheckMenuInputHoldTime() {
        if(Time.time >= MenuInputStartTime + inputHoldTime)
        {
            MenuInput = false;
        }
    }
}
