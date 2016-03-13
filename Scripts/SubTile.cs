using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubTile : MonoBehaviour {

	private UnitManager unitmanager;	
	private EnemyUnitManager enemyUnitManager;
	//private SubGrid subgrid;
	//public float columns;
	//private float listnumber;				//if we ever need to reference SubTileValues list, use listnumber
	
	private Vector3 SubTilePosition;		//will define this to be where the current tile is
	private Vector3 UnitPosition;			//variable (x,y,z) for checking individual units
	private Vector3 EnemyUnitPosition;

	private Color originalColor;			//define colors
	private Color occupiedColor = new Color(0.2f, 0.7f, 0.2f, 0.5f);
	private Color enemyOccupiedColor = new Color (0.7f, 0.15f, 0.1f, 0.6f);

	private int occupied = 0;


	void Start () 
	{
		transform.parent = GameObject.Find("SubGrid").transform;

		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();
		unitmanager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		//subgrid = GameObject.Find ("SubGrid").GetComponent<SubGrid>();

		//columns = (float)subgrid.z * 2.0f;

		SubTilePosition.x = (float)transform.position.x;						//set SubTilePosition to be (x,y,0) of the current subtile
		SubTilePosition.y = (float)transform.position.z;
		SubTilePosition.z = 0.0f;

		//listnumber = 2.0f * (columns * (SubTilePosition.x + 0.25f) + SubTilePosition.y + 0.25f);

		originalColor = renderer.material.color;
		
	}


	void Damage (float damage)
	{

	}


	void Stun (float duration)
	{

	}


	void AOEPrep (float duration)
	{

	}



	void Update ()
	{

		//Debug.Log (unitmanager.UnitList.Count);

		for (int i=0; i < unitmanager.UnitList.Count; i++)
		{
			UnitPosition.x = unitmanager.UnitList[i].x;							//for each unit in UnitList, grab its rounded x,y position
			UnitPosition.y = unitmanager.UnitList[i].y;
			UnitPosition.z = 0.0f;


			if (Vector3.SqrMagnitude(SubTilePosition - UnitPosition) < 0.525625f)		//if any are in range, then occupied will be >0
			{
				occupied++;

				renderer.material.color = occupiedColor;						//and the tile will turn red (occupied color)
				//Debug.Log (UnitPosition + " occupied by Unit " + i);
			}
		}

		for (int j=0; j < enemyUnitManager.EnemyUnitList.Count; j++)
		{
			EnemyUnitPosition.x = enemyUnitManager.EnemyUnitList[j].x;							//for each unit in UnitList, grab its rounded x,y position
			EnemyUnitPosition.y = enemyUnitManager.EnemyUnitList[j].y;
			EnemyUnitPosition.z = 0.0f;
			
			
			if (Vector3.SqrMagnitude(SubTilePosition - EnemyUnitPosition) < 0.525625f)		//if any are in range, then occupied will be >0
			{

				occupied++;
				
				renderer.material.color = enemyOccupiedColor;						//and the tile will turn red (occupied color)
				//Debug.Log (UnitPosition + " occupied by Unit " + i);

			}
		}


		if (occupied == 0)														//if none are in range, 
		{
			renderer.material.color = originalColor;
		}

		occupied = 0;

	}
}



