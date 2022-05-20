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

    private cshAttackArea m_attackArea = null;


    void Start()
    {
        m_animator = GetComponent<Animator>(); // ���� ĳ���Ͱ� ������ �ִ� �ִϸ����� �Ӽ� ������ 
        m_attackArea = GetComponentInChildren<cshAttackArea>(); // �ڽ��� ������ �ִ� attackArea��� �Ӽ��� ������ 
    


    }

    void Update()
    {
        PlayerMove(); // �̵��ϴ� �Լ� ȣ��
        m_animator.SetBool("Jump", !m_isGrounded); // Jump��� �ϴ� bool parameter�� m_isGrounded�� �ݴ�� 
    }

    public bool CanAttack()
    {
        return 0 < m_attackArea.colliders.Count; // attackArea�� collider ����Ʈ�� ī��Ʈ�� 0���� ũ�� (�����Ǿȿ� ������ �� �ִ� ����� �ϳ��� ������) true ��ȯ 
    }



    public void OnVirtualPadJump()
    { // ���� ��ư ������ ����

        if (this == null) { return; }
        const float rayDistance = 0.2f;
        var ray = new Ray(transform.localPosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down);
        if (Physics.Raycast(ray, rayDistance)) // ���� ��ġ���� 0.2��ŭ ������ ��� ������ �������� �����ٸ� ������ �ִ� ��
        {
            // �� ���� ������
            m_jumpOn = true; // ������ ���� 
        }
    }

    public void OnVirtualPadAttack() // attack ��ư ������ ����
    {
        if (this == null) { return; }

        m_animator.SetTrigger("Attack"); // ���� �������� �ٲ� 

        Vector3 center = Vector3.zero;
        int cnt = m_attackArea.colliders.Count; // attackArea�� �ִ� �浹������ ������Ʈ ���� ��
        int cntBreak = 0; // ���� �μ� �� �ִ� ������Ʈ 

        for (int i = 0; i < m_attackArea.colliders.Count; ++i)
        {
            var collider = m_attackArea.colliders[i]; // attackArea�� �ִ� ������Ʈ �ϳ��� �޾ƿ� 
            center += collider.transform.localPosition; // ������Ʈ�� ��ġ ���� 

            var obj = collider.GetComponent<cshBreakableObject>(); // ������Ʈ�� breakableObject��� 
            if (obj != null)
            {
                obj.PlayEffect(); // �ν����� ȿ���� �Բ� �������
                cntBreak++;
            }
            var enemy = collider.GetComponent<cshEnemyController>(); // ������Ʈ�� enemy���
            if (enemy != null)
            {
                enemy.Damage();
                if (enemy.GetHP() <= 0) m_attackArea.colliders.Clear(); // ���� hp�� 0�� �Ǹ� ������Ʈ ������� 
            }
            else
            { // breakableObject���
                Destroy(collider.gameObject); // �浹�� object�� destroy
            }
        }
        if (cntBreak > 0) m_attackArea.colliders.Clear(); // �ν��� �� �ִ� ������Ʈ�� 0���� ũ�� ����Ʈ �����

        center /= cnt; // ������Ų ��ǥ�� ������ ������ �� ����� ���� �� 
        center.y = transform.localPosition.y; // y�� ĳ������ ��ġ��(���� �ٶ󺸸� ���� �ǹǷ�)
        transform.LookAt(center); // x, z �� ����� ���� �Ĵٺ�
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

