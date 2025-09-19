using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ChildEnumerator : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private string childPrefix;
    
    public void EnumerateChildren()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            child.name = childPrefix + i;
            Debug.Log(childPrefix + i);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChildEnumerator))]
public class ChildEnumeratorEditor : Editor
{
    ChildEnumerator childEnumerator;
    private void OnEnable()
    {
        childEnumerator = (ChildEnumerator)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Enumerate Children"))
        {
            childEnumerator.EnumerateChildren();
        }
    }
}
#endif
