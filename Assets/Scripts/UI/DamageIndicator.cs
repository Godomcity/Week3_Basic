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
        // 이벤트 함수 onTakeDamage가 실행 될 때 Flash함수 실행
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if(coroutine != null)
        {
            // coroutine이 null이 아니면 coroutine을 멈춘다.
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        // 코루틴 실행
        coroutine = StartCoroutine(FadeAway());
    }

    // 체력이 캠프파이어에 닿아 감소될때 화면을 번쩍이는 효과를 주는 코루틴
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            // 알파 값을 서서히 빼준다.
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        image.enabled = false;
    }
}
