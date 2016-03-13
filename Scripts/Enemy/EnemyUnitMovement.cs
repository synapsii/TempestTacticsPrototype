using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnitMovement : MonoBehaviour
{
	private SquareGrid grid;
	private EnemyUnitManager enemyUnitManager;
	public EnemyUnitCollision unit;
	
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
	//private float minDistance = Mathf.Infinity;
	
	
	
	
	void Start()
	{
		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();
		unit = gameObject.GetComponent<EnemyUnitCollision>();
		
		movecdglobal = 2.0f;		//global movement cooldown
		movecdshort = 1.0f;
		
		columns = grid.z;
		
		goal = transform.position;			//no movement at start
		
		moving = false;						//starts with no movement command

		//Debug.Log (unit.id);
		//Debug.Log (enemyUnitManager.EnemyUnitList.Count);
		//unitRange = enemyUnitManager.EnemyUnitList[unit.id].moveRange;

		unitRange = 5.3f;
	}
	
	
	
	
	
	//-------------------------------------------------BASIC MOVEMENT COMMAND SENT TO UNIT--------------------------
	
	public void MoveOrder(Vector3 newGoal)
	{
		if (movecd <= 0.0f && !moving && !(Mathf.Round (newGoal.x) == transform.position.x && Mathf.Round (newGoal.z) == transform.position.z)) {		//check that movement is not on cd and not already moving
			
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
				//grid.gridValues[listnumber].rightclicked = true;
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
				//grid.gridValues[listnumber].rightclicked = true;
				
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
			//grid.gridValues[listnumber].rightclicked = false;
			
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
			grid.gridValues[newlistnumber].enemyoccupied = true;					//tell tile it's occupied.
			stationarylistnumber = newlistnumber;
		}
		else
		{
			grid.gridValues[stationarylistnumber].enemyoccupied = false;
		}
		
		
		
		
		
		if (!moving && grid.gridValues[newlistnumber].occupancy > 1)		//if grid has more than 1 unit on it, 
		{
			for (int j = 0; j < enemyUnitManager.EnemyUnitList.Count; j++)
			{
				if (j == unit.id)
				{
					for (int i = 0; i < grid.z*grid.x; i++)						//right now looks through every single tile
					{															//can optimize to look through 5x5 or 7x7
						tileloc.x = grid.gridValues[i].x + Random.Range (-0.02f, 0.02f);	// randomizes left or right
						tileloc.y = 0.0f;
						tileloc.z = grid.gridValues[i].y + Random.Range (-0.02f, 0.02f);
						dist = Vector3.SqrMagnitude(transform.position - tileloc);	//calculates distance between current position and tile
						
						if (dist < minDist && grid.gridValues[i].enemyoccupied == false && grid.gridValues[i].occupied == false)
						{
							minDist = dist;
							goal.x = Mathf.Round(tileloc.x);								//new goal is the closest tile
							goal.z = Mathf.Round(tileloc.z);								//problem -- for each unit, it only works once
							//Debug.Log (i + ", " + goal);					//then it ignores secondary + primary collision entirely if there is a secondary
						}
						
					}
					grid.gridValues[newlistnumber].occupancy = grid.gridValues[newlistnumber].occupancy - 1;
					
					minDist = Mathf.Infinity;
					j = enemyUnitManager.EnemyUnitList.Count + 10;
				}
			}
		}
		
		
		
		
		
		//-----------------------------Secondary collision check:
		
		if (moving == true && Vector3.SqrMagnitude (goal - transform.position) < 1.0f)	//if we're within <distance> of target
		{
			
			if (grid.gridValues[listnumber].enemyoccupied == true)				//if that target is occupied, look for closest tile
			{
				//Debug.Log ("OCCUPIED!");
				
				if (grid.gridValues[newlistnumber].enemyoccupied == false)		//if current closest tile is open
				{
					if (Vector3.SqrMagnitude(goal - origin) < 2.1f)
					{
						nomovecd = true;
					}
					
					goal.x = Mathf.Round (transform.position.x);			//new goal is that tile
					goal.z = Mathf.Round (transform.position.z);
					
					
				}
				else if (grid.gridValues[newlistnumber].enemyoccupied == true)		//if current closest tile is occupied
				{
					
					
					for (int i = 0; i < grid.z*grid.x; i++)						//right now looks through every single tile
					{															//can optimize to look through 5x5 or 7x7
						tileloc.x = grid.gridValues[i].x - Random.Range (-0.02f, 0.02f);	// randomizes left or right
						tileloc.y = 0.0f;
						tileloc.z = grid.gridValues[i].y - 0.01f;
						dist = Vector3.Magnitude(transform.position - tileloc);	//calculates distance between current position and tile
						
						if (dist < minDist && grid.gridValues[i].enemyoccupied == false)
						{
							minDist = dist;
							goal.x = tileloc.x;								//new goal is the closest tile
							goal.z = tileloc.z;								//problem -- for each unit, it only works once
							//Debug.Log (i + ", " + goal);					//then it ignores secondary + primary collision entirely if there is a secondary
						}
					}
					
					minDist = Mathf.Infinity;
					
				}
				//grid.gridValues[listnumber].rightclicked = false;			//unhighlights original target
				
				listnumber = newlistnumber;
				//grid.gridValues[listnumber].rightclicked = true;			//highlights tile you will be pushed to
				
				
			}
			
		}

	}
}
