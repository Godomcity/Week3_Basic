using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    // �÷��̾ ���س��� Ű�� ������ OnAttackInput�� ����
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }

    // ���� ������ ��

    public void EquipNew(ItemData data)
    {
        // ������ �ִ� ���⸦ �������� ���ش�.
        // ���� �����ϴ� �������� �������� �������ְ� �����ۿ� �ִ� Equip�� �����´�.
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
       if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
