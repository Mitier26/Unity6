using UnityEngine;

/// <summary>
/// 개발 중 디버깅을 위한 자유 비행 카메라
/// WASD: 이동, 마우스: 시점 회전, Shift: 빠른 이동, Space/Ctrl: 위/아래 이동
/// </summary>
public class FlyCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("일반 이동 속도")]
    public float normalSpeed = 10.0f;
    
    [Tooltip("Shift 키를 누를 때 이동 속도")]
    public float fastSpeed = 20.0f;
    
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 3.0f;

    [Header("Debug Settings")]
    [Tooltip("활성화/비활성화 키")]
    public KeyCode toggleKey = KeyCode.F1;
    
    [Tooltip("마우스 커서 숨김 여부")]
    public bool hideCursor = true;
    
    [Tooltip("시작 시 활성화 여부")]
    public bool activeOnStart = true;

    private float currentSpeed;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private bool isEnabled;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // 초기 위치와 회전 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        // 초기 yaw와 pitch 설정
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        
        // 시작 시 활성화 설정
        SetEnabled(activeOnStart);
    }

    void Update()
    {
        // 토글 키로 활성화/비활성화
        if (Input.GetKeyDown(toggleKey))
        {
            SetEnabled(!isEnabled);
        }

        if (!isEnabled) return;

        // 마우스 입력으로 회전
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 이동 속도 설정 (Shift로 빠른 이동)
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;

        // WASD 이동
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * currentSpeed * Time.deltaTime;

        // 위/아래 이동 (Space/Ctrl)
        if (Input.GetKey(KeyCode.Space))
            transform.position += Vector3.up * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl))
            transform.position -= Vector3.up * currentSpeed * Time.deltaTime;

        // R 키를 누르면 시작 위치로 리셋
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            yaw = initialRotation.eulerAngles.y;
            pitch = initialRotation.eulerAngles.x;
        }

        // 디버그 정보 표시
        DrawDebugInfo();
    }

    private void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        
        if (hideCursor && isEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void DrawDebugInfo()
    {
        // 여기에 필요한 디버그 정보를 추가할 수 있습니다.
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 3f, Color.red);
        Debug.DrawRay(transform.position, transform.up * 3f, Color.green);
    }

    void OnGUI()
    {
        if (!isEnabled) return;

        // 화면 왼쪽 상단에 기본 정보 표시
        GUI.Label(new Rect(10, 10, 300, 20), $"위치: {transform.position.ToString("F2")}");
        GUI.Label(new Rect(10, 30, 300, 20), $"회전: ({pitch:F1}, {yaw:F1})");
        GUI.Label(new Rect(10, 50, 300, 20), $"속도: {currentSpeed}");
        GUI.Label(new Rect(10, 70, 300, 20), "F1: 비행 카메라 켜기/끄기, R: 위치 리셋");
    }

    void OnDestroy()
    {
        // 스크립트가 파괴될 때 커서 상태 복원
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}