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
        // 아웃라인의 컴퍼넌트를 가져온다.
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        // 아이템이 추가됬을 때 아이템 슬롯에 오브젝트를 켜주고 아이템 정보에 있는 아이콘으로 설정해준다.
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        // 아이템 갯수가 1보다 크면 quantity 만큼 텍스트로 보여주고 아니면 빈 텍스트로한다.
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }
}
