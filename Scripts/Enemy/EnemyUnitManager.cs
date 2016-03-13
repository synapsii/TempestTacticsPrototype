using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnitManager : MonoBehaviour {

	public GameObject[] enemylist;
	public List <UnitValues> EnemyUnitList = new List<UnitValues>();
	public EnemyUnitCollision enemyUnit;


	public List<GameObject> selectedUnits;

	//private SquareGrid grid;
	private int x = 0; //unit id placeholder



	void Start () {
	
		//grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		
		//selectedUnits.Clear ();
		
		enemylist = GameObject.FindGameObjectsWithTag("EnemyUnit");
		Debug.Log ("There are " + enemylist.Length + " enemy units on the field");

		for (int i = 0; i < enemylist.Length; i++)
		{
			
			foreach(GameObject gameobject in enemylist)
			{

				enemyUnit = gameobject.GetComponent<EnemyUnitCollision>();
				x = enemyUnit.id;
				//x = 0;
				
				
				if (x == i)
				{
					UnitValues instance = new UnitValues(0.0f, 0.0f, 100.0f, 10.0f, 5.3f, false, 2.0f, 0.5f, x);
					EnemyUnitList.Add (instance);
					//creates list with UnitValues for each unit
					//need to implement ID for each unit rather than assigning them from 0 to #

					//Debug.Log (EnemyUnitList[0].moveRange);
				}
			}
		}

		//Debug.Log (EnemyUnitList[0].moveRange);

	}



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
		selectedUnits.Add (unit);
		//Debug.Log (selectedUnits);
		//unit.SendMessage ("ShowRange");
	}







	void Update () {
	
	}
}
