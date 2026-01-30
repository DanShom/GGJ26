using TarodevController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(MaskHandler))]
[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour, Damageable<float>
{
    private InputSystem_Actions playerInput;
    private FrameInput _frameInput;
    private SmartSwitch jumpSwtich;
    private CharacterMovement controller;


    private void GatherInput()
    {
        jumpSwtich.Update(playerInput.Player.Jump.IsPressed());
        if (jumpSwtich.OnPress())
        {
            Debug.Log("Jumped");
        }
        _frameInput = new FrameInput
        {
            JumpDown = jumpSwtich.OnPress(),
            JumpHeld = jumpSwtich.OnHold(),
            Move = playerInput.Player.Move.ReadValue<Vector2>()
        };

        controller.SetInput(playerInput.Player.Move.ReadValue<Vector2>(), jumpSwtich.OnPress(), jumpSwtich.OnHold());

        /*if (_stats.SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }

        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }*/
    }
    private void Awake()
    {
        controller = GetComponent<CharacterMovement>();
        jumpSwtich = new SmartSwitch();
        _frameInput = new FrameInput()
        {
            JumpDown = false,
            JumpHeld = false,
            Move = Vector2.zero
        };
        playerInput = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();

    }
    private void OnDisable()
    {
        playerInput.Player.Disable();

    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    public void OnDamage(float log)
    {
        //throw new System.NotImplementedException();
    }

    public bool IsDead()
    {
        //throw new System.NotImplementedException();
        return false;
    }
}
