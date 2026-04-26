using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BoxInfoSO", menuName = "ScriptableObjects/BoxInfoSO", order = 1)]
public class BoxInfoSO : ScriptableObject
{
    #region Serialized Variables
    [SerializeField] private List<BoxInfo> boxInfos;
    #endregion
    #region Public Properties
    public List<BoxInfo> BoxInfos => boxInfos;
    #endregion
    #region Private Variables
    private Dictionary<BoxType, BoxInfo> boxInfoDict = new Dictionary<BoxType, BoxInfo>();
    #endregion
    #region Functions
    public void InitializeBoxDictionary()
    {
        boxInfoDict.Clear();
        boxInfoDict.InitializeFromList(boxInfos, b => b.BoxType);
    }

    public BoxInfo GetBoxInfo(BoxType boxType)
    {
        return boxInfoDict.TryGetValue(boxType, out var info) ? info : null;
    }

    public Sprite GetBoxSprite(BoxType boxType)
    {
        return GetBoxInfo(boxType)?.BoxSprite;
    }
    #endregion
}
