using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ScrollRect sc;
    [SerializeField] private RectTransform[] rects;
    
    public void SetContent(int index)
    {
        sc.content.gameObject.SetActive(false);
        rects[index].gameObject.SetActive(true);
        sc.content = rects[index];
    }
}
