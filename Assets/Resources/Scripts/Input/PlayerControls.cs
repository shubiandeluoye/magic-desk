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
                Debug.LogWarning("PlayerInput component not found, adding one...");
                playerInput = gameObject.AddComponent<PlayerInput>();
            }
            
            inputActions = playerInput.actions;
            if (inputActions == null)
            {
                Debug.LogError("Input Actions asset not assigned to PlayerInput component!");
                return;
            }
            
            // Cache the actions with null checks
            moveAction = inputActions.FindAction("Move");
            fire30DownAction = inputActions.FindAction("Fire30Down");
            fire30UpAction = inputActions.FindAction("Fire30Up");
            fireStraightAction = inputActions.FindAction("FireStraight");
            toggleAngleAction = inputActions.FindAction("ToggleAngle");

            if (moveAction == null || fire30DownAction == null || fire30UpAction == null || 
                fireStraightAction == null || toggleAngleAction == null)
            {
                Debug.LogError("One or more required input actions not found in the Input Actions asset!");
                return;
            }

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
        if (moveAction == null)
        {
            Debug.LogWarning("Move action is null!");
            return Vector2.zero;
        }
        return moveAction.ReadValue<Vector2>();
    }

    public bool GetFire30Down()
    {
        if (fire30DownAction == null)
        {
            Debug.LogWarning("Fire30Down action is null!");
            return false;
        }
        return fire30DownAction.triggered;
    }

    public bool GetFire30Up()
    {
        if (fire30UpAction == null)
        {
            Debug.LogWarning("Fire30Up action is null!");
            return false;
        }
        return fire30UpAction.triggered;
    }

    public bool GetFireStraight()
    {
        if (fireStraightAction == null)
        {
            Debug.LogWarning("FireStraight action is null!");
            return false;
        }
        return fireStraightAction.triggered;
    }

    public bool GetToggleAngle()
    {
        if (toggleAngleAction == null)
        {
            Debug.LogWarning("ToggleAngle action is null!");
            return false;
        }
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
