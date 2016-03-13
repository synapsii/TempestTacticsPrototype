using UnityEngine;
using System.Collections;

public class EnemyBattle : MonoBehaviour {

	private UnitManager unitManager;
	private EnemyUnitManager enemyUnitManager;
	private EnemyUnitCollision unit;
	//private float damage;

	void Start () {

		unitManager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();
		unit = gameObject.GetComponent<EnemyUnitCollision>();

	}


	void Clicked ()
	{
		//if in casting state && single target spell
		//make sure to turn on raycasts for enemy units if single target

		foreach (GameObject unit in unitManager.selectedUnits)
		{
			if (unitManager.casttype == 3)		// 3 = attack move
			{
				unit.SendMessage ("Attack", gameObject, SendMessageOptions.DontRequireReceiver);
			}

			unitManager.casttype = 99;
		}

	}


	void RightClicked ()
	{
		//Debug.Log ("rightclick received");

		foreach (GameObject unit in unitManager.selectedUnits)
		{

			unit.SendMessage ("Attack", gameObject, SendMessageOptions.DontRequireReceiver);
			//Debug.Log ("attack sent");
		}

	}


	void TakeDamage (float damage)
	{
		enemyUnitManager.EnemyUnitList[unit.id].hp = enemyUnitManager.EnemyUnitList[unit.id].hp - damage;
		Debug.Log ("damage taken; hp = " + enemyUnitManager.EnemyUnitList[unit.id].hp);
	}




	void Update () 
	{
	
	}
}
