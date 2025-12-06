using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Text lifeText;
    [SerializeField] private RectTransform lifeRect;
    [SerializeField] private Text staminaText;
    [SerializeField] private RectTransform staminaRect;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text deadTimeText;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private Image weaponImage;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject pauseMenu;
    #endregion
    #region Actions
    public Action OnPausePressed;
    #endregion
    #region Functions
    public void SetLifeText(string text)
    {
        lifeText.text = text;
    }

    public void SetLifeWidth(float scaleX)
    {
        lifeRect.localScale = new Vector3(scaleX, 1, 1);
    }

    public void SetStaminaText(string text)
    {
        staminaText.text = text;
    }

    public void SetBulletText(string text)
    {
        bulletText.text = text;
    }

    public void SetStaminaWidth(float scaleX)
    {
        staminaRect.localScale = new Vector3(scaleX, 1, 1);
    }

    public void SetDeadTimeText(string text)
    {
        deadTimeText.text = text;
    }

    public void ActiveDeadMenu(bool value)
    {
        deadMenu.SetActive(value);
    }

    public void SetWeponSprite(int index)
    {
        weaponImage.sprite = weaponSprites[index];
    }

    public void ChangePauseMenu()
    {
        OnPausePressed?.Invoke();
    }

    public void OpenPauseMenu()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion
}
