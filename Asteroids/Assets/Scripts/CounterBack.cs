using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*use it on a GameObject with Text component and use it calling Reiniciate() and SetActive(true) methods*/
public class CounterBack : MonoBehaviour
{
    private Text counter;
    private float textTime = 3f;
    private int actualValue;
    [SerializeField] PauseManager pauseManager;

    public void SetTime(float time)
    {
        textTime = time;
    }
    public float GetTime()
    {
        return textTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        counter = GetComponent<Text>();
    }

    // Update is called once per frame
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
            pauseManager.ContinueGame();
        }
    }

    public void Reiniciate()
    {
        textTime = 3f;
        gameObject.SetActive(true);
    }
}
