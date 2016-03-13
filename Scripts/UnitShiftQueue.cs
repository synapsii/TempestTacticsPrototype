using UnityEngine;
using System.Collections;

public class UnitShiftQueue {

	public int action;
	public Vector3 position;
	public GameObject target;

	public UnitShiftQueue(int a, Vector3 b, GameObject c)
	{
		action = a;
		position = b;
		target = c;

//		ACTIONS:
//		0 = identifier
//		1 = move
//		2 = splitmove
//		3 = a-move
//		4 = ( fill later )
//		5 = Q
//		6 = W
//		7 = E
//		8 = R
//		9 = D
//		10 = F
//		99 = do nothing

	}

	void Start ()
	{

	}

}
