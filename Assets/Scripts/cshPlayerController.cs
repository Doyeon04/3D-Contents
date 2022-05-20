using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPlayerController : MonoBehaviour
{
    private Animator m_animator; // animation 속성을 담고있는 animator 컴포넌트를 불러올 변수
    private Vector3 m_velocity; // 3차원 벡터. 캐릭터가 이동될 방향
    private bool m_isGrounded = true; // 현재 캐릭터가 땅 위에 있는지 공중에 있는지 판단하는 변수
    private bool m_jumpOn = false; // 점프 버튼이 눌리면 true로

    public cshJoystick sJoystick; // background가 가지고 있는 스크립트(가상 패드를 x,y축으로 얼만큼 이동시키고 있는지 가져오기 위함) 
    public float m_moveSpeed = 2.0f; // 캐릭터 이동 속도
    public float m_jumpForce = 5.0f; // 점프 높이 

    private cshAttackArea m_attackArea = null;


    void Start()
    {
        m_animator = GetComponent<Animator>(); // 현재 캐릭터가 가지고 있는 애니메이터 속성 가져옴 
        m_attackArea = GetComponentInChildren<cshAttackArea>(); // 자식이 가지고 있는 attackArea라는 속성을 가져옴 
    


    }

    void Update()
    {
        PlayerMove(); // 이동하는 함수 호출
        m_animator.SetBool("Jump", !m_isGrounded); // Jump라고 하는 bool parameter를 m_isGrounded의 반대로 
    }

    public bool CanAttack()
    {
        return 0 < m_attackArea.colliders.Count; // attackArea의 collider 리스트의 카운트가 0보다 크면 (사정권안에 공격할 수 있는 대상이 하나라도 있으면) true 반환 
    }



    public void OnVirtualPadJump()
    { // 점프 버튼 누르면 실행

        if (this == null) { return; }
        const float rayDistance = 0.2f;
        var ray = new Ray(transform.localPosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down);
        if (Physics.Raycast(ray, rayDistance)) // 현재 위치에서 0.2만큼 광선을 쏘고 광선이 누군가와 만난다면 땅위에 있는 거
        {
            // 땅 위에 있으면
            m_jumpOn = true; // 점프가 가능 
        }
    }

    public void OnVirtualPadAttack() // attack 버튼 누르면 실행
    {
        if (this == null) { return; }

        m_animator.SetTrigger("Attack"); // 공격 동작으로 바꿈 

        Vector3 center = Vector3.zero;
        int cnt = m_attackArea.colliders.Count; // attackArea에 있는 충돌가능한 오브젝트 개수 셈
        int cntBreak = 0; // 때려 부술 수 있는 오브젝트 

        for (int i = 0; i < m_attackArea.colliders.Count; ++i)
        {
            var collider = m_attackArea.colliders[i]; // attackArea에 있는 오브젝트 하나씩 받아옴 
            center += collider.transform.localPosition; // 오브젝트의 위치 누적 

            var obj = collider.GetComponent<cshBreakableObject>(); // 오브젝트가 breakableObject라면 
            if (obj != null)
            {
                obj.PlayEffect(); // 부숴지는 효과와 함께 사라지게
                cntBreak++;
            }
            var enemy = collider.GetComponent<cshEnemyController>(); // 오브젝트가 enemy라면
            if (enemy != null)
            {
                enemy.Damage();
                if (enemy.GetHP() <= 0) m_attackArea.colliders.Clear(); // 현재 hp가 0이 되면 오브젝트 사라지게 
            }
            else
            { // breakableObject라면
                Destroy(collider.gameObject); // 충돌된 object를 destroy
            }
        }
        if (cntBreak > 0) m_attackArea.colliders.Clear(); // 부숴질 수 있는 오브젝트가 0보다 크면 리스트 비워줌

        center /= cnt; // 누적시킨 좌표를 개수로 나누면 한 가운데를 보게 됨 
        center.y = transform.localPosition.y; // y는 캐릭터의 위치로(위를 바라보면 눕게 되므로)
        transform.LookAt(center); // x, z 만 평균을 구해 쳐다봄
    }




    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f; // regidbody 안주기 때문에 중력 직접 구현 

        if (controller.isGrounded) // characterController에는 내부적으로 현재 캐릭터가 땅 위에 있는지 계산에 변수에 넣어줌 
            // 현재 캐릭터 컨트롤러가 지면 위에 있다면 
        {
            float h = sJoystick.GetHorizontalValue(); // joystick에서 x축으로 얼마만큼 움직였는지 
            float v = sJoystick.GetVerticalValue(); // joystick에서 y축으로 얼마만큼 움직였는지
            m_velocity = new Vector3(h, 0, v); // x, z 축 
            m_velocity = m_velocity.normalized;

            m_animator.SetFloat("Move", m_velocity.magnitude); // magnitude: 길이. (1,0,1)이면 루트 2

           // move값이 0보다 커지면 걷는 값으로 

            if (m_jumpOn) 
            {
                m_velocity.y = m_jumpForce; // 키가 눌리는 딱 한순간 y축(위)으로 5만큼 
                m_jumpOn = false; // 다시 false로
            }
            else if (m_velocity.magnitude > 0.5)
            {
                transform.LookAt(transform.position + m_velocity); // 캐릭터가 이동하는 방향으로 쳐다보게 
            }
        }

        m_velocity.y -= gravity * Time.deltaTime; // velocity.y를 gravity만큼 감소 
        // ex) 점프중이면 공중에 뜸. 다시 -20만큼 일정속도로 감소해 땅에 떨어질 수 있게 
        // rigidbody 없기 때문에 필요 
        controller.Move(m_velocity * m_moveSpeed * Time.deltaTime); // charactor controller에 있는 move함수
        // 현재 지정한 방향, 크기, 플랫폼에 상관없이 캐릭터가 move

        m_isGrounded = controller.isGrounded; // 캐릭터가 땅 위에 있으면 true, 공중에 있으면 false로 
    }

}

