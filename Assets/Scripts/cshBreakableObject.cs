using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBreakableObject : MonoBehaviour
{
    public GameObject destroyEffectPrefab;

    public void PlayEffect() // �� �Լ��� ȣ��Ǹ� 
    {
        Instantiate(destroyEffectPrefab, transform.localPosition, Quaternion.identity); // destroyEffectPrefab�� ���� ĳ���Ͱ� �����ִ� �����ǿ� �����ض�
    }
}

