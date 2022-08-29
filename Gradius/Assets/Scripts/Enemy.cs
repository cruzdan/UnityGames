using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int score = 100;
    [SerializeField] private int lifes = 1;
    [SerializeField] private bool upgrade = false;
    [SerializeField] private Information info;
    private bool dead = false;
    public void AddScore(int shipIndex) { info.AddScore(shipIndex, score); }
    public void SetLifes(int newLifes) { lifes = newLifes; }
    public void SetScore(int newScore) { score = newScore; }
    public void SetUpgrade(bool newUpgrade) { upgrade = newUpgrade; }
    public void SetDead(bool newDead) { dead = newDead; }
    public void SetInformation(Information i) { info = i; }
    public int GetLifes() { return lifes; }
    public int GetScore() { return score; }
    public bool GetUpgrade() { return upgrade; }
    public bool GetDead() { return dead; }
}