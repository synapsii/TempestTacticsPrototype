using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float ScrollSpeed = 15;
	public float ScrollEdge = 0.01f;
	public float PanSpeed = 10;
	
	public Vector2 zoomRange = new Vector2( -10, 100 );
	public float CurrentZoom = 0;
	public float ZoomZpeed = 1;
	public float ZoomRotation = 1;
	
	public Vector2 zoomAngleRange = new Vector2( 20, 70 );
	
	public float rotateSpeed = 10;
	
	private Vector3 initialPosition;
	private Vector3 initialRotation;

	private SquareGrid grid;

	void Start () {
		initialPosition = transform.position;      
		initialRotation = transform.eulerAngles;

		grid = GameObject.Find ("Grid").GetComponent<SquareGrid>();
	}
	
	
	void Update () {


		// middle click panning     
		if ( Input.GetMouseButton( 2 ) ) {
			transform.Translate(Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
			transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
		}

		else if ( Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) ) {
			//Debug.Log ("Alt held");
		}
		else 
		{
			//edge / UDLR panning

			if ( Input.GetKey("right") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge) ) {             

				if (transform.position.x < grid.x - 5.0f)
				{
					transform.Translate(Vector3.right * Time.deltaTime * PanSpeed, Space.Self );   
				}
			}
			else if ( Input.GetKey("left") || Input.mousePosition.x <= Screen.width * ScrollEdge) {            

				if (transform.position.x > 3.0f)
				{
				transform.Translate(Vector3.right * Time.deltaTime * -PanSpeed, Space.Self );        
				}
			}
			
			if ( Input.GetKey("up") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge) ) {            

				if (transform.position.z < grid.z - 7.0f)
				{
					transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed, Space.World );  
				}
			}
			else if ( Input.GetKey("down") || Input.mousePosition.y <= Screen.height * ScrollEdge ) {         

				if (transform.position.z > - 2.0f)
				{
					transform.Translate(Vector3.forward * Time.deltaTime * -PanSpeed, Space.World ); 
				}
			}  


			//camera rotation

//			if ( Input.GetKey("q")) {
//				transform.Rotate(Vector3.up * Time.deltaTime * -rotateSpeed, Space.World);
//			}
//			else if ( Input.GetKey("e")) {
//				transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
//			}
		}



		// zoom in/out
		CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * ZoomZpeed;
		
		CurrentZoom = Mathf.Clamp( CurrentZoom, zoomRange.x, zoomRange.y );
		
		transform.position = new Vector3( transform.position.x, transform.position.y - (transform.position.y - (initialPosition.y + CurrentZoom)) * 0.1f, transform.position.z );
		
		float x = transform.eulerAngles.x - (transform.eulerAngles.x - (initialRotation.x + CurrentZoom * ZoomRotation)) * 0.1f;
		x = Mathf.Clamp( x, zoomAngleRange.x, zoomAngleRange.y );
		
		transform.eulerAngles = new Vector3( x, transform.eulerAngles.y, transform.eulerAngles.z );
	}
}
