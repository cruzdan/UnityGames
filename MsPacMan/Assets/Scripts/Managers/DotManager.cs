using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotManager : Singleton<DotManager>
{
    [SerializeField] private ObjectPool pacDotPool;
    [SerializeField] private ObjectPool powerPelletPool;
    [SerializeField] private Color[] dotColors;
    Blinking[] powerPelletBlinking = new Blinking[4];
    const int totalPacDots = 240;
    SpriteRenderer[] dotRenderers = new SpriteRenderer[totalPacDots];
    SpriteRenderer[] powerPelletRenderers = new SpriteRenderer[4];
    bool initialized = false;
    public void InitPowerPellets()
    {
        Transform powerPelletParent = powerPelletPool.gameObject.transform;
        int total = powerPelletParent.childCount;
        for (int i = 0; i < total; i++)
        {
            powerPelletBlinking[i] = powerPelletParent.GetChild(i).GetComponent<Blinking>();
            powerPelletRenderers[i] = powerPelletBlinking[i].GetComponent<SpriteRenderer>();
        }
        initialized = true;
    }
    public void InitPacDots()
    {
        Transform pacDotParent = pacDotPool.gameObject.transform;
        int total = pacDotParent.childCount;
        for (int i = 0; i < total; i++)
        {
            dotRenderers[i] = pacDotParent.GetChild(i).GetComponent<SpriteRenderer>();
        }
        initialized = true;
    }
    public void SetDotColorsByIndex(int index)
    {
        int total = totalPacDots;
        for(int i = 0; i < total; i++)
        {
            dotRenderers[i].color = dotColors[index];
        }
        for(int i = 0; i < 4; i++)
        {
            powerPelletRenderers[i].color = dotColors[index];
            powerPelletBlinking[i].SetCurrentColor(dotColors[index]);
        }
    }
    public void ReturnPacDot(GameObject pacDot)
    {
        pacDotPool.ReturnObjectToPool(pacDot);
    }
    public GameObject GetPacDot()
    {
        return pacDotPool.GetObjectFromPool();
    }
    public void ReturnPowerPellet(GameObject powerPellet)
    {
        powerPelletPool.ReturnObjectToPool(powerPellet);
    }
    public GameObject GetPowerPellet()
    {
        return powerPelletPool.GetObjectFromPool();
    }
    public void ActivePowerPelletAnimation(bool value)
    {
        int total = powerPelletBlinking.Length;
        for(int i = 0; i < total; i++)
        {
            powerPelletBlinking[i].SetPause(!value);
            if(value)
                powerPelletBlinking[i].Restart();
        }
    }
    public void SetVisiblePowerPellets(bool value)
    {
        int total = powerPelletBlinking.Length;
        for (int i = 0; i < total; i++)
        {
            powerPelletBlinking[i].SetVisible(value);
        }
    }
    public bool GetInitialized()
    {
        return initialized;
    }
}
