using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasicUnitMovement : MonoBehaviour
{
	private SquareGrid grid;
	private UnitManager unitmanager;
	public UnitCollision unit;
	private UnitBattle unitBattle;

	private float moveSpeed = 10.0f;  		//movement speed
	private Vector3 goal;					//xyz for cursor target
	
	public float movecdglobal = 3.0f;		//global movement cooldown
	public float movecdshort = 1.5f;
	public float movecd = 0.0f;				//movement cooldown timer
	private bool nomovecd;
	public bool moving = false;				//check if moving
	private bool shortmoving = false;

	private int listnumber;
	private int newlistnumber;
	private int stationarylistnumber;
	private int columns;

	private float dist;
	private float minDist = Mathf.Infinity;
	private Vector3 tileloc;
	private Vector3 closestTile;

	public int collisionPriority;

	private Vector3 tiledist;
	private Vector3 tiledist2;
	private GameObject tile;
	private float CTIRdistance;
	public float unitRange;

	private Vector3 origin;


	public int tilerangecheck;

	//SHIFT QUEUE

	public Queue <UnitShiftQueue> ShiftQueue = new Queue <UnitShiftQueue>();
	private Vector3 shiftgoal;

	//SHIFT SPLIT

	private Vector3 testClickPosition;
	private Vector3 newClickPosition;

	private float distance;
	private float minDistance = Mathf.Infinity;



	
	void Start()
	{
		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		unitmanager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		unit = GetComponent<UnitCollision>();
		unitBattle = gameObject.GetComponent<UnitBattle>();

		movecdglobal = 2.0f;		//global movement cooldown
		movecdshort = 1.0f;

		columns = grid.z;

		goal = transform.position;			//no movement at start

		moving = false;						//starts with no movement command

		unitRange = unitmanager.UnitList[unit.id].moveRange;
	}





	//-------------------------------------------------BASIC MOVEMENT COMMAND SENT TO UNIT--------------------------

	public void MoveOrder(Vector3 newGoal)
	{
		if (movecd <= 0.0f && !moving && unitBattle.casting == false && !(Mathf.Round (newGoal.x) == transform.position.x && Mathf.Round (newGoal.z) == transform.position.z)) {		//check that movement is not on cd and not already moving

			if (Vector3.SqrMagnitude(newGoal - transform.position) <= unitRange * unitRange)
			{
				origin = transform.position;
				nomovecd = false;


					goal.x = Mathf.RoundToInt (newGoal.x);
					goal.y = 0.5f;
					goal.z = Mathf.RoundToInt (newGoal.z);

																			//goal set to clicked target point, estimate to tile
																			//target set to 0.5 units above plane
				moving = true;												//set status to moving
					
				if (Vector3.SqrMagnitude(goal - transform.position) < 6.25f)
				{
					shortmoving = true;				// if movement is < 2.5 units in length, you get shorter cd
					//Debug.Log ("shortmoving");
				}

				//Debug.Log ("Unit moving towards" + goal + "; move cd is " + movecd + " seconds.");
	
				listnumber = (int)(goal.x * columns + goal.z);
				grid.gridValues[listnumber].rightclicked = true;
			}
			else
			{													//if right click out of unit range,
																//find nearest tile in range, make that the new goal.
				for (int t = 0; t < grid.x * grid.z; t++)
				{
					tiledist.x = grid.gridValues[t].x;
					tiledist.y = 0.0f;
					tiledist.z = grid.gridValues[t].y;

					if (Vector3.SqrMagnitude(tiledist - transform.position) < unitRange * unitRange)
					{
						CTIRdistance = Vector3.SqrMagnitude(tiledist - newGoal);

						if (CTIRdistance < minDist)
						{
							minDist = CTIRdistance;
							listnumber = t;
						}

					}
				}

				goal.x = Mathf.RoundToInt (grid.gridValues[listnumber].x);
				goal.y = 0.5f;
				goal.z = Mathf.RoundToInt (grid.gridValues[listnumber].y);
				
				moving = true;
				grid.gridValues[listnumber].rightclicked = true;

				//Debug.Log ("moving = " + moving + "; new goal is " + goal + "; closest tile is #" + listnumber);

				minDist = Mathf.Infinity;
			}
			//DontShowRange ();

		} else {
			
			if(movecd > 0.0f)
			{
				Debug.Log ("Movement on cd");
			}
			if(Vector3.Magnitude(newGoal - transform.position) < unitRange)
			{
				Debug.Log ("Movement out of range");
			}

			else if (moving)
			{
				Debug.Log ("Cannot issue move command while moving");
			}
		}
	}






	//--------------------------------------------OTHER MOVE COMMANDS------------------------------------------------

	void ShiftMove (Vector3 newGoal)
	{
		if (ShiftQueue.Count <= 10)
		{
			shiftgoal.x = Mathf.RoundToInt (newGoal.x);
			shiftgoal.y = 0.5f;
			shiftgoal.z = Mathf.RoundToInt (newGoal.z);
		

			if (ShiftQueue.Peek().action == 3)
			{
				ShiftQueue.Clear();
			}

			ShiftQueue.Enqueue(new UnitShiftQueue(1, shiftgoal, null));
			Debug.Log ("movement enqueued, # in queue = " + ShiftQueue.Count);
		}
	}



	void ShiftSplitMove (Vector3 clickPosition)
	{
		if (ShiftQueue.Count <= 10)
		{
			//Debug.Log ("splitmove enqueued" + gameObject.name);
			ShiftQueue.Enqueue(new UnitShiftQueue(2, clickPosition, null));								//send SPLITMOVE enqueue to self
		}
	}




	void SplitMove (Vector3 newGoal)
	{
		for (int t = 0; t < grid.x * grid.z; t++)													//check every tile
		{
			testClickPosition.x = grid.gridValues[t].x;												//convert listnumber to position
			testClickPosition.y = 0.0f;
			testClickPosition.z = grid.gridValues[t].y;
			
			if (Vector3.SqrMagnitude(testClickPosition - transform.position) < unitRange * unitRange)	//if tilepos-fuck
			{
				distance = Vector3.SqrMagnitude(testClickPosition - newGoal);
				
				if (distance < minDistance && grid.gridValues[t].rightclicked == false && grid.gridValues[t].occupied == false && grid.gridValues[t].enemyoccupied == false)
				{
					minDistance = distance;
					//finallistnumber = t;
					newClickPosition = testClickPosition;
				}
				
			}
		}
		
		gameObject.SendMessage ("MoveOrder", newClickPosition);
		minDistance = Mathf.Infinity;
		//Debug.Log ("splitmove" + gameObject.name);
	}







	//-------------------------------------------------Sends Message to all tiles in movement range to highlight

	void ShowRange ()
	{

		for (int j = -5; j < 6; j++)
		{
			for (int k = -5; k < 6; k++)
			{
				tilerangecheck = (Mathf.RoundToInt (transform.position.x) + j)*grid.z + Mathf.RoundToInt (transform.position.z) + k;

				if (tilerangecheck >= 0 && tilerangecheck < (grid.x)*grid.z)
				{

					tiledist.x = grid.gridValues[tilerangecheck].x;
					tiledist.y = 0;
					tiledist.z = grid.gridValues[tilerangecheck].y;

					if (Vector3.Magnitude(tiledist - transform.position) <= unitRange && !moving)
					{
						
						tile = GameObject.Find ("Tile " + Mathf.Round (grid.gridValues[tilerangecheck].x) + ", " + Mathf.Round (grid.gridValues[tilerangecheck].y));
						if (tile != null)
						{
							tile.SendMessage("ShowRange", SendMessageOptions.DontRequireReceiver);
							//Debug.Log ("showrange" + tile);
							
							if (Vector3.Magnitude(tiledist - transform.position) < 2.5f)
							{
								tile.SendMessage("ShowShortRange", SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
			}
		}

	}


	//-------------------------------------------------ATTACKING:



	void QueueAttack (GameObject enemy)
	{

		ShiftQueue.Enqueue(new UnitShiftQueue(3, transform.position, enemy));

	}




	//-------------------------------------------------UPDATE CHECKS:


	void Update()
	{
		if (movecd > 0.0f) {												//if movement is on cooldown
			movecd -= Time.deltaTime;										//incrementally lower movement cd
		} 
		else 
		{
			movecd = 0.0f;													//lock cd at 0 once below 0
			
		}

		if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.LeftAlt))		//hold alt to only show and move in shortrange
		{
			unitRange = 2.5f;
		}
		else
		{
			unitRange = 5.3f;
		}

		transform.position += (goal - transform.position).normalized*moveSpeed*Time.deltaTime;						//movement (normalize vector, transform in direction * ms)

		moveSpeed = Mathf.Clamp (Vector3.Magnitude(goal - transform.position) * 0.65f + 2.5f, 0.0f, 5.0f);			//movespeed function as distance to target
																													//clamped between 2.5 and 5.0
		if(Vector3.SqrMagnitude(goal - transform.position) < 0.06f * 0.06f)
		{
		
		transform.position = goal;											//snap to target position
		grid.gridValues[listnumber].rightclicked = false;
		
			if(moving && !shortmoving)										//makes sure movement cd only gets set after movement
			{

				movecd = movecdglobal;										//set movement cd back to global cd value
				moving = false;												//no longer under movement command		


			}
			else if(shortmoving)
			{
				if (nomovecd == false)
				{
					movecd = movecdshort;									//short movement gives half cd
				}
				else
				{
					movecd = 0;
				}

				shortmoving = false;
				moving = false;
			}
		}
		
		//----------------------------------below is a constantly refreshing tile position found with newlist#


		newlistnumber = (int)( columns * Mathf.Round (transform.position.x) + Mathf.Round (transform.position.z));		
		//Debug.Log (newlistnumber + " =? " + (columns * transform.position.x + transform.position.z));


		//------------------------------------------------------------------------------------------------------





		if (moving == false)												//occupied tile check
		{
			grid.gridValues[newlistnumber].occupied = true;					//tell tile it's occupied.
			stationarylistnumber = newlistnumber;
		}
		else
		{
			grid.gridValues[stationarylistnumber].occupied = false;
		}





		if (!moving && grid.gridValues[newlistnumber].occupancy > 1)		//if grid has more than 1 unit on it, 
		{
			for (int j = 0; j < unitmanager.UnitList.Count; j++)
			{
				if (j == unit.id)
				{
					for (int i = 0; i < grid.z*grid.x; i++)						//right now looks through every single tile
					{															//can optimize to look through 5x5 or 7x7
						tileloc.x = grid.gridValues[i].x + Random.Range (-0.02f, 0.02f);	// randomizes left or right
						tileloc.y = 0.0f;
						tileloc.z = grid.gridValues[i].y + Random.Range (-0.02f, 0.02f);
						dist = Vector3.SqrMagnitude(transform.position - tileloc);	//calculates distance between current position and tile
						
						if (dist < minDist && grid.gridValues[i].occupied == false && grid.gridValues[i].enemyoccupied == false)
						{
							minDist = dist;
							goal.x = Mathf.Round(tileloc.x);								//new goal is the closest tile
							goal.z = Mathf.Round(tileloc.z);								//problem -- for each unit, it only works once
							//Debug.Log (i + ", " + goal);					//then it ignores secondary + primary collision entirely if there is a secondary
						}

					}
					grid.gridValues[newlistnumber].occupancy = grid.gridValues[newlistnumber].occupancy - 1;

					minDist = Mathf.Infinity;
					j = unitmanager.UnitList.Count + 10;
				}
			}
		}





		//-----------------------------Secondary collision check:

		if (moving == true && Vector3.SqrMagnitude (goal - transform.position) < 1.0f)	//if we're within <distance> of target
		{

			if (grid.gridValues[listnumber].occupied == true || grid.gridValues[listnumber].enemyoccupied == true)				//if that target is occupied, look for closest tile
			{
				//Debug.Log ("OCCUPIED!");

				if (grid.gridValues[newlistnumber].occupied == false && grid.gridValues[newlistnumber].enemyoccupied == false)		//if current closest tile is open
				{
					if (Vector3.SqrMagnitude(goal - origin) < 2.1f)
					{
						nomovecd = true;
					}

					goal.x = Mathf.Round (transform.position.x);			//new goal is that tile
					goal.z = Mathf.Round (transform.position.z);


				}
				else if (grid.gridValues[newlistnumber].occupied == true || grid.gridValues[newlistnumber].enemyoccupied == true)		//if current closest tile is occupied
				{


					for (int i = 0; i < grid.z*grid.x; i++)						//right now looks through every single tile
					{															//can optimize to look through 5x5 or 7x7
						tileloc.x = grid.gridValues[i].x - Random.Range (-0.02f, 0.02f);	// randomizes left or right
						tileloc.y = 0.0f;
						tileloc.z = grid.gridValues[i].y - 0.01f;
						dist = Vector3.Magnitude(transform.position - tileloc);	//calculates distance between current position and tile
	
						if (dist < minDist && grid.gridValues[i].occupied == false && grid.gridValues[i].enemyoccupied == false)
						{
							minDist = dist;
							goal.x = tileloc.x;								//new goal is the closest tile
							goal.z = tileloc.z;								//problem -- for each unit, it only works once
							//Debug.Log (i + ", " + goal);					//then it ignores secondary + primary collision entirely if there is a secondary
						}
					}

					minDist = Mathf.Infinity;

				}
				grid.gridValues[listnumber].rightclicked = false;			//unhighlights original target

				listnumber = newlistnumber;
				grid.gridValues[listnumber].rightclicked = true;			//highlights tile you will be pushed to


			}

		}



		//-------------------------------------------------SHIFT QUEUE CHECKS-----------------

		if (Input.GetKey(KeyCode.S))
		{
			ShiftQueue.Clear();		//S for stop, clears any shift-queued actions
		}

		if (ShiftQueue.Count > 0 && movecd == 0 && !moving)
		{
			if (ShiftQueue.Peek().action == 1)
			{
//				goal.x = Mathf.Round (ShiftQueue.Peek().x);
//				goal.y = Mathf.Round (ShiftQueue.Peek().y);

				gameObject.SendMessage("MoveOrder", ShiftQueue.Peek ().position);
				Debug.Log ("shiftqueue dequeued; queue count = " + ShiftQueue.Count);

				ShiftQueue.Dequeue();
			}
			else if (ShiftQueue.Peek().action == 2)											//split move, i.e. multiple units don't target same tile on rclick
			{

				gameObject.SendMessage("SplitMove", ShiftQueue.Peek ().position);
				ShiftQueue.Dequeue();

				Debug.Log ("shiftqueue dequeued; queue count = " + ShiftQueue.Count);

			}
		}

		if (ShiftQueue.Count > 0 && unitBattle.casting == false && !moving)
		{
			if (ShiftQueue.Peek().action == 3)
			{
				
				gameObject.SendMessage("Attack", ShiftQueue.Peek ().target, SendMessageOptions.DontRequireReceiver);
				//ShiftQueue.Dequeue();
				
			}
		}
	}
}
