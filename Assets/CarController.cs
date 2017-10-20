using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarController : MonoBehaviour {

	public enum CarDirection
	{
		NORTH,
		SOUTH,
		WEST,
		EAST
	}

	public enum Lane
	{
		LANE1,
		LANE2
	}

	public GameObject motor;
	public float speed;

	// Manager
	bool isTurn;
	CarDirection turnToDirection;

	//	Available Street
	public StreetController STREET_TOP;
	public StreetController STREET_HORIZONTAL;
	public StreetController STREET_BOTTOM;
	public StreetController STREET_LEFT;
	public StreetController STREET_VERTICAL;
	public StreetController STREET_RIGHT;

	//	Street define
//	private CarDirection currentCarDirection;
//	private StreetController currentStreet;
//	private Lane currentLane;
	public CarDirection currentCarDirection;
	public StreetController currentStreet;
	public Lane currentLane;
	public float temp;

	// Use this for initialization
	void Start () {
		//	Firstly run
//		currentCarDirection = CarDirection.NORTH;
//		currentStreet = STREET_VERTICAL;
//		currentLane = Lane.LANE1;

		setFirstPosition ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			this.transform.Rotate (0, 0, 15); 
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			this.transform.Rotate (0, 0, -15);
		}else if(Input.GetKeyDown(KeyCode.Space)){
			this.autoBackToLane ();
		}else {
			this.transform.Rotate(0, 0, 0); 
		}

		if (isTurn) {
			
			if (this.transform.eulerAngles.z >= 345) {
				this.transform.eulerAngles = new Vector3 (0, 0, 0);
				isTurn = false;
				return;
			} else {
				this.transform.Rotate (0, 0, 15, Space.World);	
			}

			float turnOff = 0;

			switch (turnToDirection) {
			case CarDirection.NORTH:
				{
					turnOff = 360;
					break;
				}
			case CarDirection.SOUTH:
				{
					turnOff = 180;
					break;
				}
			case CarDirection.WEST:
				{
					turnOff = 90;
					break;
				}
			case CarDirection.EAST:
				{
					turnOff = 270;
					break;
				}
			}

			if (this.transform.eulerAngles.z >= turnOff)
				isTurn = false;
		}
	}

	void FixedUpdate(){
		Vector3 position = motor.transform.localPosition;
		motor.transform.localPosition = new Vector3 (position.x, position.y+speed, position.z);
		this.transform.position = new Vector3 (motor.transform.position.x, motor.transform.position.y, 0);
		motor.transform.localPosition = position;

		//	Autoback to a lane
//		autoBackToLane();
	}

	//	MARK: Public
	public void turnCar(){
		isTurn = true;
		switch ((int)this.transform.eulerAngles.z) {
		case 0:
			{
				turnToDirection = CarDirection.WEST;
				break;
			}
		case 90:
			{
				turnToDirection = CarDirection.SOUTH;
				break;
			}
		case 180:
			{
				turnToDirection = CarDirection.EAST;
				break;
			}
		case 270:
			{
				turnToDirection = CarDirection.NORTH;
				break;
			}
		}



		currentCarDirection = turnToDirection;

	}

	// MARK: Handle back to lane
	void autoBackToLane(){

		//	Check and update position
		checkStates(onUpdateX, onUpdateY);
	}

	bool onUpdateX(float x, float rotate){
		Vector3 curPos = this.transform.position;
		this.transform.position = new Vector3(x, curPos.y);
		this.transform.eulerAngles = new Vector3 (0, 0, rotate);
		return true;
	}

	bool onUpdateY(float y, float rotate){
		Vector3 curPos = this.transform.position;
		this.transform.position = new Vector3(curPos.x, y);
		this.transform.eulerAngles = new Vector3 (0, 0, rotate);
		return true;
	}

	void checkStates(Func<float,float,bool> onUpdateX, Func<float,float,bool> onUpdateY){
		
		switch(currentStreet.streetStyle){
		case StreetController.StreetStyle.VERTICAL:
			{
				float x = 0;
				float rotate = 0;
				switch (currentCarDirection) {
				case CarDirection.NORTH:
					{
						switch (currentLane) {
						case Lane.LANE1:
							{
								x = currentStreet.limitPointWest;
								break;
							}
						case Lane.LANE2:
							{
								x = currentStreet.limitPointEast;
								break;
							}
						default:
							break;
						}
						rotate = 0;
						break;
					}
				case CarDirection.SOUTH:
					{
						switch (currentLane) {
						case Lane.LANE1:
							{
								x = currentStreet.limitPointEast;
								break;
							}
						case Lane.LANE2:
							{
								x = currentStreet.limitPointWest;
								break;
							}
						default:
							break;
						}
						rotate = 180;
						break;
					}
				default:
					break;
				}

				// Update X
				onUpdateX(x, rotate);

				break;
			}
		case StreetController.StreetStyle.HORIZONTAL:
			{
				float y = 0;
				float rotate = 0;
				switch (currentCarDirection) {
				case CarDirection.WEST:
					{
						switch (currentLane) {
						case Lane.LANE1:
							{
								y = currentStreet.limitPointSouth;
								break;
							}
						case Lane.LANE2:
							{
								y = currentStreet.limitPointNorth;
								break;
							}
						default:
							break;
						}
						rotate = 90;
						break;
					}
				case CarDirection.EAST:
					{
						switch (currentLane) {
						case Lane.LANE1:
							{
								y = currentStreet.limitPointNorth;
								break;
							}
						case Lane.LANE2:
							{
								y = currentStreet.limitPointSouth;
								break;
							}
						default:
							break;
						}
						rotate = -90;
						break;
					}
				default:
					break;
				}

				// Update Y
				onUpdateY(y, rotate);

				break;
			}
		default:
			break;
		}
	}

	//	MARK: Handle
	void setFirstPosition(){
		float x = temp;
		float y = temp;
		float rotate = 0;
		switch (currentStreet.streetStyle) {
		case StreetController.StreetStyle.VERTICAL:
			{
				//	Set X
				switch (currentLane) {
				case Lane.LANE1:
					{
						x = currentStreet.limitPointWest;
						break;
					}
				case Lane.LANE2:
					{
						x = currentStreet.limitPointEast;
						break;
					}
				}

				//	Set Y
//				y = currentStreet.limitPointSouth;
				break;
			}
		case StreetController.StreetStyle.HORIZONTAL:
			{
				//	Set Y
				switch (currentLane) {
				case Lane.LANE1:
					{
						y = currentStreet.limitPointSouth;
						break;
					}
				case Lane.LANE2:
					{
						y = currentStreet.limitPointNorth;
						break;
					}
				}

				//	Set X
//				x = currentStreet.limitPointWest;
				break;
			}
		default:
			break;
		}

		//	Set Rotate
		switch(currentCarDirection){
		case CarDirection.NORTH:
			{
				rotate = 0;
				break;
			}
		case CarDirection.SOUTH:
			{
				rotate = 180;
				break;
			}
		case CarDirection.WEST:
			{
				rotate = 90;
				break;
			}
		case CarDirection.EAST:
			{
				rotate = -90;
				break;
			}
		default:
			break;
		}

		print (x + " " + y + " " + rotate);

		this.transform.position = new Vector3 (x, y);
		this.transform.Rotate (0, 0, rotate);
	}
}
