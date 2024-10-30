using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    // �ڿ��� ������ ������ Ÿ��
    Resource,
    // ������ �� �ִ� ������ Ÿ��
    Equipable,
    // ���� �� �ִ� ������ Ÿ��
    Consumalble
}

public enum ConsumableType
{
    // ������ ������� ä���ִ� ������ Ÿ��
    Hunger,
    // ������ ü���� ä���ִ� ������ Ÿ��
    Health
}

[System.Serializable]
public class ItemDataConsumable
{
    // ����� �Ǵ� ü���� �󸶳� ä���ִ���
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]

public class ItemData : ScriptableObject
{
    // ������ ����
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    // �������� �ߺ����� ������ ���� �� �ִ��� üũ �� �󸶳� ������ �������� ���� ����
    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    // �Դ� �������� ����
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    // ������ �������� ������ ����
    [Header("Equip")]
    public GameObject equipPrefab;
}
