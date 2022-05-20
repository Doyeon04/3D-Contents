using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshAttackArea : MonoBehaviour
{
    public List<Collider> colliders // collider�� �迭 ���·� ���� 
        // colliderList�ȿ� �����͸� �ִ� ����
    {
        get
        {
            if (0 < colliderList.Count) // Attack Area �ȿ�  ���� ������ ����� ������ 0���� ũ�� 
            {

                // ��ü�� ���� ���ݴµ� �迭 �ȿ��� �������� ���� ���. �� �տ� ������� ����Ʈ������ ���ֱ� ���� 
                // ���� colliders ����Ʈ�� ��ü�� null�� ���� �����Ͽ� colliderList�� ���� �� ��ȯ
                colliderList.RemoveAll(c => c == null);
            }
            return colliderList;
        }
    }
    private List<Collider> colliderList = new List<Collider>();
    private void OnTriggerEnter(Collider other) // Ʈ���Ű� ������ ����Ǵ� ����Ƽ �⺻ �Լ� 
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy")) // �浹ü�� �±װ� BreakableObject �Ǵ� Enemy �̸�
        {
            colliders.Add(other); // collider ����Ʈ�� ������Ʈ�� �߰� 
        }
    }
    private void OnTriggerExit(Collider other) // Ʈ���Ű� ������ �� ���� 
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy"))
        {
            colliders.Remove(other); // collider ����Ʈ�� ������Ʈ�� ����
        }
    }
}

