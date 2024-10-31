using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 외부에서 플레이어의 정보를 가져올 때 Player 스크립트를 접근하여 정보를 가져온다.
    public PlayerController controller;
    public PlayerCondition condition;

    // 아이템의 정보와 이벤트 생성
    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
