using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuInformation : MonoBehaviour
{
    [SerializeField] private int[] score;
    [SerializeField] private int[] lifes;
    [SerializeField] private Text[] scoreText;
    [SerializeField] private Text[] lifesText;
    [SerializeField] private GradiusManager gradiusManager;

    public void SetLifes(int shipIndex, int value) { lifes[shipIndex] = value; }
    public void SetScore(int shipIndex, int value) { score[shipIndex] = value; }
    public int GetLifes(int shipIndex) { return lifes[shipIndex]; }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerVariables.Instance.GetPlayers() == 1)
        {
            score = new int[1];
            lifes = new int[1];
            scoreText = new Text[1];
            lifesText = new Text[1];
        }
        else
        {
            score = new int[2];
            lifes = new int[2];
            scoreText = new Text[2];
            lifesText = new Text[2];
            score[1] = 0;
            lifes[1] = 3;
            scoreText[1] = transform.GetChild(5).GetComponent<Text>();
            lifesText[1] = transform.GetChild(1).GetComponent<Text>();
            lifesText[1].text = lifes[1].ToString();
        }
        scoreText[0] = transform.GetChild(3).GetComponent<Text>();
        lifesText[0] = transform.GetChild(0).GetComponent<Text>();
        score[0] = 0;
        lifes[0] = 3;
        lifesText[0].text = lifes[0].ToString();
    }

    public void DecrementLifes(int shipIndex)
    {
        lifes[shipIndex]--;
        lifesText[shipIndex].text = lifes[shipIndex].ToString();
        if(lifes[shipIndex] < 1)
        {
            gradiusManager.ShipDead(shipIndex);
        }
    }

    public void AddScore(int shipIndex, int newScore)
    {
        score[shipIndex] += newScore;
        
        string auxiliar = "";
        int total = 7 - score[shipIndex].ToString().Length;
        for (int i = 0; i < total; i++)
        {
            auxiliar += "0";
        }
        auxiliar += score[shipIndex].ToString();

        scoreText[shipIndex].text = auxiliar;
    }
    public void Set2Players()
    {
        transform.GetChild(2).GetComponent<Text>().text = "2P";
    }
}
