using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // ������ UI��ũ��Ʈ�� ����� �̷���
    // �ڵ��� �������� ���뼺�� ���̰�, ���������� ���� �ϱ� ����
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    void Start()
    {
        curValue = startValue;
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amout)
    {
        curValue = Mathf.Min(curValue + amout, maxValue);
    }

    public void Subtract(float amout)
    {
        curValue = Mathf.Max(curValue + amout, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
