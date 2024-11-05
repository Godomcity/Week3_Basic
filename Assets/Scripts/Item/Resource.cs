using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            // Capacity�� 0���� ������ Ż��
            if (capacity <= 0) break;

            capacity -= 1;
            // �����Ϳ��� �������� �������� hitPoint���� ��ȯ���ش�.
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        // Capacity�� 0���� ������ ���ҽ� ���� ������Ʈ�� �������ش�.
        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
