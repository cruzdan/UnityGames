using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonFunctions2: MonoBehaviour
{
    [SerializeField] private ShopInformation shopInf;
    [SerializeField] private Button lifeButton;
    [SerializeField] private Button bulletButton;
    // Start is called before the first frame update
    void Start()
    {
        
        lifeButton.onClick.AddListener(() => { shopInf.ShowButtons(3, 0); });
        lifeButton.onClick.AddListener(() => { shopInf.ShowInfo(3, 0, shopInf.GetShopInformation(13)); });
        lifeButton.onClick.AddListener(() => { shopInf.SaveIndex(3, 0); });

        bulletButton.onClick.AddListener(() => { shopInf.ShowButtons(4, 0); });
        bulletButton.onClick.AddListener(() => { shopInf.ShowInfo(4, 0, shopInf.GetShopInformation(14)); });
        bulletButton.onClick.AddListener(() => { shopInf.SaveIndex(4, 0); });
    }
}
