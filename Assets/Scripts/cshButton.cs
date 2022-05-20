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
        btnJump.gameObject.SetActive(true); // jump 버튼 활성화 
        btnJump.onClick.RemoveAllListeners(); // 일단 버튼의 클릭과 관련된 모든 이벤트 제거 
        btnJump.onClick.AddListener(OnClickJumpButton); // 함수를 이벤트 리스너로 추가 
        btnAttack.gameObject.SetActive(false); // attack 버튼 비활성화 
        btnAttack.onClick.RemoveAllListeners();
        btnAttack.onClick.AddListener(OnClickAttackButton); 
    }
    void Update()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        bool canAttack = sPlayer.CanAttack(); // player 스크립트의 canAttack 함수 호출
       
        btnAttack.gameObject.SetActive(canAttack);  // 공격 가능한 대상이 있으면  attack 버튼 활성화 
        btnJump.gameObject.SetActive(!canAttack); // 없으면 jump 버튼 활성화 
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

