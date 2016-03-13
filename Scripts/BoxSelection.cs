using UnityEngine;
using System.Collections;

public class BoxSelection : MonoBehaviour {
	
	//on LMB down, instantiate plane at y = 0.5
	//corner of plane is at where mouse is when LMB down
	
	//on LMB up, select all gameobjects tagged unit that are in collision with plane
	
	//Update:
	//if LMB is held, plane resizes according to where mouse is
	
	public GameObject selector;
	private UnitManager unitManager;
	
	private Vector3 corner;
	private GameObject selectorInstance;

	private int click;
	private float dragfalsetimer = 0.2f;

	void Start()
	{

		unitManager = GameObject.FindGameObjectWithTag ("UnitManager").GetComponent<UnitManager> ();
	}
	
	void OnMouseDown()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit info;
		Physics.Raycast (ray, out info, Mathf.Infinity, 1);
		
		corner = info.point;
		
		click = 1;
		dragfalsetimer = 0.2f;
	}
	
	
	void OnMouseDrag()
	{



		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit info;
		Physics.Raycast (ray, out info, Mathf.Infinity, 1);

		if (Vector3.Magnitude(info.point - corner) > 0.1f)
		{
			unitManager.SendMessage("DraggingTrue");
			
			if (click == 1)
			{
				selectorInstance = Instantiate(selector, corner, Quaternion.identity) as GameObject;
				click = 0;
			}
		Vector3 resizeVector = info.point - corner;
		Vector3 newScale = selectorInstance.transform.localScale;
		newScale.x = resizeVector.x;
		newScale.z = -resizeVector.z;
		selectorInstance.transform.localScale = newScale;
		
		}
	}
	
	void OnMouseUp()
	{
		Destroy(selectorInstance);

		dragfalsetimer -= Time.deltaTime;

		if (dragfalsetimer < 0)
		{
			unitManager.SendMessage("DraggingFalse");
			dragfalsetimer = 0;

		}


	}
	
	
}