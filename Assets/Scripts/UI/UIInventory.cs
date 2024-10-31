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

        // 기존 켜져있는 인벤토리 창을 꺼준다.
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            // slots[i]를 slotPanel에 있는 모든 자식의 ItemSlot으로 바꾼다.
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            // ItemSlot에 있는 인덱스를 i의 값으로 넣어준다.
            slots[i].index = i;
            // ItemSlot에 있는 인벤토리는 현재 스크립트가 된다.
            slots[i].inventory = this;
            // slots를 비워준다.
            slots[i].Clear();
        }
    }

    public void Toggle()
    {
        // 인벤토리 창이 켜져있다면 꺼주고 아니면 켜준다.
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
        // 인벤토리의 켜짐과 꺼짐 상태를 반환해준다.
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        // 플레이어에 있는 아이템데이터를 넣어준다.
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 플레이어에 있는 아이템데이터에서 cnaStack을 찾아 만약 true라면
        // 스택의 정보를 가져와 준다
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                // 슬롯이 null이 아니라면
                // 슬롯의 스택갯수 더해주고 UI를 업데이트 해주며
                // 플레이어에 있는 아이템 정보를 초기화
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();
        // null값이 들어오지 않았다면
        if (emptySlot != null)
        {
            // 슬롯의 아이템은 캐릭터가 가지고 있는 아이템 데이터
            // 가질 수 있는갯수는 1개 뿐이므로 1개만 플러스
            // UI업데이트
            // 플레이어가 가지고있는 아이템 정보 초기화
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
        // 아이템을 버린다면 아이템 프리펩을 소환한다.
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length;i++)
        {
            if (slots[i].item != null)
            {
                // null이 아니라면 아이템 셋팅
                slots[i].Set();
            }
            else
            {
                // null이라면 초기화
                slots[i].Clear();
            }
        }
    }

    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 만약 슬롯에 아이템이 데이터와 같고 아이템을 가지고있는 갯수가 맥스스택 보다 작으면
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                // 아이템 슬롯을 반환해준다.
                return slots[i];
            }
        }
        // 아니라면 null반환
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i< slots.Length; i++)
        {
            // 슬롯의 아이템이 null이라면
            if(slots[i].item == null)
            {
                // 슬롯을 반환해준다.
                return slots[i];
            }
        }
        // 아니라면 null을 반환해준다.
        return null;
    }

    public void SelectItem(int index)
    {
        // 만약 아이템이 없다면 return
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
            // 아이템 데이터에 있는 사용되는 아이템의 정보를 가져옴
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text = selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // 사용가능한 아이템이라면 켜주기
        useButton.SetActive(selectedItem.item.type == ItemType.Consumalble);
        // 장착 가능한 아이템이라면 켜주는데 장착을 하고 있는지 없는지 같이 파악
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        // 버리기 버튼은 항상 활성화
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow()
    {
        // 슬롯에 있는 아이템 정보 초기화
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
                    // 사용되는 아이템의 타입이 헬스라면
                    case ConsumableType.Health:
                        // 플레이어의 체력회복
                        condition.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        // 플레이어의 배고픔 회복
                        condition.Eat(selectedItem.item.consumables[i].value);
                        break;
                }
            }

            RemoveSelctedItem();
        }
    }

    public void OnDropButton()
    {
        // 아이템을 버린다.
        ThrowItem(selectedItem.item);
        // 선택된 아이템의 정보를 초기화해준다.
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        // 아이템의 갯수를 마이너스 해준다.
        selectedItem.quantity--;

        if(selectedItem.quantity <= 0)
        {
            // 만약 0보다 작으면 아이템 정보 초기화
            if (slots[selectedItemIndex].equipped)
            {
                // 만약 장착되어있다면 장착해제
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
        // UI업데이트
        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}

