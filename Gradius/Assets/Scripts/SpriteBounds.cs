using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteBounds
{
    public static float GetSpriteWidth(GameObject spr)
	{
		return spr.transform.localScale.x * spr.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
	}

	public static float GetSpriteHeight(GameObject spr)
	{
		return spr.transform.localScale.y * spr.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
	}

	//normalize the size of the object (1,1) and multiply those values for the width, and height respectively 
	public static void SetScaleSquare(GameObject obj, float width, float height)
    {
		SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
		float scaleX = (obj.transform.localScale.x / sp.sprite.bounds.size.x) * width;
		float scaleY = (obj.transform.localScale.y / sp.sprite.bounds.size.y) * height;
		obj.transform.localScale = new Vector2(scaleX, scaleY);
	}
}
