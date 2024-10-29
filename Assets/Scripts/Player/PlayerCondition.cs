using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable
{
    // �������̽��� Ư¡
    // Ŭ������ ����ü�� Ư�� ����� �����ϵ��� �����ϸ鼭 �ڵ��� ���յ��� ���߰� �������� ���̴µ� ����
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
        Debug.Log("�÷��̾ �׾���");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        // ���ط��� �Է¹޾� ü���� �����ϰ� ���ظ� ������ �˸��� �̺�Ʈ�� ����
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
