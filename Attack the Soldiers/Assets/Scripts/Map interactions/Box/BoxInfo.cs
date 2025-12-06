using UnityEngine;
[System.Serializable]
public class BoxInfo
{
    #region Serialized Variables
    [SerializeField] private BoxType boxType;
    [SerializeField] private Color boxColor;
    #endregion
    #region Public Properties
    public BoxType BoxType { get { return boxType; } }
    public Color BoxColor { get { return boxColor; } }
    #endregion
}

public enum BoxType
{
    Health = 0,
    Speed = 1,
    Weapon = 2
}