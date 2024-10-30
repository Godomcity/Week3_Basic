using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    public Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        // �ƿ������� ���۳�Ʈ�� �����´�.
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        // �������� �߰����� �� ������ ���Կ� ������Ʈ�� ���ְ� ������ ������ �ִ� ���������� �������ش�.
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        // ������ ������ 1���� ũ�� quantity ��ŭ �ؽ�Ʈ�� �����ְ� �ƴϸ� �� �ؽ�Ʈ���Ѵ�.
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }
}
