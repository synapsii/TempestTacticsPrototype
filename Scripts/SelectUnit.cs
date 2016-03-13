using UnityEngine;
using System.Collections;

public class SelectUnit : MonoBehaviour {

	public string[] tags;
	private bool Select = false;

	//private BoxSelection box;

	private UnitManager unitManager;
	
	void Start ()
	{
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag ("UnitManager");
		unitManager = unitManagerObject.GetComponent<UnitManager> ();

		//box = GameObject.FindGameObjectWithTag("Tile").GetComponent<BoxSelection>();

	}
	
	void Clicked ()
	{
		
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) 
		{
			if(unitManager.IsSelected(gameObject))
			{
				unitManager.DeselectSingleUnit (gameObject);		//deselect if shift + l.click already selected unit
			}
			else
			{
				unitManager.SelectAdditionalUnits (gameObject);		//add to selection if not
			}
		}
		else 
		{
			unitManager.SelectSingleUnit (gameObject);				//select unit
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		foreach(string tag in tags)
		{
			if(collider.tag == tag)
			{
				Select = true;
				gameObject.GetComponent<HighlightUnit>().highlight = true;

				return;
			}
		}
	}

	void OnTriggerExit (Collider collider)
	{
		foreach (string tag in tags)
		{
			if(collider.tag == tag)
			{
				Select = false;
				gameObject.GetComponent<HighlightUnit>().highlight = false;
			}
		}
	}


	void AddToControlGroup (int i)
	{
		unitManager.ControlGroups[i].Add (gameObject);
	}



	void Update ()
	{

		if(Input.GetMouseButtonUp (0) && unitManager.dragging == true) // right now it's not reading unitManager.dragging fast enough...
		{
			if(Select == true)
			{

				gameObject.GetComponent<HighlightUnit>().highlight = false;
				unitManager.SelectAdditionalUnits (gameObject);	

			}
			Select = false;
		}



//		if((Input.GetKey (KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
//		{
//			if (Input.GetKey (KeyCode.Alpha1))
//			{
//				unitManager.ControlGroups[1].Clear();
//				unitManager.ControlGroups[1].Add (gameObject);
//			}
//		}
//
//
//		if (Input.GetKey(KeyCode.Alpha1))
//		{
//			foreach (GameObject unit in unitManager.ControlGroups[1])
//			{
//				unitManager.SelectAdditionalUnits (gameObject);
//			}
//		}

	}



}
