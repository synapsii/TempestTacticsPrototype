using UnityEngine;
using System.Collections;

public class HighlightUnit : MonoBehaviour {
	
	private Color originalColor;
	private UnitManager unitManager;
	//private BasicUnitMovement movement;
	
	private Color highlightedColor = new Color(0.35f, 0.85f, 0.45f, 0.3f);
	public float highlightTime = 0.0f;

	public bool highlight;

	void Start () 
	{
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag ("UnitManager");
		unitManager = unitManagerObject.GetComponent<UnitManager>();
		
		//GameObject playerUnitObject = GameObject.Find("Unit 0");
		//movement = playerUnitObject.GetComponent<BasicUnitMovement>();
		
		
		originalColor = renderer.material.color;				//make sure script remembers original color
	}


	void RightClicked ()
	{
		highlightTime = 0.2f;									//briefly highlight if right clicked
	}

	void Update () 
	{


		if (unitManager.IsSelected (gameObject) || highlight == true) 	//if unit is selected, change to highlighted color
		{
			renderer.material.color = highlightedColor;
		}

		else
		{
			if (highlightTime > 0.0f)
			{
				highlightTime -= Time.deltaTime;
				renderer.material.color = highlightedColor;		//countodown for right click on highlight
			}

			else
			{
				highlightTime = 0.0f;
				renderer.material.color = originalColor;			//else use original color
			}
		}

	}
}
