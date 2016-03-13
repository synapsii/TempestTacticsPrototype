using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileValues
{
	public int x;
	public int y;
	public bool occupied;
	public int elevation;
	public bool rightclicked;
	public int occupancy;
	public bool enemyoccupied;
	public bool neutraloccupied;

	public TileValues(int a, int b, bool c, int d, bool e, int f, bool g, bool h)
	{
		x = a;
		y = b;
		occupied = c;
		elevation = d;
		rightclicked = e;
		occupancy = f;
		enemyoccupied = g;
		neutraloccupied = h;

	}
}
