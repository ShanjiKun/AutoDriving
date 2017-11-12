using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour {

	public enum Type 
	{
		AHEAD,
		FRONT,
		RIGHT,
		BOTTOM,
		LEFT,
		BODY
	}

	public Type type;
	public Car car;
	public List<GameObject> ListObject = new List<GameObject> ();

	//	Handle
	float distanceToEnemy(GameObject enemy)
	{
		Vector2 carPos = car.transform.position;
		Vector2 enePos = enemy.transform.position;
		return Mathf.Sqrt (Mathf.Pow((carPos.x - enePos.x), 2) + Mathf.Pow((carPos.y - enePos.y), 2));
	}

	//	Trigger Callback
	void OnTriggerEnter2D(Collider2D other){
		car.onDetectionEnter(type, other);

		//	Add to list
		ListObject.Add (other.gameObject);
	}

	void OnTriggerExit2D(Collider2D other){
		car.onDetectionExit(type, other);

		//	Remove from list
		ListObject.Remove (other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other){
		car.onDetectionStay(type, other);
//		print (other.tag);
	}
}
