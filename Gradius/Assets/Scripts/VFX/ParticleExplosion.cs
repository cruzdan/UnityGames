using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
    private float timer;
    private float timeToStop = 1f;
    public void Restart() { timer = timeToStop; }
    void Start()
    {
        GetComponent<ForwardMovement>().Init(SquaresResolution.TotalSquaresX / 7f, 180);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            ParticleManager.Instance.ReturnParticleObject(this.gameObject);
        }
    }
}
