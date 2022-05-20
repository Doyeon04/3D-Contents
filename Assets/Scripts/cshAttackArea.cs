using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshAttackArea : MonoBehaviour
{
    public List<Collider> colliders // collider만 배열 형태로 저장 
        // colliderList안에 데이터를 넣는 변수
    {
        get
        {
            if (0 < colliderList.Count) // Attack Area 안에  공격 가능한 대상의 개수가 0보다 크면 
            {

                // 물체가 들어와 없앴는데 배열 안에는 남아있을 때를 대비. 눈 앞에 사라지면 리스트에서도 없애기 위함 
                // 현재 colliders 리스트에 객체중 null인 것은 제거하여 colliderList에 저장 후 반환
                colliderList.RemoveAll(c => c == null);
            }
            return colliderList;
        }
    }
    private List<Collider> colliderList = new List<Collider>();
    private void OnTriggerEnter(Collider other) // 트리거가 들어오면 실행되는 유니티 기본 함수 
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy")) // 충돌체의 태그가 BreakableObject 또는 Enemy 이면
        {
            colliders.Add(other); // collider 리스트에 오브젝트를 추가 
        }
    }
    private void OnTriggerExit(Collider other) // 트리거가 나갔을 때 실행 
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy"))
        {
            colliders.Remove(other); // collider 리스트에 오브젝트를 제거
        }
    }
}

