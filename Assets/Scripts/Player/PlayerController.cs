using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 입문 주차와 숙련 주차의 입력방식의 차이와 공통점
    // 입력 주차는 "On + ActionName"인 함수를 찾아 호출하는 방식 (SendMessage)
    // 숙력 주차는 Inspector 상에서 Action에 함수를 설정하고 키 입력이 들어 왔을 때 호출하는 방식 (Invoke Event)
    // SendMessage와 Invoke Event의 공통점은 Action을 통해 작동한다는 점이 같습니다.
    // SendMessage는 함수를 찾아 호출하지만 Invoke Event는 버튼 처럼 함수를 설정하고 키를 눌렀을 때만 호출하여 작동 됩니다.

    [Header("Moverment")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurxRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // 물리연산을 하는 곳에는 FiexdUpdate에서 호출해주는게 좋다.
        // 그 이유는 고정된 간격으로 호출되기 때문에 프레임에 따라 호출되는 Update보다 일관된 결과를 얻을 수 있기 때문
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void Move()
    {
        // Vector3 dir 변수에 앞뒤 * curMovementInput.y의 값과 오른쪽 왼쪽 * curMovementInput.x의 값을 더해준다.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        // dir(방향)에 MoveSpeed를 계속 곱해준다.
        dir *= moveSpeed;
        // dir.y는 rigidbody를 통해 초기화
        dir.y = rigidbody.velocity.y;
        // rigidbody.velocity에 방향을 넣어줘서 움직일 수 있게 설정
        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        // 카메라를 돌려줄 값을 mouseDealt.y에서 받고 민감도를 곱해준다.
        camCurxRot += mouseDelta.y * lookSensitivity;
        // camCurRot의 최소값 최대값을 넘어가지 않게 Mathf.Clamp()를 활용한다.
        // camCurRot가 minXLook을 넘어가면 minXLook값을 반환하고 maxXLook을 넘어가면 maxXLook값을 반환한다.
        camCurxRot = Mathf.Clamp(camCurxRot, minXLook, maxXLook);
        // 월드 좌표에 있는게 아니라 로컬 좌표로 불러온다.
        cameraContainer.localEulerAngles = new Vector3(-camCurxRot, 0, 0);

        // 카메라의 상하를 움직이기 위해 mouseDelta.x를 캐릭터 y값에 넣어준다.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}