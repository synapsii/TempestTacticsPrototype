using UnityEngine;
using System.Collections;

public class EnemyHighlightUnit : MonoBehaviour {
	
	private Color originalColor;
	private EnemyUnitManager enemyUnitManager;
	//private BasicUnitMovement movement;
	
	private Color highlightedColor = new Color(0.85f, 0.25f, 0.15f, 0.3f);
	public float highlightTime = 0.0f;
	
	public bool highlight;
	
	void Start () 
	{
		//GameObject unitManagerObject = GameObject.FindGameObjectWithTag ("UnitManager");
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();
		
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
		
		
		if (enemyUnitManager.IsSelected (gameObject) || highlight == true) 	//if unit is selected, change to highlighted color
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
