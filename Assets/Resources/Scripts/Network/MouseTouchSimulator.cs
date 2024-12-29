using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(PlayerControls))]
public class MouseTouchSimulator : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private Mouse mouse;
    private Keyboard keyboard;
    private PlayerControls playerControls;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouse = Mouse.current;
        keyboard = Keyboard.current;
        playerControls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        // 如果在编辑器中运行，并且没有触摸输入
        if (Application.isEditor && !Touchscreen.current.touches[0].isInProgress)
        {
            // 模拟触摸开始
            if (mouse.leftButton.wasPressedThisFrame)
            {
                SimulateTouch(mouse.position.ReadValue(), UnityEngine.InputSystem.TouchPhase.Began);
            }
            // 模拟触摸移动
            else if (mouse.leftButton.isPressed)
            {
                SimulateTouch(mouse.position.ReadValue(), UnityEngine.InputSystem.TouchPhase.Moved);
            }
            // 模拟触摸结束
            else if (mouse.leftButton.wasReleasedThisFrame)
            {
                SimulateTouch(mouse.position.ReadValue(), UnityEngine.InputSystem.TouchPhase.Ended);
            }
        }

        // 使用新的输入系统获取移动输入
        Vector2 movement = playerControls.GetMovementInput();
        if (movement.magnitude > 0)
        {
            Vector3 movement3D = new Vector3(movement.x, 0.0f, movement.y);
            HandleMovement(movement3D);
        }
    }

    void SimulateTouch(Vector2 position, UnityEngine.InputSystem.TouchPhase phase)
    {
        // 使用新的输入系统模拟触摸
        var touchState = new TouchState
        {
            phase = phase,
            position = position,
            touchId = 1
        };

        InputSystem.QueueStateEvent(Touchscreen.current, touchState);
    }

    void HandleMovement(Vector3 movement)
    {
        // 在这里处理移动逻辑，例如更新玩家位置
        Debug.Log("Handling movement: " + movement);
        rb.velocity = movement * speed;
    }
}
