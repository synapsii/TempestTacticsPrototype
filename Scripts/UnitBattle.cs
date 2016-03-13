using UnityEngine;
using System.Collections;

public class UnitBattle : MonoBehaviour {

	private UnitAbilities abilities;
	private UnitManager unitManager;
	private UnitCollision unit;

	private GameObject targetedenemy;
	private Vector3 newclickposition;

	public float attackRange;
	
	public bool casting;



	void Start () {
	
		abilities = gameObject.GetComponent<UnitAbilities>();
		casting = false;

		unitManager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		unit = gameObject.GetComponent<UnitCollision>();
		unitManager.UnitList[unit.id].attackRange = attackRange;

	}


	void Attack (GameObject enemy)
	{

		if (Vector3.SqrMagnitude(transform.position - enemy.transform.position) < attackRange * attackRange)
		{

			//check for id
			//abilitycd = ##

			abilities.abilitycd = 1.5f;
			abilities.abilitycdglobal = 1.5f;

			targetedenemy = enemy;
			casting = true;

			gameObject.SendMessage("QueueAttack", enemy);

		}
		else
		{
			newclickposition = transform.position + 
				Vector3.Normalize(enemy.transform.position - transform.position)*(Vector3.Magnitude(enemy.transform.position - transform.position)-(attackRange-0.5f));
			gameObject.SendMessage("MoveOrder", newclickposition);
			gameObject.SendMessage("QueueAttack", enemy);
		}
	}




	void Update () {
	
		if (casting == true)
		{
			if (abilities.abilitycd == 0.0f)
			{
				targetedenemy.SendMessage("TakeDamage", 10.0f);
				casting = false;
			}
		}



	}
}
