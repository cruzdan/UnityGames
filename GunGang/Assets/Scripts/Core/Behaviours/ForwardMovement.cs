using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    [SerializeField] private Vector3 movement;
    public void Init(Vector3 velocity)
    {
        movement = velocity;
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;
    }
}
