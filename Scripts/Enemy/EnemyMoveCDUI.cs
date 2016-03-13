using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMoveCDUI : MonoBehaviour {
	
	public EnemyUnitMovement movement;
	//public Canvas movecdcanvas;
	public Image movecdimage;
	//private float movecdfill;
	
	// Use this for initialization
	void Start () {
		
		//Debug.Log ("test");
		
		movement = transform.parent.parent.GetComponent<EnemyUnitMovement>();
		
		//movecdcanvas = movement.GetComponent<Canvas>();
		//movecdimage = movecdcanvas.GetComponent<Image>();
		
		//movecdimage = transform.parent.GetComponent<Image>();
		
		
		
		//Debug.Log (movecdimage.fillAmount);
		
		//movecdfill = transform.parent.GetComponentInChildren("Image (Script)");
		
		movecdimage = gameObject.GetComponentInChildren<Image>();
		movecdimage.fillAmount = 0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (movement.movecd > 0)
		{
			movecdimage.fillAmount = (movement.movecd / movement.movecdglobal);
		}
		else
		{
			movecdimage.fillAmount = 0f;
		}
	}
}
