using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//	Define Streets

	//	Define Crossroads
	public List<Crossroads> crossroadsList;

	// Define Car
	public CarController car;
	public List<Crossroads> crossroadsAhead;
	public List<Crossroads> crossroadsOnStreet;
	public Crossroads crossroadsNearest;
	public float distance;
	// Manger
	bool isTurning;

	// First call
	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			followCar ();
			findCrossroadsNearest ();
		}

		checkDistanceCarAndCrossroads ();
	}

	//	MARK: Handles turn anyway
	void turnCar(){
		car.turnCar ();
		print ("Turn left");
	}

	void changeStreet(){
		car.currentStreet = crossroadsNearest.changeStreet (car.currentStreet);
		followCar ();
		findCrossroadsNearest ();
	}

	//	MARK: Handles Find Crossroads
	void checkDistanceCarAndCrossroads(){

		if (crossroadsNearest == null)
			return;

		distance = 0;

		switch (car.currentCarDirection) {
		case CarController.CarDirection.NORTH:
		case CarController.CarDirection.SOUTH:
			{
				distance = Mathf.Abs (car.transform.position.y - crossroadsNearest.transform.position.y);
				break;
			}
		case CarController.CarDirection.WEST:
		case CarController.CarDirection.EAST:
			{
				distance = Mathf.Abs (car.transform.position.x - crossroadsNearest.transform.position.x);
				break;
			}
		default:
			print("Distance WTF");
			break;
		}
		if (!isTurning && distance <= 0.7f && distance >= 0.05f) {
			isTurning = true;
			turnCar ();
			changeStreet ();
			print ("Turning");
		} else if(isTurning && distance > 0.7f) {
			isTurning = false;
			print ("TurningOff");
		}


	}

	void findCrossroadsNearest(){

		crossroadsNearest = null;

		if (crossroadsAhead.Count == 0)
			return;

		switch (car.currentCarDirection) {
		case CarController.CarDirection.NORTH:
		case CarController.CarDirection.SOUTH:
			{
				float y = Mathf.Abs(crossroadsAhead[0].transform.position.y);
				int index = 0;
				for (int i = 1; i < crossroadsAhead.Count; i++) {
					Crossroads cr = crossroadsAhead [i];
					if (y > Mathf.Abs(cr.transform.position.y)) {
						y = Mathf.Abs(cr.transform.position.y);
						index = i;
					}
				}
				crossroadsNearest = crossroadsAhead [index];
				break;
			}
		case CarController.CarDirection.WEST:
		case CarController.CarDirection.EAST:
			{
				float x = Mathf.Abs(crossroadsAhead[0].transform.position.x);
				int index = 0;
				for (int i = 1; i < crossroadsAhead.Count; i++) {
					Crossroads cr = crossroadsAhead [i];
					if (x > Mathf.Abs(cr.transform.position.x)) {
						x = Mathf.Abs(cr.transform.position.x);
						index = i;
					}
				}
				crossroadsNearest = crossroadsAhead [index];
				break;
			}
		}
	}

	void followCar() {

		crossroadsAhead = new List<Crossroads> ();
		//	Find out crossroads on street
		crossroadsOnStreet = findCrossroadsOntreet();
		print (crossroadsOnStreet.Count);
		//	Check Direction to find out crossroads in front of the car.
		switch(car.currentCarDirection){
		case CarController.CarDirection.NORTH:
			{
				// Find out crossroads based on increment Y.
				for(int i=0; i<crossroadsOnStreet.Count; i++){
					Crossroads crossroads = crossroadsOnStreet [i];
					if (car.transform.position.y <= crossroads.transform.position.y) {
						crossroadsAhead.Add (crossroads);
					}
				}
				break;
			}
		case CarController.CarDirection.SOUTH:
			{
				// Find out crossroads based on descrement Y.
				for(int i=0; i<crossroadsOnStreet.Count; i++){
					Crossroads crossroads = crossroadsOnStreet [i];
					if (car.transform.position.y >= crossroads.transform.position.y) {
						crossroadsAhead.Add (crossroads);
					}
				}
				break;
			}
		case CarController.CarDirection.WEST:
			{
				// Find out crossroads based on descrement X.
				for(int i=0; i<crossroadsOnStreet.Count; i++){
					Crossroads crossroads = crossroadsOnStreet [i];
					if (car.transform.position.x >= crossroads.transform.position.x) {
						crossroadsAhead.Add (crossroads);
					}
				}
				break;
			}
		case CarController.CarDirection.EAST:
			{
				// Find out crossroads based on increment X.
				for(int i=0; i<crossroadsOnStreet.Count; i++){
					Crossroads crossroads = crossroadsOnStreet [i];
					if (car.transform.position.x <= crossroads.transform.position.x) {
						crossroadsAhead.Add (crossroads);
					}
				}
				break;
			}
		default:
			print ("WTF");
			break;
		}
	}

	List<Crossroads> findCrossroadsOntreet() {
		List<Crossroads> crossroadsOnStreet = new List<Crossroads> ();
		for (int i = 0; i < crossroadsList.Count; i++) {
			Crossroads crossroads = crossroadsList [i];
			for (int j = 0; j < crossroads.streetList.Count; j++) {
				StreetController street = crossroads.streetList [j];
				if (street == car.currentStreet) {
					crossroadsOnStreet.Add (crossroads);
					break;
				}
			}
		}
		return crossroadsOnStreet;
	}
}
