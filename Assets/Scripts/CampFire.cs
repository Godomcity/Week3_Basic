using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    private List<IDamgable> things = new List<IDamgable>();

    private void Start()
    {
        // List에 오브젝트가 있다면 지속적인 데미지를 준다
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            // List[i]에 데미지를 준다.
            things[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 오브젝트에 들어갔을 때
        // other의 게임 오브젝트 컴퍼넌트에서 IDamable를 찾아 가져온다.
        if(other.gameObject.TryGetComponent(out IDamgable damagable))
        {
            // List에 가져온 IDamable를 추가
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 오브젝트에 나갔을 때
        // other의 게임 오브젝트 컴퍼넌트에서 IDamable를 찾아 가져온다.
        if (other.gameObject.TryGetComponent(out IDamgable damagable))
        {
            // List에 가져온 IDamable를 삭제
            things.Remove(damagable);
        }
    }
}
