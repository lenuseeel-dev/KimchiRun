using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 7f; // 점프하는 힘

    private Rigidbody2D rigid;
    private bool isGrounded; // 바닥에 닿아있는지(점프 가능한지) 기억하는 변수

    private void Start()
    {
        // 시작할 때(Start), Player 게임 오브젝트에 붙어있는 Rigidbody2D 컴포넌트를 찾아서 가져옵니다.
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 스페이스바를 눌렀고(AND), 바닥에 닿아있을 때만(isGrounded == true) 점프!
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 위쪽 방향(Vector2.up)으로 jumpForce만큼 순간적인 힘(Impulse)을 줍니다.
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            // 점프를 했으니 이제 바닥에서 떨어진 상태가 됩니다.
            isGrounded = false; 
        }
    }

    // 다른 물리적인 물체와 쾅! 하고 부딪혔을 때 유니티가 자동으로 실행해주는 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 우리가 부딪힌 상대방(collision.gameObject)의 이름표(Tag)가 "Ground"라면?
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 바닥에 닿았다고 기억합니다. 이제 다시 점프할 수 있어요!
            isGrounded = true;
        }
    }
}
