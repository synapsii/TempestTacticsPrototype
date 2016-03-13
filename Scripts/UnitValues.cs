using UnityEngine;
using System.Collections;

public class UnitValues {

	public float x;
	public float y;
	public float hp;
	public float moveSpeed;
	public float moveRange;
	public bool moving;
	public float movecd;
	public float attackRange;
	public int identifier;

	public UnitValues(float a, float b, float c, float d, float e, bool f, float g, float h, int i)
	{
		x = a;
		y = b;
		hp = c;
		moveSpeed = d;
		moveRange = e;
		moving = f;
		movecd = g;
		attackRange = h;
		identifier = i;

	}
}
