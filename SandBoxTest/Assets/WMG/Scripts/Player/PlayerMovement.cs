using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;

    private Vector2 moveInput;
    private Vector3 lookInput;
    private float xRotation = 0f;
    
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    
    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        var data = SaveSystem.LoadGameState();
        if (data != null)
        {
            // 위치 적용
            controller.enabled = false;
            transform.position = data.playerPosition.ToVector3();
            transform.rotation = Quaternion.Euler(0f, data.playerRotationY, 0f);
            controller.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveSystem.DeleteAllSaves();
            Debug.Log("🗑[T] 키 입력 → 저장 데이터 삭제 완료 테스트용 PlayerMovement.cs");
        }
        
        if (ObjectInspector.Instance?.IsInspecting == true || PuzzleManager.Instance?.IsCutsceneActive == true || PanelManager.Instance?.IsUiOpened == true)
        {
            return;
        }
        
        moveInput = moveAction.action.ReadValue<Vector2>();
        lookInput = lookAction.action.ReadValue<Vector2>();

        // 이동
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 마우스 회전 (Look)
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
