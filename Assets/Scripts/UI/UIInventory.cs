using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;

        CharacterManager.Instance.Player.addItem += AddItem;

        // ���� �����ִ� �κ��丮 â�� ���ش�.
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            // slots[i]�� slotPanel�� �ִ� ��� �ڽ��� ItemSlot���� �ٲ۴�.
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            // ItemSlot�� �ִ� �ε����� i�� ������ �־��ش�.
            slots[i].index = i;
            // ItemSlot�� �ִ� �κ��丮�� ���� ��ũ��Ʈ�� �ȴ�.
            slots[i].inventory = this;
            // slots�� ����ش�.
            slots[i].Clear();
        }
    }

    public void Toggle()
    {
        // �κ��丮 â�� �����ִٸ� ���ְ� �ƴϸ� ���ش�.
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        // �κ��丮�� ������ ���� ���¸� ��ȯ���ش�.
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        // �÷��̾ �ִ� �����۵����͸� �־��ش�.
        ItemData data = CharacterManager.Instance.Player.itemData;

        // �÷��̾ �ִ� �����۵����Ϳ��� cnaStack�� ã�� ���� true���
        // ������ ������ ������ �ش�
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                // ������ null�� �ƴ϶��
                // ������ ���ð��� �����ְ� UI�� ������Ʈ ���ָ�
                // �÷��̾ �ִ� ������ ������ �ʱ�ȭ
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();
        // null���� ������ �ʾҴٸ�
        if (emptySlot != null)
        {
            // ������ �������� ĳ���Ͱ� ������ �ִ� ������ ������
            // ���� �� �ִ°����� 1�� ���̹Ƿ� 1���� �÷���
            // UI������Ʈ
            // �÷��̾ �������ִ� ������ ���� �ʱ�ȭ
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    private void ThrowItem(ItemData data)
    {
        // �������� �����ٸ� ������ �������� ��ȯ�Ѵ�.
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length;i++)
        {
            if (slots[i].item != null)
            {
                // null�� �ƴ϶�� ������ ����
                slots[i].Set();
            }
            else
            {
                // null�̶�� �ʱ�ȭ
                slots[i].Clear();
            }
        }
    }

    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���� ���Կ� �������� �����Ϳ� ���� �������� �������ִ� ������ �ƽ����� ���� ������
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                // ������ ������ ��ȯ���ش�.
                return slots[i];
            }
        }
        // �ƴ϶�� null��ȯ
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i< slots.Length; i++)
        {
            // ������ �������� null�̶��
            if(slots[i].item == null)
            {
                // ������ ��ȯ���ش�.
                return slots[i];
            }
        }
        // �ƴ϶�� null�� ��ȯ���ش�.
        return null;
    }

    public void SelectItem(int index)
    {
        // ���� �������� ���ٸ� return
        if (slots[index].item == null)
        {
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            // ������ �����Ϳ� �ִ� ���Ǵ� �������� ������ ������
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text = selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // ��밡���� �������̶�� ���ֱ�
        useButton.SetActive(selectedItem.item.type == ItemType.Consumalble);
        // ���� ������ �������̶�� ���ִµ� ������ �ϰ� �ִ��� ������ ���� �ľ�
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        // ������ ��ư�� �׻� Ȱ��ȭ
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow()
    {
        // ���Կ� �ִ� ������ ���� �ʱ�ȭ
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUesButton()
    {
        if (selectedItem.item.type == ItemType.Consumalble)
        {
            for(int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch(selectedItem.item.consumables[i].type)
                {
                    // ���Ǵ� �������� Ÿ���� �ｺ���
                    case ConsumableType.Health:
                        // �÷��̾��� ü��ȸ��
                        condition.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        // �÷��̾��� ����� ȸ��
                        condition.Eat(selectedItem.item.consumables[i].value);
                        break;
                }
            }

            RemoveSelctedItem();
        }
    }

    public void OnDropButton()
    {
        // �������� ������.
        ThrowItem(selectedItem.item);
        // ���õ� �������� ������ �ʱ�ȭ���ش�.
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        // �������� ������ ���̳ʽ� ���ش�.
        selectedItem.quantity--;

        if(selectedItem.quantity <= 0)
        {
            // ���� 0���� ������ ������ ���� �ʱ�ȭ
            if (slots[selectedItemIndex].equipped)
            {
                // ���� �����Ǿ��ִٸ� ��������
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
        // UI������Ʈ
        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}

