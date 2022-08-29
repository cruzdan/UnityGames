using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToShip : MonoBehaviour
{
	/*
     * sprites:
    0 -> down, 75°
    1 -> down, 45°
	2 -> down, 0°
     */
	[SerializeField] private Sprite[] sprites;
    [SerializeField] private Transform ship;
	[SerializeField] private bool up;
	private int spriteIndex = 0;
	
	float firstX;
	float firstY;

	public void SetShip(Transform newShip) { ship = newShip; }

	public void SetUp(bool newUp) { up = newUp; }

	// Start is called before the first frame update
	void Start()
    {
		firstX = Squares.totalSquaresX / 2;
		firstY = Squares.totalSquaresY / 2;
    }

    // Update is called once per frame
    void Update()
    {
		//ship position and transform position are calculated by the 0,0 coordinate on the left up side, x+ to right and y+ to down
        float distanceX = (firstX + ship.position.x) - (firstX + transform.position.x);
        float distanceY = (firstY - transform.position.y) - (firstY - ship.position.y);
        float div = distanceY / distanceX;

		if (distanceX > 0)
		{
			if (div > 1.732)
			{
				//enemy is down and is aim 75°
				spriteIndex = 0;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

			}
			else if (div > 0.57773)
			{
				//enemy is down and is aim 45°
				spriteIndex = 1;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

			}
			else if (div > -0.25773)
			{
				if (up)
				{
					//enemy is up and is aim 0°
					spriteIndex = 2;
					GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
					transform.rotation = new Quaternion(180f, 0f, 0f, 0f);

				}
				else
				{
					//enemy is down and is aim 0°
					spriteIndex = 2;
					GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
					transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

				}

			}
			else if (div > -1.732)
			{
				//enemy is up and is aim -45°
				spriteIndex = 1;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(180f, 0f, 0f, 0f);

			}
			else
			{
				//enemy is up and is aim -75°
				spriteIndex = 0;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(180f, 0f, 0f, 0f);

			}
		}
		else
		{
			if (div > 1.732)
			{
				//enemy is up and is aim 285°
				spriteIndex = 0;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
			}
			else if (div > 0.57773)
			{
				//enemy is up and is aim 225°
				spriteIndex = 1;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(0f, 0f, 180f, 0f);

			}
			else if (div > -0.25773)
			{
				if (up)
				{
					//enemy is up and is aim 180°
					spriteIndex = 2;
					GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
					transform.rotation = new Quaternion(0f, 0f, 180f, 0f);

				}
				else
				{
					//enemy is down and is aim 180°
					spriteIndex = 2;
					GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
					transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

				}

			}
			else if (div > -1.732)
			{
				//enemy is down and is aim 135°
				spriteIndex = 1;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

			}
			else
			{
				//enemy is down and is aim 165°
				spriteIndex = 0;
				GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
				transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

			}
		}
	}
}
