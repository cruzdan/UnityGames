using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private float timeToActivateTrap;
    [SerializeField] private Material highwayMaterial;
    [SerializeField] private Material lavaMaterial;
    [SerializeField] private Transform target;
    //distance to generate enemies from the target
    [SerializeField] private float distance;
    private float timer;
    private bool trapActive = false;
    private Renderer ren;
    private Rigidbody trapRigidbody;
    private BoxCollider trapCollider;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
    }
    public void SetFloor(GameObject newFloor)
    {
        floor = newFloor;
        trapRigidbody = floor.AddComponent<Rigidbody>();
        trapRigidbody.useGravity = false;
        trapRigidbody.freezeRotation = false;
        floor.gameObject.tag = "Enemy";
        trapCollider = floor.GetComponent<BoxCollider>();
    }
    public void SetTimeToActivateTrap(float time)
    {
        timeToActivateTrap = time;
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToActivateTrap;
        ren = floor.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Mathf.Abs(transform.position.z - target.position.z);
        if(targetDistance <= distance)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (trapActive)
                {
                    trapActive = false;
                    trapCollider.isTrigger = false;
                    timer = timeToActivateTrap;
                    ren.material = highwayMaterial;
                }
                else
                {
                    trapActive = true;
                    trapCollider.isTrigger = true;
                    timer = timeToActivateTrap / 2f;
                    ren.material = lavaMaterial;
                }
            }
        }
    }
}
