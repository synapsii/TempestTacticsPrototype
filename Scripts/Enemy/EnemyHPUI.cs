using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHPUI : MonoBehaviour {
	
	public EnemyUnitManager enemyUnitManager;
	public Image HPBar;
	
	
	void Start () {
		
		enemyUnitManager = GameObject.Find ("EnemyUnitManager").GetComponent<EnemyUnitManager>();
		//movement = transform.parent.parent.GetComponent<BasicUnitMovement>();
		
		HPBar = gameObject.GetComponentInChildren<Image>();
		HPBar.fillAmount = 0f;
		enemyUnitManager.EnemyUnitList[0].hp = 100f;
	}
	
	
	void Update () {
		
		if (enemyUnitManager.EnemyUnitList[0].hp < 100f)
		{
			HPBar.fillAmount = (enemyUnitManager.EnemyUnitList[0].hp / 100.0f);
		}
		else
		{
			HPBar.fillAmount = 0f;
		}
	}
}
