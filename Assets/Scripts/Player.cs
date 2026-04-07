using UnityEngine;

public class Player : MonoBehaviour
{
    // 좌우로 움직이는 속도입니다.
    [SerializeField] private float moveSpeed = 5f;

    // 점프 뛰는 힘
    [SerializeField] private float jumpForce = 10f;
    
    // 떨어지는 속도
    [SerializeField] private float gravity = 2.5f;
    
    // 물리 엔진을 담당하는 도구
    private Rigidbody2D rigidBody;
    
    // 그림(Sprite)을 담당하는 도구
    private SpriteRenderer spriteRenderer;

    // 현재 땅에 닿아있는지(점프 가능한 상태인지) 여부를 저장할 상자입니다.
    private bool isGrounded = true;

    // 게임이 일시정지 상태인지 기억하는 상자입니다. (처음엔 게임이 돌아가야 하니 false)
    private bool isPaused = false;

    // 캐릭터의 원래 크기를 기억해 둘 상자입니다.
    private Vector3 originalScale;
    // 캐릭터의 원래 이동 속도와 점프력을 기억해 둘 상자입니다.
    private float baseMoveSpeed;
    private float baseJumpForce;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity;
        
        // 내 오브젝트에서 그림 그리는 도구를 찾아서 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작할 때 내 캐릭터의 각종 원래 수치들을 미리 기억해 둡니다.
        originalScale = transform.localScale;
        baseMoveSpeed = moveSpeed;
        baseJumpForce = jumpForce;
    }

    private void Update()
    {
        // -----------------------------
        // 0. 게임 일시정지 (S 키)
        // -----------------------------
        // S 키를 누를 때마다 일시정지와 재생 상태가 번갈아가며 바뀝니다.
        if (Input.GetKeyDown(KeyCode.S))
        {
            isPaused = !isPaused; // 상태 뒤집기 (true -> false, false -> true)

            if (isPaused)
            {
                Time.timeScale = 0f; // 우주의 시간 흐름을 완전히 멈춥니다! 🪐
            }
            else
            {
                Time.timeScale = 1f; // 시간 흐름을 1배속(정상 속도)으로 되돌립니다.
            }
        }

        // 일시정지 중일 때는 아래에 있는 모든 코드가 무시되도록, 여기서 튕겨냅니다(return).
        if (isPaused) return;


        // -----------------------------
        // 1. 좌우 이동 (A, D 키)
        // -----------------------------
        float horizontalSpeed = 0f; // 기본은 안 움직임(0)

        // D를 누르는 동안엔 오른쪽(양수)으로 속도를 냅니다.
        if (Input.GetKey(KeyCode.D))
        {
            horizontalSpeed = moveSpeed;
            spriteRenderer.flipX = false; // 기본 방향(오른쪽) 유지
        }
        // A를 누르는 동안엔 왼쪽(음수)으로 속도를 냅니다.
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalSpeed = -moveSpeed; // 왼쪽이니까 마이너스!
            spriteRenderer.flipX = true; // 그림(Sprite)을 왼쪽으로 휙! 뒤집기
        }

        // 왼쪽 시프트(LeftShift) 혹은 오른쪽 시프트(RightShift) 키를 누르고 있다면 속도를 1.5배 뻥튀기 해줍니다. (달리기!)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            horizontalSpeed *= 1.5f; 
        }
        // 반대로 왼쪽 컨트롤(LeftControl)이나 오른쪽 컨트롤(RightControl)을 누르고 있다면 속도를 절반(0.5배)으로 줄입니다. (살금살금 걷기!)
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            horizontalSpeed *= 0.5f;
        }

        // 적용: 떨어지거나 뛰는 위아래 속도(velocity.y)는 그대로 두고, 좌우 속도 부분만 바꿔줍니다.
        rigidBody.linearVelocity = new Vector2(horizontalSpeed, rigidBody.linearVelocity.y);


        // -----------------------------
        // 2. 점프 (스페이스바)
        // -----------------------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    // -----------------------------
        // 3. 캐릭터 크기 변경 (Q, W, E 키)
        // -----------------------------
        // Q 키를 누르면 크기가 2배(200%)로 엄청나게 커집니다!
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.localScale = originalScale * 2f;
            jumpForce = baseJumpForce * 1.5f; // 몸집이 커지면 다리 힘도 세져서 점프력이 1.5배가 됩니다!
            moveSpeed = baseMoveSpeed; // 대신 이동 속도는 기본 속도로 유지됩니다.
        }
        // W 키를 누르면 다시 원래 크기(100%)로 돌아옵니다.
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localScale = originalScale;
            jumpForce = baseJumpForce; // 점프력 원상복구
            moveSpeed = baseMoveSpeed; // 이동 속도 원상복구
        }
        // E 키를 누르면 크기가 절반(50%)으로 작아집니다.
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.localScale = originalScale * 0.5f;
            moveSpeed = baseMoveSpeed * 1.5f; // 몸집이 작아진 만큼 가벼워져서 이동 속도가 1.5배 빨라집니다!
            jumpForce = baseJumpForce; // 점프력 원상복구
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }
}