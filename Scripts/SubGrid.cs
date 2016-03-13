using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubGrid : MonoBehaviour {

	public Transform spawnThis;

	private SquareGrid grid;
	
	private Vector2 subgridpos;

	public List <SubTileValues> subgridValues = new List<SubTileValues>();		//initializes list of class SubTileValues
	
	// Initialization
	void Start () {

		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();

		subgridValues.Clear ();													//make sure list is clear
		
		for( int i = 0; i < 2.0f*grid.x; i++) {
			for( int j = 0; j < 2.0f*grid.z; j++) {

				subgridpos.x = i*0.5f - 0.25f;									//nested for loop to spawn subtiles
				subgridpos.y = j*0.5f - 0.25f;

				Vector3 pos = new Vector3(subgridpos.x, 0, subgridpos.y);
				Transform SubTilePiece = Instantiate (spawnThis, pos, Quaternion.identity) as Transform;

				SubTilePiece.name = "SubTile " + subgridpos.x + ", " + subgridpos.y;		//rename them according to location

				SubTileValues instance = new SubTileValues(subgridpos.x, subgridpos.y, false, false);
				subgridValues.Add(instance);									//add them to list subgridValues<SubTileValues>
			}
		}

		//Destroy (GameObject.Find ("SubTile"), 0.2f);							//destroy base subtile, since there would be duplicate
	}
}
