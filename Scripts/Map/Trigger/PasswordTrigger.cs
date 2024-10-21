using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine.Serialization;


/* 
 * 패스워드를 입력할 수 있는 공간에 들어와서 'F'를 누르면 
 * 카메라 초점이 도어락에 고정되게 하는 트리거
 */
public class PasswordTrigger : MonoBehaviour
{

    [SerializeField]
    public CinemachineVirtualCamera DoorLockCam;
    
    public GameObject player;
    public StarterAssetsInputs starterAssetsInputs;
    private bool isTriggered = false;
    private bool isFocused = false;
    public GameObject message_focus;
    public GameObject message_return;

    public RecordPathCSV recordPathCsv;

    public int a = 0;
    
    private void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFocusOnDoorLock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            recordPathCsv.End_Timer();
            isTriggered = true;
            message_focus.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isTriggered = false;
            message_focus.SetActive(false);
        }
    }

    public void ToggleFocusOnDoorLock()
    {
        if (!isFocused)
        {
            EnterFocusOnDoorLock();
        }
        else
        {
            ExitFocusOnDoorLock();
        }
    }
    
    // 도어락에 카메라 초점 고정
    public void EnterFocusOnDoorLock()
    {
        message_focus.SetActive(false);
        message_return.SetActive(true);
        
        isFocused = true;
        DoorLockCam.Priority = 100;
        starterAssetsInputs.SetCursorState(false);
        starterAssetsInputs.move = new Vector2(0,0);
        player.SetActive(false);
    }
    
    // 도어락에 카메라 초점 고정 해제
    public void ExitFocusOnDoorLock()
    {
        message_focus.SetActive(true);
        message_return.SetActive(false);
        
        isFocused = false;
        DoorLockCam.Priority = 9;
        starterAssetsInputs.SetCursorState(true);
        player.SetActive(true);
    }
}
