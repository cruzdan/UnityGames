using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundAdder : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private ButtonSoundInfo[] specificButtonSounds;
    [SerializeField] private Button[] buttonsWithoutSound;
    #endregion
    #region Private Variables
    [SerializeField] private Object[] buttonObjects;
    private int buttonIndex = -1;
    #endregion
    #region Functions
    //private void Start()
    //{
    //        buttonObjects = Resources.FindObjectsOfTypeAll(typeof(Button));
    //    foreach (var buttonObject in buttonObjects)
    //    {
    //        Button button = (Button)buttonObject;
    //        buttonIndex = ButtonSoundInfo.GetButtonIndexInButtonSoundInfos(button, specificButtonSounds);
    //        if (buttonIndex >= 0)
    //        {
    //            int specificButtonIndex = buttonIndex;
    //            button.onClick.AddListener(() =>
    //            {
    //                SFXManager.Instance.PlaySFX(specificButtonSounds[specificButtonIndex].ButtonClip);
    //            });
    //        }
    //        else if (!buttonsWithoutSound.Contains(button))
    //        {
    //            button.onClick.AddListener(() =>
    //            {
    //                SFXManager.Instance.PlaySFX(buttonClip);
    //            });
    //        }
    //    }
    //}

    private void OnEnable()
    {
        Invoke(nameof(AddSoundToButtons), 2f);
    }

    void AddSoundToButtons()
    {
        buttonObjects = Resources.FindObjectsOfTypeAll(typeof(Button));
        foreach (var buttonObject in buttonObjects)
        {
            Button button = (Button)buttonObject;
            buttonIndex = ButtonSoundInfo.GetButtonIndexInButtonSoundInfos(button, specificButtonSounds);
            if (buttonIndex >= 0)
            {
                int specificButtonIndex = buttonIndex;
                button.onClick.AddListener(() =>
                {
                    SFXManager.Instance.PlaySFX(specificButtonSounds[specificButtonIndex].ButtonClip);
                });
            }
            else if (!buttonsWithoutSound.Contains(button))
            {
                button.onClick.AddListener(() =>
                {
                    SFXManager.Instance.PlaySFX(buttonClip);
                });
            }
        }
    }

    [ContextMenu("Test")]
    void Test()
    {
        buttonObjects = Resources.FindObjectsOfTypeAll(typeof(Button));
    }
    #endregion
}

[System.Serializable]
public class ButtonSoundInfo
{
    public AudioClip ButtonClip;
    public Button Button;
    public static int GetButtonIndexInButtonSoundInfos(Button button, ButtonSoundInfo[] buttonSoundInfos)
    {
        int total = buttonSoundInfos.Length;
        for (int i = 0; i < total; i++)
        {
            if (buttonSoundInfos[i].Button == button)
            {
                return i;
            }
        }
        return -1;
    }
}