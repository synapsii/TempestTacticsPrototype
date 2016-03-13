using UnityEngine;
using System.Collections;

public class EnemySelectUnit : MonoBehaviour {
	
	public string[] tags;
	//private bool Select = false;
	
	//private BoxSelection box;

	private UnitManager unitManager;
	private EnemyUnitManager enemyUnitManager;
	
	void Start ()
	{
		unitManager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		enemyUnitManager = GameObject.Find("EnemyUnitManager").GetComponent<EnemyUnitManager> ();
		
		//box = GameObject.FindGameObjectWithTag("Tile").GetComponent<BoxSelection>();
		
	}
	
	void Clicked ()
	{

		unitManager.selectedUnits.Clear ();
		enemyUnitManager.SelectSingleUnit (gameObject);				//select unit

	}



//	void OnTriggerEnter (Collider collider)
//	{
//		foreach(string tag in tags)
//		{
//			if(collider.tag == tag)
//			{
//				Select = true;
//				gameObject.GetComponent<HighlightUnit>().highlight = true;
//				
//				return;
//			}
//		}
//	}
//	
//	void OnTriggerExit (Collider collider)
//	{
//		foreach (string tag in tags)
//		{
//			if(collider.tag == tag)
//			{
//				Select = false;
//				gameObject.GetComponent<HighlightUnit>().highlight = false;
//			}
//		}
//	}
	
	
//	void AddToControlGroup (int i)
//	{
//		unitManager.ControlGroups[i].Add (gameObject);
//	}
	
	
	
	void Update ()
	{
		
//		if(Input.GetMouseButtonUp (0) && unitManager.dragging == true) // right now it's not reading unitManager.dragging fast enough...
//		{
//			if(Select == true)
//			{
//				
//				gameObject.GetComponent<HighlightUnit>().highlight = false;
//				unitManager.SelectAdditionalUnits (gameObject);	
//				
//			}
//			Select = false;
//		}
		
	}
	
	
	
}
