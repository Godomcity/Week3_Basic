using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    // 자원이 나오는 아이템 타입
    Resource,
    // 장착할 수 있는 아이템 타입
    Equipable,
    // 먹을 수 있는 아이템 타입
    Consumalble
}

public enum ConsumableType
{
    // 먹으면 배고픔을 채워주는 아이템 타입
    Hunger,
    // 먹으면 체력을 채워주는 아이템 타입
    Health
}

[System.Serializable]
public class ItemDataConsumable
{
    // 배고픈 또는 체력을 얼마나 채워주는지
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]

public class ItemData : ScriptableObject
{
    // 아이템 정보
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    // 아이템을 중복으로 가지고 있을 수 있는지 체크 및 얼마나 가지고 있을지에 대한 정보
    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    // 먹는 아이템의 정보
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    // 장착한 아이템의 프리펩 정보
    [Header("Equip")]
    public GameObject equipPrefab;
}
