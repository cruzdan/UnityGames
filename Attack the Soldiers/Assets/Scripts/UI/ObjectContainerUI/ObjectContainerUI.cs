using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

//Class to manage a UI container that displays a list of objects with pagination support.
public class ObjectContainerUI : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private List<GameObject> objectListToShow;
    [SerializeField] private GameObject[] objectsInUI;
    [SerializeField] private TextMeshProUGUI currentPageText;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;
    #endregion
    #region Private Variables
    private int currentPage = 0;
    #endregion
    #region Actions
    public Action<GameObject> OnAddObject;
    public Action<GameObject> OnRemoveObject;
    public Action OnInitialized;
    #endregion
    #region Public Properties
    public List<GameObject> ObjectListToShow { get => objectListToShow; set => objectListToShow = value; }
    public GameObject[] ObjectsInUI => objectsInUI;
    public int CurrentPage => currentPage;
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.Instance.IsOnline) return;
        AddButtonEvents();
    }

    public void AddObject(GameObject objectToAdd)
    {
        objectListToShow.Add(objectToAdd);
        OnAddObject?.Invoke(objectToAdd);
        Initialize();
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        objectListToShow.Remove(objectToRemove);
        OnRemoveObject?.Invoke(objectToRemove);
        Initialize();
    }

    public void Initialize()
    {
        ActiveObjectsInPageUI(currentPage);
        currentPageText.text = (currentPage + 1).ToString();
        OnInitialized?.Invoke();
    }

    void ActiveObjectsInPageUI(int page)
    {
        int objectsPerPage = objectsInUI.Length;
        int startIndex = page * objectsPerPage;
        for (int i = 0; i < objectsInUI.Length; i++)
        {
            if (objectsInUI[i] == null) continue;
            int objectIndex = startIndex + i;
            if (objectIndex < objectListToShow.Count)
            {
                objectsInUI[i].SetActive(true);
            }
            else
            {
                objectsInUI[i].SetActive(false);
            }
        }
    }

    void AddButtonEvents()
    {
        previousPageButton.onClick.AddListener(OnPreviousPageClicked);
        nextPageButton.onClick.AddListener(OnNextPageClicked);
    }

    void OnPreviousPageClicked()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ActiveObjectsInPageUI(currentPage);
            currentPageText.text = (currentPage + 1).ToString();
        }
    }

    void OnNextPageClicked()
    {
        int maxPage = (objectListToShow.Count - 1) / objectsInUI.Length;
        if (currentPage < maxPage)
        {
            currentPage++;
            ActiveObjectsInPageUI(currentPage);
            currentPageText.text = (currentPage + 1).ToString();
        }
    }
    #endregion
}
