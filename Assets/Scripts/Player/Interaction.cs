using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    // ���� �÷��̾ �����ִ� ���ͷ����� �Ǵ� ���ӿ�����Ʈ
    public GameObject curInteractGameObject;
    // ���� �÷��̾ �����ִ� ���ͷ����� �Ǵ� ���ӿ�����Ʈ�� ����
    private IInteractable curIneractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private void Start()
    {
        // raycast�� �� � ����ī�޶� ����ϱ����� �޾ƿ�
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ī�޶� �����ִ� ȭ���� ����, ���θ� ������ ������.
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // Origin���� direction �������� maxCheckDistance��ŭ ���.(layerMask�� �ִ� ������Ʈ��)
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // ���� ���̿� �浹�� ���ӿ�����Ʈ�� ���� ���ͷ����� �Ǵ� ������Ʈ�� �ٸ��ٸ�
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    // ���� ���ͷ����� �Ǵ� ������Ʈ�� �浹�� ������Ʈ�� ����
                    curInteractGameObject = hit.collider.gameObject;
                    // �浹�� ������Ʈ�� IInteractable�� ����
                    curIneractable = hit.collider.GetComponent<IInteractable>();
                    // ������Ʈ�� ���� ������ �ִ� Text�� ���ְ� ������ �������ش�.
                    SetPromptText();
                }
            }
            else
            {
                // ���� layerMask�� ���� ������Ʈ�� �ôٸ�
                // ��� ������ null�� ������ش�.
                curInteractGameObject = null;
                curIneractable = null;
                // ������ ������ �ؽ�Ʈ�� ���ش�.
                promptText.gameObject.SetActive(false);
            }

        }
                
    }

    private void SetPromptText()
    {
        // ������ ������ �ؽ�Ʈ ���ӿ�����Ʈ�� ���ش�.
        promptText.gameObject.SetActive(true);
        // ���� ���ͷ����� �Ǵ� ������Ʈ�� ������ ������ Txet�� �����ش�.
        promptText.text = curIneractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // ���� ���� ������ �ִ� ������ Ű�� ������ �� �׸��� curinteractGameObject�� null�� �ƴ� ��
        if (context.phase == InputActionPhase.Started && curInteractGameObject != null)
        {
            // �̺�Ʈ�� �߻��ϰ� ���ӿ�����Ʈ�� �����Ǹ� �κ��丮�� ����.
            curIneractable.OnInteract();
            // �κ��丮�� ���� ���ÿ� ������Ʈ�� ������Ʈ ������ null�� ������ش�.
            curInteractGameObject = null;
            curIneractable = null;
            // ������ ������ �ؽ�Ʈ ���ӿ�����Ʈ�� ���ش�.(�������� ������� ������)
            promptText.gameObject.SetActive(false);
        }
    }

}
