using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �ܺο��� �÷��̾��� ������ ������ �� Player ��ũ��Ʈ�� �����Ͽ� ������ �����´�.

    public PlayerController controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    }
}
