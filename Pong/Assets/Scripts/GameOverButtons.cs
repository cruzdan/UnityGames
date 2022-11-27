using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverButtons : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button exitButton;
    void Start()
    {
        yesButton.onClick.AddListener(() => GameObject.Find("PlayerManager").GetComponent<PlayerManager>().ChangeScene("GamePong"));
        exitButton.onClick.AddListener(() => GameObject.Find("PongGameManager").GetComponent<PongGameManager>().Exit());
    }

}
