using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable
{
    // 인터페이스의 특징
    // 클래스와 구조체가 특정 기능을 구현하도록 강제하면서 코드의 결합도를 낮추고 유연성을 높이는데 유용
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamgable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get {  return uiCondition.stamina; } }

    private float noHungerHealthDecay;
    public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue + Time.deltaTime); 
        
        if (hunger.curValue < 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue < 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        // 피해량을 입력받아 체력을 감소하고 피해를 받음을 알리는 이벤트를 실행
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
