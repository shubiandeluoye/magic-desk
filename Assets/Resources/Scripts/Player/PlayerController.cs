using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private bool isStunned;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        // Handle keyboard input for testing
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 keyboardInput = new Vector2(horizontal, vertical);
        
        if (keyboardInput.magnitude > 0.1f)
        {
            // Normalize keyboard input to match joystick behavior
            OnMovementInput(keyboardInput.normalized);
            
            // Log movement for debugging
            Debug.Log($"Keyboard Input: {keyboardInput.normalized}, IsMine: {photonView.IsMine}");
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || isStunned) return;
        
        // 应用移动
        rb.velocity = moveInput * moveSpeed;
    }

    // 由MovementJoystick调用的移动输入方法
    public void OnMovementInput(Vector2 input)
    {
        if (!photonView.IsMine) return;
        moveInput = input;
    }

    // 眩晕效果
    [PunRPC]
    public void ApplyStun(float duration)
    {
        if (!photonView.IsMine) return;
        
        isStunned = true;
        rb.velocity = Vector2.zero;
        Invoke(nameof(RemoveStun), duration);
    }

    private void RemoveStun()
    {
        isStunned = false;
    }

    // 网络同步玩家位置
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(isStunned);
        }
        else
        {
            rb.position = (Vector2)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();
            isStunned = (bool)stream.ReceiveNext();
        }
    }
}
