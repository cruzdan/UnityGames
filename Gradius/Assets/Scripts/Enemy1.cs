using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
	private Rigidbody2D rb;
	[SerializeField] private float speedX;
	[SerializeField] private float speedY;
	[SerializeField] private float limitUpY;
	[SerializeField] private float limitDownY;

	//0->down, 1->up
	[SerializeField] private Sprite[] sprites;
	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speedX, speedY);
	}

	public void Init(float newSpeedX, float newSpeedY, float limYUp, float limYDown)
    {
		speedX = newSpeedX;
		speedY = newSpeedY;
		limitUpY = limYUp;
		limitDownY = limYDown;
		if(speedY > 0)
        {
			GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
			GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
	}

    private void FixedUpdate()
    {
		if (rb.velocity.y > 0)
		{
			if (transform.position.y > limitUpY)
			{
				rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
				GetComponent<SpriteRenderer>().sprite = sprites[0];
			}
		}
		else
		{
			if (transform.position.y < limitDownY)
			{
				rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
				GetComponent<SpriteRenderer>().sprite = sprites[1];
			}
		}
	}
}
