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
        // List�� ������Ʈ�� �ִٸ� �������� �������� �ش�
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            // List[i]�� �������� �ش�.
            things[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������Ʈ�� ���� ��
        // other�� ���� ������Ʈ ���۳�Ʈ���� IDamable�� ã�� �����´�.
        if(other.gameObject.TryGetComponent(out IDamgable damagable))
        {
            // List�� ������ IDamable�� �߰�
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������Ʈ�� ������ ��
        // other�� ���� ������Ʈ ���۳�Ʈ���� IDamable�� ã�� �����´�.
        if (other.gameObject.TryGetComponent(out IDamgable damagable))
        {
            // List�� ������ IDamable�� ����
            things.Remove(damagable);
        }
    }
}
