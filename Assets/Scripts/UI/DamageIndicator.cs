using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;

    void Start()
    {
        // �̺�Ʈ �Լ� onTakeDamage�� ���� �� �� Flash�Լ� ����
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if(coroutine != null)
        {
            // coroutine�� null�� �ƴϸ� coroutine�� �����.
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        // �ڷ�ƾ ����
        coroutine = StartCoroutine(FadeAway());
    }

    // ü���� ķ�����̾ ��� ���ҵɶ� ȭ���� ��½�̴� ȿ���� �ִ� �ڷ�ƾ
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            // ���� ���� ������ ���ش�.
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        image.enabled = false;
    }
}
