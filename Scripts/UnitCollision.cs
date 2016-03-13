using UnityEngine;
using System.Collections;

public class UnitCollision : MonoBehaviour {

	public new GameObject gameObject;
	public int id;							//need to make this actually detect ID of unit; have each unit assigned an ID
											//right now I have the IDs manually set in the Inspector.


	private UnitManager unitmanager;
	//private SquareGrid grid;
	//private SubGrid subgrid;

	private int posx;
	private int posy;
	private int subcolumns;
	private int sublistnumber;

	private string notux;				//name of tile underneath
	private string notuy;
	
	void Start () {

		unitmanager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		//grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		//subgrid = GameObject.Find ("SubGrid").GetComponent<SubGrid>();

	}

	void RightClicked () {									//tell the tile beneath this unit that it's been right-clicked

		posx = (int)Mathf.Round (transform.position.x);
		posy = (int)Mathf.Round (transform.position.z);		

		notux = posx.ToString ();
		notuy = posy.ToString ();
		Debug.Log (notux + ", " + notuy);


		GameObject.Find("Tile " + notux + ", " + notuy).SendMessage("RightClicked", transform.position);

	}
	

	void Update () {
	
		unitmanager.UnitList[id].x = 0.5f * Mathf.Round(transform.position.x * 2.0f);
		unitmanager.UnitList[id].y = 0.5f * Mathf.Round(transform.position.z * 2.0f);



	}	

}
