using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Settings")] 
    [SerializeField] private float _inputDeadZone = 0.05f;

    private PlayerInput _input;
    private Vector2 _rawMovement;
    
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool PausePressed { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Gameplay.Enable();

        _input.Gameplay.Move.performed += ctx => _rawMovement = ctx.ReadValue<Vector2>();
        _input.Gameplay.Move.canceled += _ => _rawMovement = Vector2.zero;

        _input.Gameplay.Jump.performed += _ =>
        {
            JumpPressed = true;
            JumpHeld = true;
        };

        _input.Gameplay.Jump.canceled += _ => { JumpHeld = false; };

        _input.Gameplay.Attack.performed += _ => AttackPressed = true;

        _input.Gameplay.Pause.performed += _ => PausePressed = true;
    }

    private void OnDisable()
    {
        _input.Gameplay.Disable();
    }

    private void Update()
    {
        CaptureMovementInput();
    }
    
    private void CaptureMovementInput()
    {
        var x = _rawMovement.x;
        HorizontalInput = Mathf.Abs(x) < _inputDeadZone ? 0f : Mathf.Sign(x) * Mathf.Min(Mathf.Abs(x), 1f);
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        AttackPressed = false;
        PausePressed = false;
    }
}
