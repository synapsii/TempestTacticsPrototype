using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SquareGrid : MonoBehaviour {

	//public Transform parent;

	public Transform spawnThis;

	public int x = 11;		//these variables are modifiable -- length/width of tile grid
	public int z = 11;

	private Vector2 gridpos;

	public List <TileValues> gridValues = new List<TileValues> ();


	void Start () {
	
		gridValues.Clear ();


		for( int i = 0; i < x; i++) {
			for( int j = 0; j < z; j++) {
				//Debug.Log("i = " + i +"; j = " + j);
				gridpos.x = i;
				gridpos.y = j;

				Vector3 pos = new Vector3(gridpos.x, 0, gridpos.y);
				Transform TilePiece = Instantiate (spawnThis, pos, Quaternion.identity) as Transform;

				TilePiece.name = "Tile " + gridpos.x + ", " + gridpos.y;

				//spawnThis.transform.parent = transform;

				TileValues instance = new TileValues(i, j, false, 0, false, 0, false, false);
				gridValues.Add(instance);

			}
		}

		Debug.Log("Total Tiles =" + gridValues.Count);				//shows total # tiles spawned

		//Destroy (GameObject.Find ("Tile"), 0.5f);					//Destroy original tile so no duplicates
	}
}
