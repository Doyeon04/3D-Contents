using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPlayerController : MonoBehaviour
{
    private Animator m_animator; // animation �Ӽ��� ����ִ� animator ������Ʈ�� �ҷ��� ����
    private Vector3 m_velocity; // 3���� ����. ĳ���Ͱ� �̵��� ����
    private bool m_isGrounded = true; // ���� ĳ���Ͱ� �� ���� �ִ��� ���߿� �ִ��� �Ǵ��ϴ� ����
    private bool m_jumpOn = false; // ���� ��ư�� ������ true��

    public cshJoystick sJoystick; // background�� ������ �ִ� ��ũ��Ʈ(���� �е带 x,y������ ��ŭ �̵���Ű�� �ִ��� �������� ����) 
    public float m_moveSpeed = 2.0f; // ĳ���� �̵� �ӵ�
    public float m_jumpForce = 5.0f; // ���� ���� 

    void Start()
    {
        m_animator = GetComponent<Animator>(); // ���� ĳ���Ͱ� ������ �ִ� �ִϸ����� �Ӽ� ������ 
    }

    void Update()
    {
        PlayerMove(); // �̵��ϴ� �Լ� ȣ��
        m_animator.SetBool("Jump", !m_isGrounded); // Jump��� �ϴ� bool parameter�� m_isGrounded�� �ݴ�� 
    }

    public void OnVirtualPadJump()
    {

        if (this == null) { return; }
        const float rayDistance = 0.2f;
        var ray = new Ray(transform.localPosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down);
        if (Physics.Raycast(ray, rayDistance))
        {
            m_jumpOn = true;
        }
    }


    private void PlayerMove()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float gravity = 20.0f; // regidbody ���ֱ� ������ �߷� ���� ���� 

        if (controller.isGrounded) // characterController���� ���������� ���� ĳ���Ͱ� �� ���� �ִ��� ��꿡 ������ �־��� 
            // ���� ĳ���� ��Ʈ�ѷ��� ���� ���� �ִٸ� 
        {
            float h = sJoystick.GetHorizontalValue(); // joystick���� x������ �󸶸�ŭ ���������� 
            float v = sJoystick.GetVerticalValue(); // joystick���� y������ �󸶸�ŭ ����������
            m_velocity = new Vector3(h, 0, v); // x, z �� 
            m_velocity = m_velocity.normalized;

            m_animator.SetFloat("Move", m_velocity.magnitude); // magnitude: ����. (1,0,1)�̸� ��Ʈ 2

           // move���� 0���� Ŀ���� �ȴ� ������ 

            if (m_jumpOn)
            {
                m_velocity.y = m_jumpForce; // Ű�� ������ �� �Ѽ��� y��(��)���� 5��ŭ 
                m_jumpOn = false; // �ٽ� false��
            }
            else if (m_velocity.magnitude > 0.5)
            {
                transform.LookAt(transform.position + m_velocity); // ĳ���Ͱ� �̵��ϴ� �������� �Ĵٺ��� 
            }
        }

        m_velocity.y -= gravity * Time.deltaTime; // velocity.y�� gravity��ŭ ���� 
        // ex) �������̸� ���߿� ��. �ٽ� -20��ŭ �����ӵ��� ������ ���� ������ �� �ְ� 
        // rigidbody ���� ������ �ʿ� 
        controller.Move(m_velocity * m_moveSpeed * Time.deltaTime); // charactor controller�� �ִ� move�Լ�
        // ���� ������ ����, ũ��, �÷����� ������� ĳ���Ͱ� move

        m_isGrounded = controller.isGrounded; // ĳ���Ͱ� �� ���� ������ true, ���߿� ������ false�� 
    }

}

