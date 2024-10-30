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

    // 현재 플레이어가 보고있는 인터렉션이 되는 게임오브젝트
    public GameObject curInteractGameObject;
    // 현재 플레이어가 보고있는 인터렉션이 되는 게임오브젝트의 정보
    private IInteractable curIneractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private void Start()
    {
        // raycast를 쏠때 어떤 메인카메라를 사용하기위해 받아옴
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 카메라가 보고있는 화면을 가로, 세로를 반으로 나눈다.
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // Origin에서 direction 방향으로 maxCheckDistance만큼 쏜다.(layerMask가 있는 오브젝트만)
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 만약 레이에 충돌한 게임오브젝트가 현재 인터렉션이 되는 오브젝트와 다르다면
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    // 현재 인터렉션이 되는 오브젝트를 충돌한 오브젝트로 변경
                    curInteractGameObject = hit.collider.gameObject;
                    // 충돌된 오브젝트의 IInteractable로 변경
                    curIneractable = hit.collider.GetComponent<IInteractable>();
                    // 오브젝트에 대한 설명이 있는 Text를 켜주고 정보를 변경해준다.
                    SetPromptText();
                }
            }
            else
            {
                // 만약 layerMask가 없는 오브젝트를 봤다면
                // 모든 정보를 null로 만들어준다.
                curInteractGameObject = null;
                curIneractable = null;
                // 설명이 나오는 텍스트도 꺼준다.
                promptText.gameObject.SetActive(false);
            }

        }
                
    }

    private void SetPromptText()
    {
        // 설명이 나오는 텍스트 게임오브젝트를 켜준다.
        promptText.gameObject.SetActive(true);
        // 현재 인터렉션이 되는 오브젝트의 설명을 가져와 Txet로 보여준다.
        promptText.text = curIneractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // 만약 지금 누르고 있는 정보가 키를 눌렀을 때 그리고 curinteractGameObject가 null이 아닐 때
        if (context.phase == InputActionPhase.Started && curInteractGameObject != null)
        {
            // 이벤트를 발생하고 게임오브젝트가 삭제되며 인벤토리로 들어간다.
            curIneractable.OnInteract();
            // 인벤토리에 들어감과 동시에 오브젝트와 오브젝트 정보를 null로 만들어준다.
            curInteractGameObject = null;
            curIneractable = null;
            // 설명이 나오는 텍스트 게임오브젝트를 꺼준다.(아이템이 사라졌기 때문에)
            promptText.gameObject.SetActive(false);
        }
    }

}
