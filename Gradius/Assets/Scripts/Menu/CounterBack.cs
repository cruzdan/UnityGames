using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*use it on a GameObject with Text component and use it calling Reiniciate() and SetActive(true) methods*/
public class CounterBack : MonoBehaviour
{
    [SerializeField] private Text counter;
    private float textTime = 3f;
    private int actualValue;
    [SerializeField] GradiusManager gradManager;
    public void SetTime(float time)
    {
        textTime = time;
    }
    public float GetTime()
    {
        return textTime;
    }
    void Update()
    {
        actualValue = (int)textTime;
        textTime -= Time.deltaTime;
        if (actualValue != (int)textTime)
        {
            counter.text = actualValue.ToString();
        }
        if (textTime < 0)
        {
            gameObject.SetActive(false);
            gradManager.ChangeRestartingToPlayGame();
        }
    }

    public void Reiniciate()
    {
        textTime = 3f;
        gameObject.SetActive(true);
    }
}