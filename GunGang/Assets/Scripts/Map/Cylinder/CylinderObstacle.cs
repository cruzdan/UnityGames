using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class CylinderObstacle : MonoBehaviour
{
    [SerializeField] private int _life;
    [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private CharacterBehaviour _character;
    [SerializeField] private Score _score;
    public void SetLife(int life)
    {
        _life = life;
        UpdateTextMeshWithCurrentLife();
    }

    public void ReduceCylinderLife(int amount)
    {
        _life -= amount;
        UpdateTextMeshWithCurrentLife();
        if (HasNoLife())
        {
            RemoveCylinder();
            IncrementScore();
        }
    }

    void UpdateTextMeshWithCurrentLife()
    {
        _textMesh.text = _life.ToString();
    }

    void RemoveCylinder()
    {
        if (HasCharacter())
        {
            RemoveCharacter();
        }
        ObjectPool.Instance.ReturnObjectToPoolInPoolParent(gameObject, ObjectPool.PoolObjectType.Cylinder);
    }

    void IncrementScore()
    {
        _score.IncrementScore(20 + CylinderBonusFromPlayerPrefs.BonusPoints);
    }

    bool HasNoLife()
    {
        return _life <= 0;
    }

    bool HasCharacter()
    {
        return _character != null;
    }

    void RemoveCharacter()
    {
        _character.SetTrigger(true);
        _character = null;
    }

    public void SetCharacter(CharacterBehaviour character)
    {
        _character = character;
    }
    public void RestartVars()
    {
        _character = null;
    }
}
