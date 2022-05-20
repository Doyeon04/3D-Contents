using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBreakableObject : MonoBehaviour
{
    public GameObject destroyEffectPrefab;

    public void PlayEffect() // 이 함수가 호출되면 
    {
        Instantiate(destroyEffectPrefab, transform.localPosition, Quaternion.identity); // destroyEffectPrefab을 현재 캐릭터가 갖고있는 포지션에 복제해라
    }
}

