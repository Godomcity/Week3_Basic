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
            // Capacity가 0보다 작으면 탈출
            if (capacity <= 0) break;

            capacity -= 1;
            // 데이터에서 설정해준 아이템을 hitPoint위로 소환해준다.
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        // Capacity가 0보다 작으면 리소스 게임 오브젝트를 삭제해준다.
        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
