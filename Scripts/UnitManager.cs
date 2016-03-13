using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

	public List<GameObject> selectedUnits;
	public List<List<GameObject>> ControlGroups = new List<List<GameObject>>();
	public List<GameObject> example;
	
	//using list for now:
	public List <UnitValues> UnitList = new List<UnitValues>();
	public UnitCollision unit;

	public GameObject[] firstlist;
	private int x = 0;
	public bool dragging;

	private string unitID;

	//------------------multiple unit movement

	private SquareGrid grid;

	private float distance;
	private float minDistance = Mathf.Infinity;
	
//	private int newlistnumber;
//	private int finallistnumber;
	private Vector3 testClickPosition;
	private Vector3 newClickPosition;

	private float unitRange;
	private GameObject unittest;

	private int t;

	//----------------shift multiple unit movement

	public List<GameObject> shiftUnits;

	//---------------enemy unit stuff

	private EnemyUnitManager enemyUnitManager;

	//---------------casting #

	public int casttype = 99;


	void Start () {

		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();

		selectedUnits.Clear ();

		firstlist = GameObject.FindGameObjectsWithTag("Unit");
		Debug.Log ("You have " + firstlist.Length + " units on the field");

		for (int i = 0; i < firstlist.Length; i++)
		{

			foreach(GameObject gameobject in firstlist)
			{

				unit = gameobject.GetComponent<UnitCollision>();
				x = unit.id;



				if (x == i)
				{
					UnitValues instance = new UnitValues(0.0f, 0.0f, 100.0f, 10.0f, 5.3f, false, 2.0f, 0.5f, x);
					UnitList.Add (instance);
																//creates list with UnitValues for each unit
																//need to implement ID for each unit rather than assigning them from 0 to #
				}
			}
		}

		for (int j = 0; j < 10; j++)
		{
			//unitID = j.ToString();

			//example.Add (GameObject.FindGameObjectsWithTag("Unit"));
			ControlGroups.Add (new List<GameObject>());
			ControlGroups[j].Clear();

			//Debug.Log (j);
		}

		//Debug.Log (UnitList[0].identifier + ", " + UnitList[1].identifier + ", " + UnitList[2].identifier);

	}

	void DraggingTrue ()
	{
		if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
		{
			selectedUnits.Clear();
		}
		dragging = true;
		Debug.Log ("dragging = " + dragging);
	}

	void DraggingFalse ()
	{
		dragging = false;
		Debug.Log ("dragging = " + dragging);
	}


	//-----------------------------------------------------multiple unit movement




	void SplitMovement (Vector3 clickPosition)															//comes from BasicUnitMovement
	{
		selectedUnits[0].SendMessage("MoveOrder", clickPosition);										//first unit moves normally
//		newlistnumber = (int)(grid.z * Mathf.Round(clickPosition.x) + Mathf.Round (clickPosition.z));


		//--------------------------------------OLD------------------
//
//		for (int i = 1; i < selectedUnits.Count; i++)													//go through every other unit selected
//		{
//			
//			for (int t = 0; t < grid.x * grid.z; t++)													//check every tile
//			{
//				testClickPosition.x = grid.gridValues[t].x;												//convert listnumber to position
//				testClickPosition.y = 0.0f;
//				testClickPosition.z = grid.gridValues[t].y;
//				
//				//unittest = selectedUnits[i];
//				unitRange = selectedUnits[i].GetComponent<BasicUnitMovement>().unitRange;
//				
//				if (Vector3.SqrMagnitude(testClickPosition - selectedUnits[i].transform.position) < unitRange * unitRange)	//if tilepos-fuck
//				{
//					distance = Vector3.SqrMagnitude(testClickPosition - clickPosition);
//					
//					if (distance < minDistance && grid.gridValues[t].rightclicked == false && grid.gridValues[t].occupied == false)
//					{
//						minDistance = distance;
//						//finallistnumber = t;
//						newClickPosition = testClickPosition;
//					}
//					
//				}
//			}

			//-----------------------------OPTIMIZED BUT EDGE ISSUES------------------------fixed 3/10/16

		for (int i = 1; i < selectedUnits.Count; i++)										//go through every other unit selected
		{

			for (int j = -2; j < 3; j++)													//check every tile
			{
				for (int k = -2; k < 3; k++)
				{

					t = (Mathf.RoundToInt (clickPosition.x) + j)*grid.z + Mathf.RoundToInt (clickPosition.z) + k;

					if (t >= 0 && t < grid.x*grid.z)
					{

						
						testClickPosition.x = grid.gridValues[t].x;										//convert listnumber to position
						testClickPosition.y = 0.0f;
						testClickPosition.z = grid.gridValues[t].y;
						
						//unittest = selectedUnits[i];
						unitRange = selectedUnits[i].GetComponent<BasicUnitMovement>().unitRange;
						
						if (Vector3.SqrMagnitude(testClickPosition - selectedUnits[i].transform.position) < unitRange * unitRange)	//if tilepos-fuck
						{
							distance = Vector3.SqrMagnitude(testClickPosition - clickPosition);
							
							if (distance < minDistance && grid.gridValues[t].rightclicked == false && grid.gridValues[t].occupied == false)
							{
								minDistance = distance;
								//finallistnumber = t;
								newClickPosition = testClickPosition;
							}
							
						}
					}
				}
			}

			selectedUnits[i].SendMessage ("MoveOrder", newClickPosition);

			//grid.gridValues[finallistnumber].rightclicked = true;
			
			minDistance = Mathf.Infinity;
		}
	}



	//-----------------------------------------------------SHIFTSPLIT

	void ShiftSplitMovement (Vector3 clickPosition)
	{

		foreach (GameObject unit in selectedUnits)
		{
			unit.SendMessage ("ShiftSplitMove", clickPosition);
			Debug.Log("splitqueued2" + unit);
		}

	}






	//-----------------------------------------------------selection functions below



	public bool IsSelected(GameObject unit)
	{
		if (selectedUnits.Contains (unit)) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}

	public void SelectSingleUnit(GameObject unit)
	{
		selectedUnits.Clear ();
		enemyUnitManager.selectedUnits.Clear ();

		selectedUnits.Add (unit);
		//Debug.Log (selectedUnits);
		//unit.SendMessage ("ShowRange");
	}
	
	public void SelectAdditionalUnits(GameObject unit)
	{
		enemyUnitManager.selectedUnits.Clear ();

		selectedUnits.Add (unit);
		//unit.SendMessage ("ShowRange");
	}
	
	public void DeselectSingleUnit(GameObject unit)
	{
		selectedUnits.Remove (unit);
//		foreach (GameObject unit2 in firstlist)
//		{
//			if (!selectedUnits.Contains(unit2))
//			{
//				unit2.SendMessage ("DontShowRange");
//			}
//		}
	}
	
	public void DeselectAllUnits()
	{
		selectedUnits.Clear ();


//		foreach (GameObject unit2 in firstlist)
//		{
//			unit2.SendMessage ("DontShowRange");
//		}


		//Debug.Log (selectedUnits);
	}
	
	public List<GameObject> GetSelectedUnits()
	{
		return selectedUnits;
	}








	void Update ()
	{
//		foreach (GameObject unit in firstlist)
//		{
//			if (!selectedUnits.Contains (unit))
//			{
//				Debug.Log (unit);
//			}
//		}


		foreach (GameObject unit in firstlist)
		{
			if (selectedUnits.Contains (unit))
			{
				//Debug.Log ("showrange");
				unit.SendMessage ("ShowRange");			//will do this part in BasicUnitMovement script
			}

		}

		
		if (Input.GetKeyDown(KeyCode.A))
		{
			foreach (GameObject unit in selectedUnits)
			{
				casttype = 3;

			}
		}




		//-----------------CONTROL GROUPS-------------------------


		if((Input.GetKey (KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
		{
			if (Input.GetKey (KeyCode.Alpha1))
			{
				ControlGroups[1].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 1, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (Input.GetKey (KeyCode.Alpha2))
			{
				ControlGroups[2].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 2, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (Input.GetKey (KeyCode.Alpha3))
			{
				ControlGroups[3].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 3, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (Input.GetKey (KeyCode.Alpha4))
			{
				ControlGroups[4].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 4, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (Input.GetKey (KeyCode.Alpha5))
			{
				ControlGroups[5].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 5, SendMessageOptions.DontRequireReceiver);
				}
			}

			if (Input.GetKey (KeyCode.Alpha6))
			{
				ControlGroups[6].Clear();
				foreach (GameObject unit in selectedUnits)
				{
					unit.SendMessage("AddToControlGroup", 6, SendMessageOptions.DontRequireReceiver);
				}
			}

		}


		if (Input.GetKey (KeyCode.Alpha1))
		{
			DeselectAllUnits();

			foreach (GameObject unit in ControlGroups[1])
			{
				SelectAdditionalUnits(unit);
			}
		}

		if (Input.GetKey (KeyCode.Alpha2))
		{
			DeselectAllUnits();
			
			foreach (GameObject unit in ControlGroups[2])
			{
				SelectAdditionalUnits(unit);
			}
		}

		if (Input.GetKey (KeyCode.Alpha3))
		{
			DeselectAllUnits();
			
			foreach (GameObject unit in ControlGroups[3])
			{
				SelectAdditionalUnits(unit);
			}
		}

		if (Input.GetKey (KeyCode.Alpha4))
		{
			DeselectAllUnits();
			
			foreach (GameObject unit in ControlGroups[4])
			{
				SelectAdditionalUnits(unit);
			}
		}

		if (Input.GetKey (KeyCode.Alpha5))
		{
			DeselectAllUnits();
			
			foreach (GameObject unit in ControlGroups[5])
			{
				SelectAdditionalUnits(unit);
			}
		}

		if (Input.GetKey (KeyCode.Alpha6))
		{
			DeselectAllUnits();
			
			foreach (GameObject unit in ControlGroups[6])
			{
				SelectAdditionalUnits(unit);
			}
		}
	}
}
