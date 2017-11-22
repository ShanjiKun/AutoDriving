using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour {

	public enum Direction
	{
		VERTICAL,
		HORIZONTAL
	}

	public Direction direction;
	public Lane laneLeft;
	public Lane laneRight;

	private float TURN_EASY_Rotate = 10f;
	private float TURN_MEDIUM_Rotate = 20f;
	private float TURN_HARD_Rotate = 30f;

	//	Lane
	public Lane switchLane(Lane lane)
	{
		if (lane == laneLeft)
			return laneRight;

		return laneLeft;
	}

	//`Get turnRotate
	public float turnRotate(Lane currentLane, Car.TurnToLane turnType)
	{
		float rotate;
		switch (turnType) {
		case Car.TurnToLane.EASY:
			{
				rotate = TURN_EASY_Rotate;

			}
			break;
		case Car.TurnToLane.MEDIUM:
			{
				rotate = TURN_MEDIUM_Rotate;
			}
			break;
		default:
			{
				rotate = TURN_HARD_Rotate;
			}
			break;
		}

		if(currentLane == laneLeft)
			return -rotate;

		return rotate;
	}
}
