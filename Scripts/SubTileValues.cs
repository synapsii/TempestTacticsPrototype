using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubTileValues
{
	public float x;
	public float y;
	public bool occupied;  //not actually "occupied" in the sense that another unit can't be on it--just to detect collision
	public bool aoeffect;
	
	public SubTileValues(float a, float b, bool c, bool d)
	{
		x = a;
		y = b;
		occupied = c;
		aoeffect = d;

	}
}
