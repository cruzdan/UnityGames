using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleInput : MonoBehaviour
{
    private PlayerInput playerInput;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {
        if (playerInput.actions["Play"].WasPressedThisFrame())
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
