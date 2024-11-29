using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFix : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        // Gamepad가 연결되어 있다면 Control Scheme 설정
        if (Gamepad.current != null)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
            Debug.Log("Gamepad Control Scheme Activated");
        }
        else
        {
            Debug.LogWarning("No Gamepad detected!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
