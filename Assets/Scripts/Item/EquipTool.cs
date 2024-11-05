using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponentInChildren<Animator>();
    }

    // equip에서 사용된 메서드를 재정의 하여 사용
    public override void OnAttackInput()
    {
        // attacking를 체크함으로 한번만 공격이 가능하게 설정
        if(!attacking)
        {
            // 애니메이션 실행 후 다시 공격할 수 있도록 attacking를 변경해줌
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
            if(doesDealDamage && hit.collider.TryGetComponent(out IDamgable damgable))
            {
                damgable.TakePhysicalDamage(damage);
            }
        }
    }
}
