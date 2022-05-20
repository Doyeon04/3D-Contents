using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshButton : MonoBehaviour
{
    public Button btnJump;
    public Button btnAttack;
    public cshPlayerController sPlayer;
    void Start()
    {
        btnJump.gameObject.SetActive(true); // jump ��ư Ȱ��ȭ 
        btnJump.onClick.RemoveAllListeners(); // �ϴ� ��ư�� Ŭ���� ���õ� ��� �̺�Ʈ ���� 
        btnJump.onClick.AddListener(OnClickJumpButton); // �Լ��� �̺�Ʈ �����ʷ� �߰� 
        btnAttack.gameObject.SetActive(false); // attack ��ư ��Ȱ��ȭ 
        btnAttack.onClick.RemoveAllListeners();
        btnAttack.onClick.AddListener(OnClickAttackButton); 
    }
    void Update()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        bool canAttack = sPlayer.CanAttack(); // player ��ũ��Ʈ�� canAttack �Լ� ȣ��
       
        btnAttack.gameObject.SetActive(canAttack);  // ���� ������ ����� ������  attack ��ư Ȱ��ȭ 
        btnJump.gameObject.SetActive(!canAttack); // ������ jump ��ư Ȱ��ȭ 
    }


    private void OnClickJumpButton()
    {
        sPlayer.OnVirtualPadJump(); 
    }

    private void OnClickAttackButton()
    {
        sPlayer.OnVirtualPadAttack();
    }
}

