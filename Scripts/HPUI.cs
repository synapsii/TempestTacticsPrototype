using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPUI : MonoBehaviour {
	
	public UnitManager unitManager;
	public Image HPBar;
	
	
	void Start () {
		
		unitManager = GameObject.Find ("UnitManager").GetComponent<UnitManager>();
		//movement = transform.parent.parent.GetComponent<BasicUnitMovement>();
		
		HPBar = gameObject.GetComponentInChildren<Image>();
		HPBar.fillAmount = 0f;
		unitManager.UnitList[0].hp = 100f;
	}
	
	
	void Update () {
		
		if (unitManager.UnitList[0].hp < 100f)
		{
			HPBar.fillAmount = (unitManager.UnitList[0].hp / 100.0f);
		}
		else
		{
			HPBar.fillAmount = 0f;
		}
	}
}
