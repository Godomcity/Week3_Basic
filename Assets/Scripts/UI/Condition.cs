using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // 별도의 UI스크립트를 만드는 이류는
    // 코드의 가독성과 재사용성을 높이고, 유지보수를 쉽게 하기 위해
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
