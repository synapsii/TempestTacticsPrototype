using UnityEngine;
using System.Collections;

public class UnitAbilities : MonoBehaviour {

	public float abilitycd;
	public float abilitycdglobal;
	public BasicUnitMovement movement;
	private int casttype;

	void Start () {
	
		movement = gameObject.GetComponent<BasicUnitMovement>();
		abilitycdglobal = 3.0f;


	}



	void Update () {
	
		if (abilitycd > 0.0f) {				//if ability global is on cooldown
			abilitycd -= Time.deltaTime;		//incrementally lower movement cd
		} 
		else 
		{
			abilitycd = 0.0f;					//lock cd at 0 once below 0	
		}


		if (Input.GetKey (KeyCode.Q) && movement.moving == false)
		{
			abilitycd = abilitycdglobal;
		}

		//----------------------------------------------CASTING/ATTACKING MODES-------------------------





	}
}
