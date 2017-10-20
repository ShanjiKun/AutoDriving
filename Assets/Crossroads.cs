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
	public List<StreetController> streetList;

	public StreetController changeStreet(StreetController curStreet){
		for (int i = 0; i < streetList.Count; i++) {
			if (streetList [i] != curStreet)
				return streetList [i];
		}
		return streetList [0];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
