using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }
    private PlayerInput playerInput;
    private InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction fire30DownAction;
    private InputAction fire30UpAction;
    private InputAction fireStraightAction;
    private InputAction toggleAngleAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load and enable the input actions
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                playerInput = gameObject.AddComponent<PlayerInput>();
            }
            
            inputActions = playerInput.actions;
            
            // Cache the actions
            moveAction = inputActions.FindAction("Move");
            fire30DownAction = inputActions.FindAction("Fire30Down");
            fire30UpAction = inputActions.FindAction("Fire30Up");
            fireStraightAction = inputActions.FindAction("FireStraight");
            toggleAngleAction = inputActions.FindAction("ToggleAngle");

            // Enable all actions
            inputActions.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2 GetMovementInput()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public bool GetFire30Down()
    {
        return fire30DownAction.triggered;
    }

    public bool GetFire30Up()
    {
        return fire30UpAction.triggered;
    }

    public bool GetFireStraight()
    {
        return fireStraightAction.triggered;
    }

    public bool GetToggleAngle()
    {
        return toggleAngleAction.triggered;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            inputActions?.Disable();
            Instance = null;
        }
    }
}
