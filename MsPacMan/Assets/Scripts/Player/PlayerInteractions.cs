using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] private Score score;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private DotManager dotManager;
    [SerializeField] private GameManager gameManager;
    Fruit fruit;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "PacDot":
                dotManager.ReturnPacDot(collision.gameObject);
                score.AddScore(10);
                playerMovement.SetLowSpeed(1);
                gameManager.IncrementDotCounter();
                gameManager.RestartGhostTimer();
                levelManager.DecrementTotalDots();
                break;
            case "PowerPellet":
                dotManager.ReturnPowerPellet(collision.gameObject);
                score.AddScore(50);
                playerMovement.EnterFrightMode();
                playerMovement.SetLowSpeed(0);
                gameManager.IncrementDotCounter();
                gameManager.RestartGhostTimer();
                gameManager.SetFrightGhostMode();
                levelManager.DecrementTotalDots();
                gameManager.StartEatedGhostsCounter();
                break;
            case "Tunnel":
                playerMovement.SetOnTunnel(true);
                break;
            case "Fruit":
                fruit = collision.GetComponent<Fruit>();
                score.AddScore(fruit.GetScore());
                gameManager.AddScoreSprite(fruit.GetGameIndex(), LevelInformation.Instance.FruitTypes[fruit.GetGameIndex()]);
                collision.gameObject.SetActive(false);
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tunnel"))
        {
            playerMovement.SetOnTunnel(false);
        }
    }
}