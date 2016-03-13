using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	private int occupancy;
	private float mousex;
	private float mousey;
	private Vector3 mousePosition;

	private float selectdur;
	public Vector2 TilePosition;
	private bool moving;

	private Color highlighted = new Color (0.5f, 0.67f, 0.92f, 0.7f);
	private Color shortrangeColor = new Color (0.57f, 0.74f, 0.9f, 0.6f);
	private Color rangeColor = new Color (0.70f, 0.85f, 0.95f, 0.6f);
	private Color clickedColor = new Color (0.3f, 0.5f, 0.8f, 0.7f);
	private Color occupiedColor = new Color (0.12f, 0.49f, 0.32f, 1.0f);
	private Color enemyOccupiedColor = new Color (0.85f, 0.2f, 0.1f, 1.0f);
	private Color originalColor;

	private DetectClicks mouse;
	private SquareGrid grid;
	//private SubGrid subgrid;
	private UnitManager unitManager;
	private EnemyUnitManager enemyUnitManager;
	private BasicUnitMovement movement;
	private EnemyUnitMovement movement2;
	
	public int columns;
	private int listnumber;

	private bool inrange = false;
	private bool inshortrange = false;

	//------casting

	private UnitBattle unitBattle;
	private float targetDistance;
	private float minDistance = Mathf.Infinity;
	private GameObject attackmovetarget;


	void Start () 
	{
		transform.parent = GameObject.Find("Grid").transform;

		mouse = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<DetectClicks>();
		unitManager = GameObject.FindGameObjectWithTag ("UnitManager").GetComponent<UnitManager>();
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();

		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		//subgrid = GameObject.Find ("SubGrid").GetComponent<SubGrid>();

		selectdur = 0.0f;

		TilePosition.x = transform.position.x;
		TilePosition.y = transform.position.z;

		originalColor = renderer.material.color;
		//Debug.Log (TilePosition);

		columns = grid.z;

		listnumber = (columns * (int)TilePosition.x) + (int)TilePosition.y;

	}

	void OnMouseUp()
	{
		if (Vector2.SqrMagnitude(TilePosition - mouse.currentPosition) < 1.0f)
		{
			selectdur = 0.35f;
			//Debug.Log ("correct" + selectdur);
		}
	}
	
	void Clicked()
	{

		//selectdur = 0.2f;									//creates timer for how long the tile should stay blue

		//Debug.Log ("left clicked " + TilePosition);

		Debug.Log("occupancy = " + occupancy);

		if (unitManager.casttype == 99)
		{
			foreach (GameObject unit in unitManager.firstlist)
			{
				if ((int)unit.transform.position.x == (int)TilePosition.x && (int)unit.transform.position.z == (int)TilePosition.y )
				{
					unit.SendMessage ("Clicked");
					//only sending message to unit 0 for some reason
					//should send order to unit 1 first
					//then unit 0 (due to creation order)
					//Debug.Log (unit.name);
					
				}
			}
		}

														//---------------NEED TO MAKE CONDITION FOR CASTTYPE == 99!!!!!!!!!!!!

		foreach (GameObject unit2 in enemyUnitManager.enemylist)
		{
			if ((int)unit2.transform.position.x == (int)TilePosition.x && (int)unit2.transform.position.z == (int)TilePosition.y)
			{
				unit2.SendMessage ("Clicked");
				unitManager.selectedUnits.Clear ();									//----------------DISABLE THIS LATER ON ONCE ENEMY MOVING IS DISABLED TOO//
				Debug.Log ("enemy unit clicked: " + unit2.name);
			}		

		}


		//----------------------------------------------------



		if (unitManager.casttype == 3)
		{
			foreach (GameObject unit in unitManager.selectedUnits)
			{

				foreach (GameObject unit2 in enemyUnitManager.enemylist)
				{
					targetDistance = Vector3.SqrMagnitude(transform.position - unit2.transform.position);

					if (targetDistance < minDistance)
					{
						targetDistance = minDistance;
						attackmovetarget = unit2;
					}

					minDistance = Mathf.Infinity;
				}

				//find nearest enemy
				unit.SendMessage ("Attack", attackmovetarget);
			}

		}
			

			




	}
	


	void ShowRange ()
	{
		inrange = true;
		//Debug.Log ("showrange");
	}

	void ShowShortRange ()
	{
		inshortrange = true;
	}

//	void DontShowRange ()
//	{
//		inrange = false;
//	}


	void Update ()
	{
		//occupancy = # units who have this tile as their position
		occupancy = 0;

//		if (inrange == true)
//		{
//			renderer.material.color = highlighted2;
//		}
//		else
//		{
//			renderer.material.color = originalColor;
//		}

		foreach (GameObject unit in unitManager.firstlist)
		{
			movement = unit.GetComponent<BasicUnitMovement>();

			if (unit.transform.position.x == TilePosition.x && unit.transform.position.z == TilePosition.y && !movement.moving)
			{
				occupancy++;
			}

			//Debug.Log (movement.moving);
		}

		foreach(GameObject unit in enemyUnitManager.enemylist)
		{
			movement2 = unit.GetComponent<EnemyUnitMovement>();

			if (unit.transform.position.x == TilePosition.x && unit.transform.position.z == TilePosition.y && !movement2.moving)
			{
				occupancy++;
			}
		}

		grid.gridValues[listnumber].occupancy = occupancy;
		//Debug.Log (grid.gridValues[listnumber].occupancy + "at Tile #" + listnumber);

		mousex = mouse.currentPosition.x;
		mousey = mouse.currentPosition.y;

//		if (grid.gridValues[listnumber].occupied == true && grid.gridValues[listnumber].enemyoccupied == false)
//		{
//			renderer.material.color = occupiedColor;
//		}
//		else
//		{
//			renderer.material.color = originalColor;
//		}


		if (selectdur > 0.0f) 
		{
			selectdur -= Time.deltaTime;					//counts down from timer set by left clicking

			if(selectdur < 0.05f) 							//could try Color.Lerp for better fade -- this is just for fun.
			{
				renderer.material.color = highlighted;		//fades by going to highlighted color for .05 sec

				if(selectdur < 0.0f)
				{
					selectdur = 0.0f;						//snaps selectdur back to 0 seconds
				}

			}
			else
			{
				renderer.material.color = clickedColor;		//color for left clicked tile

			}
		}
		else
		{
			if (grid.gridValues[listnumber].rightclicked == true)
			{
				renderer.material.color = clickedColor;		//if the tile is considered "rightclicked" in TileList, it stays blue.
			}
			else
			{
				if (grid.gridValues[listnumber].occupied == true)
				{
					renderer.material.color = occupiedColor;
				}
				else if (grid.gridValues[listnumber].enemyoccupied == true)
				{
					renderer.material.color = enemyOccupiedColor;
				}
				else if (inrange == true)
				{
					if (inshortrange == true)
					{
						renderer.material.color = shortrangeColor;
					}
					else
					{
						renderer.material.color = rangeColor;
					}
				}

				else if (inrange == false && inshortrange == false)
				{
					renderer.material.color = originalColor;	//otherwise back to original color
				}
			}
			if (Mathf.Abs (mousex - TilePosition.x) < 0.5f && Mathf.Abs (mousey - TilePosition.y) < 0.5f)
			{
				
				if (!Input.GetMouseButton(0))
				{
					renderer.material.color = highlighted;		//if mouse hover position is close to tile, show highlighted color
				}
			}

		}



		inrange = false;
		inshortrange = false;

	}
}
