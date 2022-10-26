using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private GameObject paddle1;
    [SerializeField] private GameObject paddle2;
    [SerializeField] private GameObject ball;
    [SerializeField] private Text counter;
    float textTime = 3f;
    int actualValue;
    // Start is called before the first frame update
    void Start()
    {
        ball.SetActive(false);
        counter.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        actualValue = (int)textTime;
        textTime -= Time.deltaTime;
        if (actualValue != (int)textTime)
        {
            counter.text = actualValue.ToString();
        }
        if (textTime <= 0)
        {
            ball.SetActive(true);
            counter.gameObject.SetActive(false);
            gameObject.SetActive(false);
            paddle1.GetComponent<PlayerMovement>().SetMove(true);
            if (PlayerController.Instance.GetPlayers() == 1)
                paddle2.GetComponent<AIMovement>().SetMove(true);
            else
                paddle2.GetComponent<PlayerMovement>().SetMove(true);
        }
    }

    public void Reinitiate() // typo
    {
        textTime = 3f;
        gameObject.SetActive(true);
        counter.gameObject.SetActive(true);
        paddle1.GetComponent<PlayerMovement>().SetMove(false);
        if (PlayerController.Instance.GetPlayers() == 1)
            paddle2.GetComponent<AIMovement>().SetMove(false);
        else
            paddle2.GetComponent<PlayerMovement>().SetMove(false);
    }
}
