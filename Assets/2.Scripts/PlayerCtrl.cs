using UnityEngine;

/// <summary>
/// 플레이어 컨트롤(걷기, 달리기, 점프) 제어하는 스크립트입니다.
/// </summary>
public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 12f; // 걷기의 최대 속도          
    [SerializeField] private float maxRunSpeed = 20f; // 달리기의 최대 속도
    [SerializeField] private float runForce = 50f; // 달리기에 가해지는 힘
    [HideInInspector] public Animator anim; 
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D box;
    [HideInInspector] public Transform groundCheck; // 바닥을 인식하는 Transform
    
    private bool dirRight = true; // 방향 전환 플래그
    public bool grounded = false; // 땅 밟고있는지 여부
    public GameEnding gEnding;
    public float moveForce = 45f; // 걷기에 가해지는 힘
    public float jumpForce = 1200f; // 점프에 가해지는 힘
    private float jumpDamp = 0.5f; // 점프 감속
    private float flipDamp = 0.5f; // 방향 전환 시 감속

    void Awake()
    {
        anim = GetComponent<Animator>();
        groundCheck = transform.GetChild(1);
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpControll();
        MoveControll();
    }

    void JumpControll()
    {
        if (groundCheck == null) return; 

        // groundCheck까지 선을 그어 Ground 레이어와 충돌 여부 확인
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("isJumping", !grounded); // 땅을 밟고 있지 않는 상태 -> 점프 애니메이션
        if(gEnding.isBlockCtrl) return; // 엔딩 진입 시 컨트롤 차단

        if(Input.GetButtonDown("Jump") && grounded)
            rb.AddForce(new Vector2(0f, jumpForce));
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 )
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpDamp); // 점프중 버튼을 떼면 감속
    }
    void MoveControll()
    {
        if(gEnding.isBlockCtrl) return;
        float h = Input.GetAxis("Horizontal");
      
        anim.SetFloat("Speed", Mathf.Abs(h));
        if (h * rb.velocity.x <= maxSpeed) // 최대 속도 이하일 때 이동 힘 적용
        {
            rb.AddForce(Vector2.right * h * moveForce);

            if (Input.GetKey(KeyCode.Z) && h !=0) // Z키 입력 시 추가 힘 적용
                rb.AddForce(Vector2.right * h * runForce);
        }
       

        if (Mathf.Abs(rb.velocity.x) >= maxSpeed) // 최대 속도 초과 시 속도 고정
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            if (Input.GetKey(KeyCode.Z) &&  h != 0) // 달리기 최대 속도 고정
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxRunSpeed, rb.velocity.y); 
        }
      
        // 이동 방향과 바라보는 방향이 다르면 방향 전환
        if (h > 0 && !dirRight || h < 0 && dirRight)
            Flip();
    }

    // 방향 전환하는 함수
    void Flip()
    {
        dirRight = !dirRight;

        anim.SetTrigger("Flip");
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        rb.velocity = new Vector2(rb.velocity.x * flipDamp, rb.velocity.y); // 방향 전환 시 속도 감소
    }
}
