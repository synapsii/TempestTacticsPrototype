using UnityEngine;
using System.Collections;

public class PlaneDetection : MonoBehaviour {

	private int planewidth;
	private int planeheight;

	public new GameObject gameObject;

	private SquareGrid grid;
	private DetectClicks mouse;
	
	private int posx;
	private int posy;
	private string notux;				//name of tile nearest
	private string notuy;

	private Vector3 clicklocation;

//	public Transform spawnThis;
	
	// This script attaches to an invisible, collision-active plane to ensure that you cannot click in-between tiles.
	// Tiles do not fill up the entire grid (scale = 0.095 or something instead of scale = 0.10) for aesthetic reasons.


	void Start () {
	
		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
		mouse = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<DetectClicks>();

		planewidth = grid.x - 1;
		planeheight = grid.z - 1;

		transform.position = new Vector3 (0.5f * planewidth, -0.001f, 0.5f * planeheight);		//put it just below so it doesn't interfere
		transform.localScale = new Vector3 (0.1f * (planewidth + 10.0f) * 2.0f, 1, 0.1f * (planeheight + 1.0f) * 2.0f);	//make it the right size

//		for (int i = 0; i < 6; i++)
//		{
//			for (int j = 0; j < 6; j++)
//			{
//				Vector3 pos = new Vector3(i * 10.0f, -0.001f, j * 10.0f);
//
//				Transform TilePiece = Instantiate (spawnThis, pos, Quaternion.identity) as Transform;
//				Debug.Log ("plane created");
//			}
//		}

	}

	void Clicked () {
		//sendmessage to tile "clicked"


		clicklocation.x = (mouse.currentPosition.x);
		clicklocation.y = 0.0f;
		clicklocation.z = (mouse.currentPosition.y);
		
		posx = (int)Mathf.Round (clicklocation.x);
		posy = (int)Mathf.Round (clicklocation.z);		

		if(posx >= 0 && posy >= 0 && posx <= grid.x && posy <=grid.z)
		{
			notux = posx.ToString ();
			notuy = posy.ToString ();
			//Debug.Log (notux + ", " + notuy);

			GameObject.Find("Tile " + notux + ", " + notuy).SendMessage("Clicked", clicklocation, SendMessageOptions.DontRequireReceiver);
		}

	}

	void RightClicked () {
		//sendmessage to tile "rightclicked"

		clicklocation.x = (mouse.currentPosition.x);
		clicklocation.y = 0.0f;
		clicklocation.z = (mouse.currentPosition.y);

		posx = (int)Mathf.Round (clicklocation.x);
		posy = (int)Mathf.Round (clicklocation.z);		

		if(posx >= 0 && posy >= 0 && posx <= grid.x && posy <=grid.z)
		{
			notux = posx.ToString ();
			notuy = posy.ToString ();
			//Debug.Log (notux + ", " + notuy);

			GameObject.Find("Tile " + notux + ", " + notuy).SendMessage("RightClicked", clicklocation, SendMessageOptions.DontRequireReceiver);
		}
	}

}
