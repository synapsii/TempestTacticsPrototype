using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityCastUI : MonoBehaviour {

	public UnitAbilities abilities;
	//public BasicUnitMovement movement;
	public Image abilityCastImage;


	void Start () {
	
		abilities = transform.parent.parent.GetComponent<UnitAbilities>();
		//movement = transform.parent.parent.GetComponent<BasicUnitMovement>();

		abilityCastImage = gameObject.GetComponentInChildren<Image>();
		abilityCastImage.fillAmount = 0f;

	}
	

	void Update () {
	
		if (abilities.abilitycd > 0)
		{
			abilityCastImage.fillAmount = (abilities.abilitycd / abilities.abilitycdglobal);
		}
		else
		{
			abilityCastImage.fillAmount = 0f;
		}

	}
}
