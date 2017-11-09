using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroads : MonoBehaviour {

	public enum CRPosition
	{
		CR_LEFT_TOP,
		CR_MID_TOP,
		CR_RIGHT_TOP,
		CR_LEFT_MIDDLE,
		CR_MID_MIDDLE,
		CR_RIGHT_MIDDLE,
		CR_LEFT_BOTTOM,
		CR_MID_BOTTOM,
		CR_RIGHT_BOTTOM
	}

	public CRPosition position;
	public Street street1;
	public Street street2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
