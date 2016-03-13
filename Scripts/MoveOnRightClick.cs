using UnityEngine;
using System.Collections;

public class MoveOnRightClick : MonoBehaviour {

	private UnitManager unitManager;
	private EnemyUnitManager enemyUnitManager;
	//private SquareGrid grid;

	private float distance;
	private float minDistance = Mathf.Infinity;

	private GameObject closestunit;

	private int newlistnumber;
	private int finallistnumber;
	private Vector3 newClickPosition;



	void Start () {
		unitManager = GameObject.FindGameObjectWithTag ("UnitManager").GetComponent<UnitManager>();
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();

		//grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
	}


	void RightClicked(Vector3 clickPosition)
	{

		foreach (GameObject unit in unitManager.selectedUnits)
		{
			BasicUnitMovement movement = unit.GetComponent<BasicUnitMovement>();
			movement.ShiftQueue.Clear();
		}


		//-----------------------------------RIGHT CLICK TO MOVE / SHIFT TO QUEUE

		bool unitsSelected = false;
		if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl))
		{
			if (unitManager.selectedUnits.Count > 1)
			{
				
				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					unitManager.SendMessage ("ShiftSplitMovement", clickPosition, SendMessageOptions.DontRequireReceiver);
					//Debug.Log ("shiftsplit1 sent");
				}
				else
				{
					unitManager.SendMessage ("SplitMovement", clickPosition, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (unitManager.selectedUnits.Count == 1)
			{
				foreach (GameObject unit in unitManager.GetSelectedUnits()) 
				{
					unitsSelected = true;

					unit.SendMessage ("MoveOrder", clickPosition);					//if units are selected, send move order
				}
			}

			if (enemyUnitManager.selectedUnits.Count == 1)
			{
				foreach (GameObject unit2 in enemyUnitManager.selectedUnits)
				{
					unit2.SendMessage ("MoveOrder", clickPosition, SendMessageOptions.DontRequireReceiver);
				}
			}
		}




		//-----------------------------------------CONTROL RCLICK GROUP-----------

		if ((Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) && unitManager.selectedUnits.Count >= 1)		//hold control and right click should tell only the closest unit to move there
		{

			foreach (GameObject unit in unitManager.GetSelectedUnits())
			{
				distance = Vector3.SqrMagnitude(clickPosition - unit.transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestunit = unit;
				}

				//Debug.Log (distance + " units away --- " + unit);
				//Debug.Log ("closest unit = " + closestunit);
			}

			minDistance = Mathf.Infinity;

			if (closestunit.GetComponent<BasicUnitMovement>().moving == false)				//removes unit from selection, but only if not already moving
			{

				if (closestunit.GetComponent<BasicUnitMovement>().movecd == 0)
				{
					unitManager.selectedUnits.Remove (closestunit);
				}
			}

			closestunit.SendMessage ("MoveOrder", clickPosition);					//sends move order to closest unit
			//Debug.Log ("closest only moves");
		}



		//-----------------------------------

		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && unitManager.selectedUnits.Count == 1)
		{

			foreach (GameObject unit in unitManager.GetSelectedUnits()) 
			{
				unit.SendMessage ("ShiftMove", clickPosition);
			}

		}


		if (unitsSelected)
		{
		//Instantiate (moveEffectObject, clickPosition, moveEffectObject.transform.rotation);
		}

		//-----------not sure what stuff directly above this does; it's not mine
	}





	void Update()
	{

		//---------Sends Stop order to unit: currently NOT implemented
		//
		//if (Input.GetKeyUp (KeyCode.S))
		//{
		//	foreach (GameObject unit in unitManager.GetSelectedUnits())
		//	{
		//		unit.SendMessage ("StopOrder");
		//	}
		//}


	}
}
