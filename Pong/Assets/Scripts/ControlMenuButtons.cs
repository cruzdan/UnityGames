using UnityEngine;
using UnityEngine.UI;

public class ControlMenuButtons : MonoBehaviour
{
    [SerializeField] private Button upChangeButton;
    [SerializeField] private Button downChangeButton;
    [SerializeField] private GameObject controlPrefab;
    [SerializeField] private GameObject changeKeyPanel;
    private GameObject controlMenu;
    bool activeButtons = true;

    private void Start()
    {       
        if (PlayerController.Instance.GetPlayers() == 2)
        {
            controlMenu = Instantiate(controlPrefab) as GameObject;
            controlMenu.transform.SetParent(transform);
            controlMenu.transform.position = new Vector2(398, 167);
            AddListenerOnClick();
        }
    }

    void AddListenerOnClick()
    {
        Button button1;
        //transform.GetChild(9) is the SecondPlayerControls
        //transform.GetChild(9).GetChild(0) is the UpControlButton
        button1 = transform.GetChild(9).GetChild(0).GetComponent<Button>();
        button1.onClick.AddListener(() => GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayButtonSound());
        button1.onClick.AddListener(() => GameObject.Find("ControlsMenu").GetComponent<ControlMenuButtons>().ChangeActiveButtons());
        button1.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetActiveChangeKeyPanel());
        //transform.GetChild(9).GetChild(4) is the UpControlText
        button1.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetTextField(transform.GetChild(9).GetChild(4).GetComponent<Text>()));
        button1.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().UpKeyControl(true));
        button1.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetPlayerMovement(2));

        Button button2;
        //transform.GetChild(9).GetChild(1) is the DownSecondButton
        button2 = transform.GetChild(9).GetChild(1).GetComponent<Button>();
        button2.onClick.AddListener(() => GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayButtonSound());
        button2.onClick.AddListener(() => GameObject.Find("ControlsMenu").GetComponent<ControlMenuButtons>().ChangeActiveButtons());
        button2.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetActiveChangeKeyPanel());
        //transform.GetChild(9).GetChild(5) is the DownControlText
        button2.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetTextField(transform.GetChild(9).GetChild(5).GetComponent<Text>()));
        button2.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().UpKeyControl(false));
        button2.onClick.AddListener(() => changeKeyPanel.GetComponent<ChangeKeyManager>().SetPlayerMovement(2));
    }

    public void ChangeActiveButtons()
    {
        if (activeButtons)
        {
            InteractableButtons(false);
        }
        else
        {
            InteractableButtons(true);
        }
        activeButtons = !activeButtons;
    }

    void InteractableButtons(bool value)
    {
        upChangeButton.interactable = value;
        downChangeButton.interactable = value;
        if (PlayerController.Instance.GetPlayers() == 2)
        {
            transform.GetChild(9).GetChild(1).GetComponent<Button>().interactable = value;
            transform.GetChild(9).GetChild(0).GetComponent<Button>().interactable = value;
        }
    }
}
